using GodSharp.Sockets.Abstractions;
using System;

namespace GodSharp.Sockets
{
    public interface IUdpClient : INetBase<IUdpConnection>, IEvent<IUdpConnection, NetClientEventArgs<IUdpConnection>>, IDisposable
    {
        IUdpConnection Connection { get; }
    }
}
