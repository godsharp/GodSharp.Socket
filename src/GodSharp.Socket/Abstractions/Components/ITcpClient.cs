using GodSharp.Sockets.Abstractions;
using System;

namespace GodSharp.Sockets
{
    public interface ITcpClient : INetBase<ITcpConnection>, ITcpClientEvents, IDisposable
    {
        ITcpConnection Connection { get; }
    }
}