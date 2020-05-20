using GodSharp.Sockets.Abstractions;
using GodSharp.Sockets.Extensions;
using GodSharp.Sockets.Tcp;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace GodSharp.Sockets
{
    public sealed class TcpClient : NetBase<ITcpConnection, NetClientEventArgs<ITcpConnection>>, ITcpClient, IDisposable
    {
        private bool stopping = false;

        public override bool Running => Connection?.Listener?.Running == true;

        private TcpConnection connection { get; set; }
        public ITcpConnection Connection => connection;

        public override string Key => Connection.Key;

        public override string Name => Connection.Name;

        public override int Id => Connection.Id;
        
        public SocketEventHandler<TryConnectingEventArgs<ITcpConnection>> OnTryConnecting { get; set; }

        public TcpClient(TcpClientOptions options) => OnConstructing(options);

        public TcpClient(string remoteHost, int remotePort, int localPort = 0, string localHost = null, int connectTimeout = -1, string name = null, int id = 0)
        {
            try
            {
                TcpClientOptions options = new TcpClientOptions() { Id = id, Name = name };
                if (connectTimeout > 0) options.ConnectTimeout = connectTimeout;
                options.TryConnectionStrategy = new DefaultTryConnectionStrategy();
                options.RemoteEndPoint = new IPEndPoint(IPAddress.Parse(remoteHost), remotePort);
                options.LocalEndPoint = localHost.IsNullOrWhiteSpace() && localPort < 1 ? null : new IPEndPoint(options.RemoteEndPoint.AddressFamily == AddressFamily.InterNetworkV6 ? IPAddress.IPv6Any : IPAddress.Any, localPort);

                OnConstructing(options);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void OnConstructing(TcpClientOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (options.RemoteEndPoint == null) throw new ArgumentNullException(nameof(options.RemoteEndPoint));

            connection = new TcpConnection(options.RemoteEndPoint, options.LocalEndPoint) { OnConnected = OnConnectedHandler, OnReceived = OnReceivedHandler, OnDisconnected = OnDisconnectedHandler, OnStarted = OnStartedHandler, OnStopped = OnStoppedHandler, OnException = OnExceptionHandler, ConnectTimeout = options.ConnectTimeout, OnTryConnecting = OnTryConnectingHandler, ReconnectEnable = options.ReconnectEnable, TryConnectionStrategy = options.TryConnectionStrategy };

            if (options.Id > 0) connection.Id = options.Id;
            if (!options.Name.IsNullOrWhiteSpace()) connection.Name = options.Name;

            if (options.OnConnected != null) this.OnConnected = options.OnConnected;
            if (options.OnReceived != null) this.OnReceived = options.OnReceived;
            if (options.OnDisconnected != null) this.OnDisconnected = options.OnDisconnected;
            if (options.OnStarted != null) this.OnStarted = options.OnStarted;
            if (options.OnStopped != null) this.OnStopped = options.OnStopped;
            if (options.OnException != null) this.OnException = options.OnException;
            if (options.OnTryConnecting != null) this.OnTryConnecting = options.OnTryConnecting;
        }

        public void ReconnectAvailable(bool v) => connection.ReconnectEnable = v;

        public void UseTryConnectionStrategy(ITryConnectionStrategy strategy) => connection.TryConnectionStrategy = strategy;

        public override void Start()
        {
            stopping = false;
            Connection?.Start();
        }

        public override void Stop()
        {
            stopping = true;
            Connection?.Stop();
        }

        protected override void OnDisconnectedHandler(NetClientEventArgs<ITcpConnection> args)
        {
            base.OnDisconnectedHandler(args);
            ThreadPool.QueueUserWorkItem((o) => { connection.Reconnect(); });
            
        }
        
        protected void OnTryConnectingHandler(TryConnectingEventArgs<ITcpConnection> args) => OnTryConnecting?.Invoke(args);

        public override void Dispose() => this.Connection?.Dispose();
    }
}
