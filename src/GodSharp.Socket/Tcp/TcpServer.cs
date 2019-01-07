using GodSharp.Sockets.Abstractions;
using GodSharp.Sockets.Extensions;
using GodSharp.Sockets.Tcp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace GodSharp.Sockets
{
    public sealed class TcpServer :NetBase<ITcpConnection, NetServerEventArgs>, ITcpServer, IDisposable
    {
        private bool stopping = false;

        private bool running;
        public override bool Running => running;

        public Socket Instance { get; private set; }

        public IPEndPoint LocalEndPoint { get; private set; }

        public IDictionary<string, ITcpConnection> Connections { get; private set; } = new Dictionary<string, ITcpConnection>();

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
                throw ex;
            }
        }

        public override void Stop()
        {
            if (!Running) return;

            stopping = true;

            try
            {
                Instance.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                stopping = false;
            }

            running = false;
            stopping = false;
            
            OnStopped?.Invoke(new NetServerEventArgs(this, LocalEndPoint));
        }

        protected override void OnDisconnectedHandler(NetClientEventArgs<ITcpConnection> args)
        {
            RemoveListener(args.NetConnection.Key);
            OnDisconnected?.Invoke(args);
        }

        private void BeginAccept()
        {
            Instance.BeginAccept(AcceptCallback, null);
        }

        private void AcceptCallback(IAsyncResult result)
        {
            bool error = false;
            try
            {
                Socket socket = Instance.EndAccept(result);

                if (stopping) return;

                BeginAccept();
                
                ITcpConnection connection = new TcpConnection(socket) { OnConnected = OnConnectedHandler, OnReceived = OnReceivedHandler, OnDisconnected = OnDisconnectedHandler, OnException = OnExceptionHandler };
                connection.Start();

                RemoveListener(connection.Key);

                Connections.Add(connection.Key, connection);

                OnConnected?.Invoke(new NetClientEventArgs<ITcpConnection>(connection));
            }
            catch (SocketException ex)
            {
                error = true;
                throw ex;
            }
            catch (Exception ex)
            {
                error = true;
                throw ex;
            }
            finally
            {
                if (error) Stop();
            }
        }

        private void RemoveListener(string key)
        {
            bool existed = Connections?.ContainsKey(key) == true;

            try
            {
                if (existed) Connections[key].Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (existed) Connections?.Remove(key);
            }
        }

        public override void Dispose()
        {
            if (Running) Instance.Close();
        }
    }
}
