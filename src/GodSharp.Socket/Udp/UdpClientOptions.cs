using GodSharp.Sockets.Abstractions;
using System.Net;
using System.Net.Sockets;

namespace GodSharp.Sockets
{
    public class UdpClientOptions: NetOptions<IUdpConnection>
    {
        public AddressFamily Family { get; set; } = AddressFamily.InterNetwork;

        public IPEndPoint RemoteEndPoint { get; set; }

        public SocketEventHandler<NetClientEventArgs<IUdpConnection>> OnStarted { get; set; }

        public SocketEventHandler<NetClientEventArgs<IUdpConnection>> OnStopped { get; set; }

        public UdpClientOptions()
        {
        }

        public UdpClientOptions(IPEndPoint localEndPoint, IPEndPoint remoteEndPoint, AddressFamily? family = null)
        {
            if (localEndPoint != null) LocalEndPoint = localEndPoint;
            if (remoteEndPoint != null) RemoteEndPoint = remoteEndPoint;
            if (family != null) Family = family.Value;
        }
    }
}