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
        public bool Connected => connected;

        public bool ReconnectEnable { get; set; } = true;
        private int tryConnectCounter = 0;
        private bool started = false;
        private bool stopping = false;

        internal SocketEventHandler<TryConnectingEventArgs<ITcpConnection>> OnTryConnecting { get; set; }
        internal ITryConnectionStrategy TryConnectionStrategy { get; set; }

        internal TcpConnection(Socket socket)
        {
            stopping = false;
            created = false;
            if (socket.LocalEndPoint == null && socket.RemoteEndPoint == null) throw new ArgumentException("This socket is not connected.");

            this.Instance = socket;
            this.LocalEndPoint = socket.LocalEndPoint.As();
            this.RemoteEndPoint = socket.RemoteEndPoint.As();

            this.Key = RemoteEndPoint.ToString();
            this.Name = this.Name ?? this.Key;
            
            connected = true;
        }

        internal TcpConnection(IPEndPoint remote, IPEndPoint local)
        {
            stopping = false;
            _remote = remote;
            _local = local;
            OnConstructing();
        }

        private void OnConstructing()
        {
            created = true;
            try
            {
                if (_remote == null) throw new ArgumentNullException(nameof(_remote));

                AddressFamily family = _remote.AddressFamily;

                if (_remote.Port.NotIn(IPEndPoint.MinPort, IPEndPoint.MaxPort)) throw new ArgumentOutOfRangeException(nameof(_remote.Port), $"The {nameof(_remote.Port)} must between {IPEndPoint.MinPort} to {IPEndPoint.MaxPort}.");

                if (_remote.Port < 1) throw new ArgumentOutOfRangeException(nameof(_remote.Port));

                switch (family)
                {
                    case AddressFamily.InterNetwork:
                    case AddressFamily.InterNetworkV6:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(family), $"The AddressFamily only support AddressFamily.InterNetwork and AddressFamily.InterNetworkV6.");
                }

                if (_local != null && _local.AddressFamily != family) throw new ArgumentException($"The {nameof(_local)} and {nameof(family)} not match.");
                if (Instance != null)
                {
                    try
                    {
                        Instance.Shutdown(SocketShutdown.Both);
                    }
                    catch (Exception)
                    {
                    }

                    try
                    {
                        Instance.Close();
                    }
                    catch (Exception)
                    {
                    }

                    Instance = null;
                }

                Instance = new Socket(family, SocketType.Stream, ProtocolType.Tcp);
                if (_local != null && _local.Port > 0)
                {
                    Instance.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                    Instance.Bind(_local);
                }

                this.RemoteEndPoint = _remote;
                if (_local?.Port > 0) this.LocalEndPoint = _local;

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
                connected = false;
                if (!ReconnectEnable) return;
                if (!created) return;
                if (stopping) return;
                if (tryConnectCounter > 0) return;
                bool ret = false;

                do
                {
                    try
                    {
                        if (stopping) return;
                        Interlocked.Increment(ref tryConnectCounter);
                        //tryConnectCounter++;
                        OnTryConnecting?.Invoke(new TryConnectingEventArgs<ITcpConnection>(this, tryConnectCounter));
                        OnConstructing();
                        ret = ConnectInternal();
                    }
                    catch (Exception ex)
                    {
                        OnException?.Invoke(new NetClientEventArgs<ITcpConnection>(this) { Exception = ex });
                    }
                    finally
                    {

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
                    }
                } while (!ret);
            }
            finally
            {
            }
        }

        public override void Start()
        {
            stopping = false;
            started = ConnectInternal();
            if (!started) ThreadPool.QueueUserWorkItem((o) => { Reconnect(); });
        }

        private bool ConnectInternal()
        {
            try
            {
                if (Listener?.Running == true) return true;

                var ret = connected || Connect(ConnectTimeout);

                if (ret)
                {
                    Interlocked.Exchange(ref tryConnectCounter, 0);
                    //tryConnectCounter = 0;
                    var listener = new TcpListener(this);
                    listener.KeepAlive(KeepAliveOption);
                    Listener = listener;
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
            stopping = true;
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
            var data = new ConnectionData();
            connected = false;
            Instance.BeginConnect(RemoteEndPoint.As(), ConnectCallback, data);

            var ret = data.WaitConnected(millisecondsTimeout);

            if (!ret) data.Connected = false;

            data.WaitCompleted();

            if (!ret) throw new SocketException((int)SocketError.TimedOut);

            if (data.Connected != false || data.Exception == null) return data.Connected == true;
            OnException?.Invoke(new NetClientEventArgs<ITcpConnection>(this) { Exception = data.Exception });
            if (created) throw data.Exception;
            return false;
        }

        private void ConnectCallback(IAsyncResult result)
        {
            if (!(result.AsyncState is ConnectionData data))
            {
                return;
            }
            
            bool? isConnect = null;

            data.SetConnected();

            try
            {
                Instance.EndConnect(result);

                isConnect = false;

                if (data.Connected == false)
                {
                    connected = false;
                    return;
                }

                RemoteEndPoint = Instance.RemoteEndPoint.As();
                LocalEndPoint = Instance.LocalEndPoint.As();

                connected = true;
                OnConnected?.Invoke(new NetClientEventArgs<ITcpConnection>(this));

                data.Connected = true;
                isConnect = true;
            }
            catch (Exception ex)
            {
                data.Connected = false;
                data.Exception = ex;
            }
            finally
            {
                if (isConnect == false && data.Connected == false)
                {
                    try
                    {
                        Instance?.Shutdown(SocketShutdown.Both);
                    }
                    catch (Exception e)
                    {
                        data.Exception = data.Exception ?? e;
                        OnException?.Invoke(new NetClientEventArgs<ITcpConnection>(this) { Exception = data.Exception });
                    }

                    try
                    {
                        Instance?.Close();
                    }
                    catch (Exception e)
                    {
                        data.Exception = data.Exception ?? e;
                        OnException?.Invoke(new NetClientEventArgs<ITcpConnection>(this) { Exception = data.Exception });
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