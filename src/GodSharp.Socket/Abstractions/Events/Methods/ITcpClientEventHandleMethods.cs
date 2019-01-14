using GodSharp.Sockets.Abstractions.Events;

namespace GodSharp.Sockets.Abstractions
{
    public interface ITcpClientEventHandleMethods : INetEventMethods<ITcpConnection, NetClientEventArgs<ITcpConnection>>
    {
    }
}
