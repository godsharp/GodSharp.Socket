using GodSharp.Sockets.Abstractions.Events;

namespace GodSharp.Sockets.Abstractions
{
    public interface ITcpServerEventHandleMethods : INetEventMethods<ITcpConnection, NetServerEventArgs>
    {
        void OnServerExceptionHandler(NetServerEventArgs args);
    }
}
