using GodSharp.Sockets.Abstractions;
using System;

namespace GodSharp.Sockets
{
    public interface ITcpClient : INetBase<ITcpConnection>, IEvent<ITcpConnection, NetClientEventArgs<ITcpConnection>>, IDisposable
    {
        ITcpConnection Connection { get; }
    }
}