using GodSharp.Sockets.Abstractions;
using GodSharp.Sockets.Extensions;
using GodSharp.Sockets.Tcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace GodSharp.Sockets
{
    public sealed class TcpServer :NetBase<ITcpConnection, NetServerEventArgs>, ITcpServer, IDisposable
    {
        private readonly object ConnectionsLock = new object();

        private bool stopping = false;

        private bool running;
        public override bool Running => running;

        public Socket Instance { get; private set; }

        public IPEndPoint LocalEndPoint { get; private set; }
        
        public IDictionary<string, ITcpConnection> Connections { get; private set; } = new Dictionary<string, ITcpConnection>();

        public SocketEventHandler<NetServerEventArgs> OnServerException { get; set; }

        public TcpServer(TcpServerOptions options) => OnConstructing(options);

        public TcpServer(int port = 7788, string host = null, int backlog = int.MaxValue, AddressFamily family = AddressFamily.InterNetwork, string name = null, int id = 0)
        {
            try
            {
                TcpServerOptions options = new TcpServerOptions(new IPEndPoint(host.IsNullOrWhiteSpace() ? (family == AddressFamily.InterNetworkV6 ? IPAddress.IPv6Any : IPAddress.Any) : IPAddress.Parse(host), port), backlog, family) { Id = id, Name = name };

                OnConstructing(options);
            }
            catch (Exception ex)
            {
                OnServerException?.Invoke(new NetServerEventArgs(this, LocalEndPoint) { Exception = ex });
                throw ex;
            }
        }

        private void OnConstructing(TcpServerOptions options)
        {
            try
            {
                if (options == null) throw new ArgumentNullException(nameof(options));
                if (options.LocalEndPoint == null) throw new ArgumentNullException(nameof(options.LocalEndPoint));
                if (options.LocalEndPoint.Port.NotIn(IPEndPoint.MinPort, IPEndPoint.MaxPort)) throw new ArgumentOutOfRangeException(nameof(options.LocalEndPoint.Port), $"The {nameof(options.LocalEndPoint.Port)} must between {IPEndPoint.MinPort} to {IPEndPoint.MaxPort}.");
                if (options.LocalEndPoint.Port < 1) throw new ArgumentOutOfRangeException(nameof(options.LocalEndPoint.Port));
                if (options.Backlog < 1) throw new ArgumentOutOfRangeException(nameof(options.Backlog));
                if (options.LocalEndPoint.AddressFamily != options.Family) throw new ArgumentException($"The {nameof(options.LocalEndPoint.AddressFamily)} and {nameof(options.Family)} not match.");

                switch (options.Family)
                {
                    case AddressFamily.InterNetwork:
                    case AddressFamily.InterNetworkV6:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(options.Family), $"The AddressFamily only support AddressFamily.InterNetwork and AddressFamily.InterNetworkV6.");
                }

                if (options.OnConnected != null) this.OnConnected = options.OnConnected;
                if (options.OnReceived != null) this.OnReceived = options.OnReceived;
                if (options.OnDisconnected != null) this.OnDisconnected = options.OnDisconnected;
                if (options.OnStarted != null) this.OnStarted = options.OnStarted;
                if (options.OnStopped != null) this.OnStopped = options.OnStopped;
                if (options.OnException != null) this.OnException = options.OnException;

                LocalEndPoint = options.LocalEndPoint;

                Instance = new Socket(options.Family, SocketType.Stream, ProtocolType.Tcp);

                Instance.Bind(options.LocalEndPoint);

                Instance.Listen(options.Backlog);

                this.Key = options.LocalEndPoint.ToString();
                this.Name = options.Name ?? this.Key;
                this.Id = options.Id;
            }
            catch (Exception ex)
            {
                OnServerException?.Invoke(new NetServerEventArgs(this, LocalEndPoint) { Exception = ex });
                throw ex;
            }
        }

        public override void Start()
        {
            if (Running) return;

            try
            {
                BeginAccept();

                running = true;

                OnStarted?.Invoke(new NetServerEventArgs(this, LocalEndPoint));
            }
            catch (Exception ex)
            {
                OnServerException?.Invoke(new NetServerEventArgs(this, LocalEndPoint) { Exception = ex });
            }
        }

        public override void Stop()
        {
            if (!Running) return;

            stopping = true;

            SocketAggregateException exception = null;

            try
            {
                List<Exception> exceptions = new List<Exception>();

                try
                {
                    Instance.Close();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }

                if (!(Connections?.Count > 0)) return;

                string[] keys = Connections.Keys.ToArray();

                foreach (var item in keys)
                {
                    try
                    {
                        Connections[item]?.Stop();
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }

                if (exceptions.Count > 0) exception = new SocketAggregateException($"The tcp server {Key} throw exceptions when stopping.", exceptions.ToArray());

                running = false;
            }
            finally
            {
                if (exception != null)
                {
                    OnServerException?.Invoke(new NetServerEventArgs(this, LocalEndPoint) { Exception = exception });
                    throw exception;
                }

                if (stopping) OnStopped?.Invoke(new NetServerEventArgs(this, LocalEndPoint));

                stopping = false;
            }
        }

        protected override void OnDisconnectedHandler(NetClientEventArgs<ITcpConnection> args)
        {
            RemoveListener(args.NetConnection.Key);
            OnDisconnected?.Invoke(args);
        }

        private void BeginAccept()
        {
            try
            {
                Instance.BeginAccept(AcceptCallback, null);
            }
            catch (Exception ex)
            {
                OnServerException?.Invoke(new NetServerEventArgs(this, LocalEndPoint) { Exception = ex });

                Stop();
            }
        }

        private void AcceptCallback(IAsyncResult result)
        {
            bool accept = false;
            ITcpConnection connection = null;

            try
            {
                if (!Running || stopping) return;

                Socket socket = Instance.EndAccept(result);

                if (stopping) return;

                BeginAccept();
                accept = true;

                connection = new TcpConnection(socket) { OnConnected = OnConnectedHandler, OnReceived = OnReceivedHandler, OnDisconnected = OnDisconnectedHandler, OnException = OnExceptionHandler };
                connection.Start();

                RemoveListener(connection.Key);

                lock (ConnectionsLock)
                {
                    Connections.Add(connection.Key, connection);
                }

                OnConnected?.Invoke(new NetClientEventArgs<ITcpConnection>(connection));
            }
            catch (Exception ex)
            {
                OnServerException?.Invoke(new NetServerEventArgs(this, LocalEndPoint, connection) { Exception = ex });
            }
            finally
            {
                if (Running && !accept && !stopping) BeginAccept();
            }
        }

        private void RemoveListener(string key)
        {
            bool existed = false;

            lock (ConnectionsLock)
            {
                existed = Connections?.ContainsKey(key) == true;
            }

            try
            {
                if (existed)
                {
                    Connections[key].Stop();
                    Connections[key] = null;
                }
            }
            catch (Exception ex)
            {
                OnServerException?.Invoke(new NetServerEventArgs(this, LocalEndPoint) { Exception = ex });
            }
            finally
            {
                if (existed)
                {
                    lock (ConnectionsLock)
                    {
                        Connections?.Remove(key);
                    }
                }
            }
        }

        public override void Dispose()
        {
            if (Running) Instance.Close();
        }
    }
}
