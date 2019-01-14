using GodSharp.Sockets.Abstractions.Events;

namespace GodSharp.Sockets.Abstractions
{
    public interface IUdpClientEventHandleMethods : INetEventMethods<IUdpConnection, NetClientEventArgs<IUdpConnection>>
    {

    }
}