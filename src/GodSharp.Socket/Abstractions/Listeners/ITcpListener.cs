using System;

namespace GodSharp.Sockets.Abstractions
{
    public interface ITcpListener:INetListener<ITcpConnection>, IDisposable
    {
    }
}
