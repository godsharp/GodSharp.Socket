using GodSharp.Sockets.Abstractions;
using GodSharp.Sockets.Tcp;
using System.Net;

namespace GodSharp.Sockets
{
    public class TcpClientOptions: NetOptions<ITcpConnection>
    {
        public int ConnectTimeout { get; set; } = -1;
        
        public IPEndPoint RemoteEndPoint { get; set; }
        public bool ReconnectEnable { get; set; } = true;

        public SocketEventHandler<NetClientEventArgs<ITcpConnection>> OnConnected { get; set; }

        public SocketEventHandler<NetClientEventArgs<ITcpConnection>> OnStarted { get; set; }

        public SocketEventHandler<NetClientEventArgs<ITcpConnection>> OnStopped { get; set; }

        public SocketEventHandler<TryConnectingEventArgs<ITcpConnection>> OnTryConnecting { get; set; }
        public ITryConnectionStrategy TryConnectionStrategy { get; set; } = new DefaultTryConnectionStrategy();

        public TcpClientOptions()
        {
        }

        public TcpClientOptions(IPEndPoint remoteEndPoint, IPEndPoint localEndPoint = null, SocketEventHandler<NetClientReceivedEventArgs<ITcpConnection>> handler = null)
        {
            if (localEndPoint != null) LocalEndPoint = localEndPoint;
            if (remoteEndPoint != null) RemoteEndPoint = remoteEndPoint;
            if (handler != null) OnReceived = handler;
        }
    }
}