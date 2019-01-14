namespace GodSharp.Sockets.Abstractions
{
    public interface ITcpServerEvents : IEvent<ITcpConnection, NetServerEventArgs>
    {
        SocketEventHandler<NetServerEventArgs> OnServerException { get; set; }
    }
}