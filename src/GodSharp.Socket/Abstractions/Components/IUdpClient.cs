using GodSharp.Sockets.Abstractions;
using System;

namespace GodSharp.Sockets
{
    public interface IUdpClient : INetBase<IUdpConnection>, IUdpClientEvents, IDisposable
    {
        IUdpConnection Connection { get; }
    }
}
