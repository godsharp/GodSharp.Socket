using System;

namespace GodSharp.Sockets.Abstractions
{
    public interface IUdpListener : INetListener<IUdpConnection>, IDisposable
    {
    }
}
