using GodSharp.Sockets.Abstractions;
using GodSharp.Sockets.Extensions;
using GodSharp.Sockets.Udp;
using System;
using System.Net;
using System.Net.Sockets;

namespace GodSharp.Sockets
{
    public sealed class UdpClient : NetBase<IUdpConnection,NetClientEventArgs<IUdpConnection>>, IUdpClient, IDisposable
    {
        public override bool Running => Connection?.Listener?.Running == true;

        public override string Key => Connection.Key;

        public override string Name => Connection.Name;

        public override int Id => Connection.Id;

        private UdpConnection connection { get; set; }
        public IUdpConnection Connection => connection;

        public UdpClient(UdpClientOptions options) => OnConstructing(options);

        public UdpClient(IPEndPoint remote, IPEndPoint local, AddressFamily family = AddressFamily.InterNetwork, string name = null, int id = 0) => OnConstructing(new UdpClientOptions(local, remote, family) { Id = id, Name = name });

        public UdpClient(int localPort, string localHost = null, AddressFamily family = AddressFamily.InterNetwork, string name = null, int id = 0) => OnConstructing(new UdpClientOptions(new IPEndPoint(localHost.IsNullOrWhiteSpace() ? (family == AddressFamily.InterNetworkV6 ? IPAddress.IPv6Any : IPAddress.Any) : IPAddress.Parse(localHost), localPort), null, family) { Id = id, Name = name });

        public UdpClient(string remoteHost, int remotePort, int localPort = 8899, string localHost = null, AddressFamily family = AddressFamily.InterNetwork, string name = null, int id = 0)
        {
            try
            {
                UdpClientOptions options = new UdpClientOptions(new IPEndPoint(localHost.IsNullOrWhiteSpace() ? (family == AddressFamily.InterNetworkV6 ? IPAddress.IPv6Any : IPAddress.Any) : IPAddress.Parse(localHost), localPort), remoteHost.IsNullOrWhiteSpace() ? null : new IPEndPoint(IPAddress.Parse(remoteHost), remotePort), family) { Id = id, Name = name };
                
                OnConstructing(options);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void OnConstructing(UdpClientOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (options.RemoteEndPoint == null && options.LocalEndPoint == null) throw new ArgumentNullException(nameof(options.RemoteEndPoint));

            connection = new UdpConnection(options.RemoteEndPoint, options.LocalEndPoint, options.Family) { OnReceived = OnReceivedHandler, OnDisconnected = OnDisconnectedHandler, OnStarted = OnStartedHandler, OnStopped = OnStoppedHandler, OnException = OnExceptionHandler};

            if (options.Id > 0) connection.Id = options.Id;
            if (!options.Name.IsNullOrWhiteSpace()) connection.Name = options.Name;

            if (options.OnReceived != null) this.OnReceived = options.OnReceived;
            if (options.OnDisconnected != null) this.OnDisconnected = options.OnDisconnected;
            if (options.OnStarted != null) this.OnStarted = options.OnStarted;
            if (options.OnStopped != null) this.OnStopped = options.OnStopped;
            if (options.OnException != null) this.OnException = options.OnException;
        }

        public override void Start() => Connection?.Start();

        public override void Stop() => Connection?.Stop();

        protected override void OnConnectedHandler(NetClientEventArgs<IUdpConnection> args) => throw new NotSupportedException();

        public override void Dispose() => this.Connection?.Dispose();
    }
}