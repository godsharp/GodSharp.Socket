using GodSharp.Sockets.Abstractions;
using System.Net;
using System.Net.Sockets;

namespace GodSharp.Sockets
{
    public class TcpServerOptions : NetOptions<ITcpConnection>
    {
        public AddressFamily Family { get; set; } = AddressFamily.InterNetwork;
        
        public int Backlog { get; set; } = int.MaxValue;

        public SocketEventHandler<NetClientEventArgs<ITcpConnection>> OnConnected { get; set; }

        public SocketEventHandler<NetServerEventArgs> OnStarted { get; set; }

        public SocketEventHandler<NetServerEventArgs> OnStopped { get; set; }

        public SocketEventHandler<NetServerEventArgs> OnServerException { get; set; }

        public TcpServerOptions()
        {
        }

        public TcpServerOptions(IPEndPoint localEndPoint, int backlog = int.MaxValue, AddressFamily? family = null, SocketEventHandler<NetClientReceivedEventArgs<ITcpConnection>> handler = null)
        {
            LocalEndPoint = localEndPoint;
            if (backlog > 0) Backlog = backlog;
            if (family != null) Family = family.Value;
            if (handler != null) OnReceived = handler;
        }
    }
}