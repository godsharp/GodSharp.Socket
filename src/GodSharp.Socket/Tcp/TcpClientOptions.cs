using GodSharp.Sockets.Abstractions;
using System.Net;

namespace GodSharp.Sockets
{
    public class TcpClientOptions: NetOptions<ITcpConnection>
    {
        public int ConnectTimeout { get; set; } = 30000;
        
        public IPEndPoint RemoteEndPoint { get; set; }

        public SocketEventHandler<NetClientEventArgs<ITcpConnection>> OnConnected { get; set; }

        public SocketEventHandler<NetClientEventArgs<ITcpConnection>> OnStarted { get; set; }

        public SocketEventHandler<NetClientEventArgs<ITcpConnection>> OnStopped { get; set; }

        public TcpClientOptions()
        {
        }

        public TcpClientOptions(IPEndPoint localEndPoint, IPEndPoint remoteEndPoint, SocketEventHandler<NetClientReceivedEventArgs<ITcpConnection>> handler = null)
        {
            if (localEndPoint != null) LocalEndPoint = localEndPoint;
            if (remoteEndPoint != null) RemoteEndPoint = remoteEndPoint;
            if (handler != null) OnReceived = handler;
        }
    }
}