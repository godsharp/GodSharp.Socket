namespace GodSharp.Sockets.Abstractions
{
    public interface IUdpClientEvents : IEvent<IUdpConnection, NetClientEventArgs<IUdpConnection>>
    {
    }
}