using GodSharp.Sockets.Abstractions;
using System;

namespace GodSharp.Sockets
{
    public interface ITcpClient : INetBase<ITcpConnection>, ITcpClientEvents, IDisposable
    {
        /// <summary>
        /// socket is connected
        /// </summary>
        bool Connected { get; }
        
        ITcpConnection Connection { get; }
    }
}