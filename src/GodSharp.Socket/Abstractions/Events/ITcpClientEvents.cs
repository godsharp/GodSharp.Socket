namespace GodSharp.Sockets.Abstractions
{
    public interface ITcpClientEvents: IEvent<ITcpConnection, NetClientEventArgs<ITcpConnection>>
    {
    }
}