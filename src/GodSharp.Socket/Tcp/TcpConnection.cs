using GodSharp.Sockets.Abstractions;
using GodSharp.Sockets.Extensions;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace GodSharp.Sockets.Tcp
{
    public sealed class TcpConnection : NetConnection<ITcpConnection, NetClientEventArgs<ITcpConnection>>, ITcpConnection, IDisposable
    {
        bool connected { get; set; } = false;
        bool created { get; set; }
        IPEndPoint _remote { get; set; }
        IPEndPoint _local { get; set; }

        public ITcpListener Listener { get; internal set; }

        public int ConnectTimeout { get; internal set; } = 3000;

        public bool ReconnectEnable { get; set; } = true;
        private int tryConnectCounter = 0;
        private bool started = false;
        private bool connecting = false;

        internal SocketEventHandler<TryConnectingEventArgs<ITcpConnection>> OnTryConnecting { get; set; }
        internal ITryConnectionStrategy TryConnectionStrategy { get; set; }

        internal TcpConnection(Socket socket)
        {
            created = false;
            if (socket.LocalEndPoint == null && socket.RemoteEndPoint == null) throw new ArgumentException("This socket is not connected.");

            this.Instance = socket;
            this.LocalEndPoint = socket.LocalEndPoint.As();
            this.RemoteEndPoint = socket.RemoteEndPoint.As();

            this.Key = RemoteEndPoint.ToString();
            this.Name = this.Name ?? this.Key;
            
            connected = true;
        }

        internal TcpConnection(IPEndPoint remote, IPEndPoint local) => OnConstructing(remote, local);

        private void OnConstructing(IPEndPoint remote, IPEndPoint local)
        {
            _remote = remote;
            _local = local;
            created = true;
            try
            {
                if (remote == null) throw new ArgumentNullException(nameof(remote));

                AddressFamily family = remote.AddressFamily;

                if (remote.Port.NotIn(IPEndPoint.MinPort, IPEndPoint.MaxPort)) throw new ArgumentOutOfRangeException(nameof(remote.Port), $"The {nameof(remote.Port)} must between {IPEndPoint.MinPort} to {IPEndPoint.MaxPort}.");

                if (remote.Port < 1) throw new ArgumentOutOfRangeException(nameof(remote.Port));

                switch (family)
                {
                    case AddressFamily.InterNetwork:
                    case AddressFamily.InterNetworkV6:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(family), $"The AddressFamily only support AddressFamily.InterNetwork and AddressFamily.InterNetworkV6.");
                }

                if (local != null && local.AddressFamily != family) throw new ArgumentException($"The {nameof(local)} and {nameof(family)} not match.");

                Instance = null;
                Instance = new Socket(family, SocketType.Stream, ProtocolType.Tcp);
                if (local != null && local.Port > 0)
                {
                    //Instance.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                    Instance.Bind(local);
                }

                this.RemoteEndPoint = remote;
                if (local?.Port > 0) this.LocalEndPoint = local;

                this.Key = RemoteEndPoint.ToString();
                this.Name = this.Name ?? this.Key;
            }
            catch (Exception ex)
            {
                OnException?.Invoke(new NetClientEventArgs<ITcpConnection>(this) { Exception = ex });
                if (!ReconnectEnable) throw ex;
            }
        }

        internal void Reconnect()
        {
            try
            {
                if (!ReconnectEnable) return;
                if (!created) return;
                connecting = true;
                bool ret = false;

                do
                {
                    tryConnectCounter++;
                    Console.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}]try connect to {_remote.As()} {tryConnectCounter} ...");
                    OnTryConnecting?.Invoke(new TryConnectingEventArgs<ITcpConnection>(this, tryConnectCounter));
                    OnConstructing(_remote, _local);
                    ret = ConnectInternal();

                    if (!ret)
                    {
                        try
                        {
                            if (TryConnectionStrategy != null) Thread.Sleep(TryConnectionStrategy.Handle(tryConnectCounter));
                        }
                        catch (Exception ex)
                        {
                            OnException?.Invoke(new NetClientEventArgs<ITcpConnection>(this) { Exception = ex });
                        }
                    }
                } while (!ret);
            }
            finally
            {
                connecting = false;
            }
        }

        public override void Start()
        {
            started = ConnectInternal();
            if (!started) ThreadPool.QueueUserWorkItem((o) => { Reconnect(); });
        }

        private bool ConnectInternal()
        {
            try
            {
                if (Listener?.Running == true) return true;

                bool ret = connected ? true : Connect(ConnectTimeout);

                if (ret)
                {
                    tryConnectCounter = 0;
                    Listener = new TcpListener(this);
                    Listener.Start();
                }

                if (Listener?.Running == true) OnStarted?.Invoke(new NetClientEventArgs<ITcpConnection>(this));

                if (ret)
                {
                    if (!started) started = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                OnException?.Invoke(new NetClientEventArgs<ITcpConnection>(this) { Exception = ex });

                //if (!connected && !created && !connecting) throw ex;
                if (created && !ReconnectEnable) throw ex;
            }

            return false;
        }

        public override void Stop()
        {
            if (Listener == null) return;
            if (!Listener.Running) return;

            try
            {
                Listener?.Stop();

                OnStopped?.Invoke(new NetClientEventArgs<ITcpConnection>(this));
            }
            catch (Exception ex)
            {
                OnException?.Invoke(new NetClientEventArgs<ITcpConnection>(this) { Exception = ex });
            }
        }

        private bool Connect(int millisecondsTimeout = -1)
        {
            ConnectionData data = new ConnectionData();
            Instance.BeginConnect(this.RemoteEndPoint.As(), ConnectCallback, data);

            bool ret = data.WaitConnected(millisecondsTimeout);

            if (!ret) data.Connected = ret;

            data.WaitCompleted();

            if (!ret) throw new SocketException((int)SocketError.TimedOut);

            if (data.Connected == false && data.Exception != null)
            {
                OnException?.Invoke(new NetClientEventArgs<ITcpConnection>(this) { Exception = data.Exception });
                if (created) throw data.Exception;

                return false;
            }

            return data.Connected == true;
        }

        private void ConnectCallback(IAsyncResult result)
        {
            ConnectionData data = result.AsyncState as ConnectionData;
            bool? connected = null;

            data.SetConnected();

            try
            {
                Instance.EndConnect(result);

                connected = false;

                if (data.Connected == false) return;

                Console.WriteLine("tcp.client connected");

                this.RemoteEndPoint = Instance.RemoteEndPoint.As();
                this.LocalEndPoint = Instance.LocalEndPoint.As();

                OnConnected?.Invoke(new NetClientEventArgs<ITcpConnection>(this));

                data.Connected = true;
                connected = true;
            }
            catch (Exception ex)
            {
                data.Connected = false;
                data.Exception = ex;
            }
            finally
            {
                Console.WriteLine($"tcp.client ConnectCallback connected:{connected}");

                if (connected == false && data.Connected == false)
                {
                    try
                    {
                        Instance.Close();
                    }
                    catch (Exception ex)
                    {
                        data.Exception = data.Exception ?? ex;
                    }
                }

                data.SetCompleted();
            }
        }

        public override void Dispose() => Listener?.Dispose();

        private class ConnectionData
        {
            private EventWaitHandle ConnectedWaitHandle { get; set; }
            private EventWaitHandle CompletedWaitHandle { get; set; }

            public bool? Connected { get; set; }

            public Exception Exception { get; set; }

            public bool WaitConnected(int millisecondsTimeout = -1) => millisecondsTimeout < 1 ? ConnectedWaitHandle.WaitOne() : ConnectedWaitHandle.WaitOne(millisecondsTimeout);

            public void SetConnected() => ConnectedWaitHandle.Set();

            public bool WaitCompleted() => CompletedWaitHandle.WaitOne();

            public bool SetCompleted() => CompletedWaitHandle.Set();

            public ConnectionData()
            {
                ConnectedWaitHandle = new ManualResetEvent(false);
                CompletedWaitHandle = new ManualResetEvent(false);
            }

            public ConnectionData(bool connected, Exception exception = null)
            {
                Connected = connected;
                Exception = exception;
            }
        }
    }
}