using System;
using System.Net;

namespace GodSharp.Sockets.Abstractions
{
    public abstract class NetOptions<TConnection> where TConnection : INetConnection
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual string Key { get; set; }

        public IPEndPoint LocalEndPoint { get; set; }

        public SocketEventHandler<NetClientReceivedEventArgs<TConnection>> OnReceived { get; set; }

        public SocketEventHandler<NetClientEventArgs<TConnection>> OnDisconnected { get; set; }

        public SocketEventHandler<NetClientEventArgs<TConnection>> OnException { get; set; }
    }
}
