using GodSharp.Sockets.Abstractions;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace GodSharp.Sockets
{
    public interface ITcpServer : INetBase<ITcpConnection>, IEvent<ITcpConnection, NetServerEventArgs>, IDisposable
    {
        Socket Instance { get; }

        IDictionary<string, ITcpConnection> Connections { get; }

        SocketEventHandler<NetServerEventArgs> OnServerException { get; set; }
    }
}