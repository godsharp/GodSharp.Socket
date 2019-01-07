using System;
using System.Net;
using System.Net.Sockets;

namespace GodSharp.Sockets.Abstractions
{
    public abstract class NetConnection<TConnection, TEventArgs> : IEvent<TConnection, TEventArgs>, IDisposable
        where TConnection : INetConnection
        where TEventArgs : NetEventArgs
    {
        public virtual int Id { get; internal set; }

        public virtual string Name { get; internal set; }

        public virtual string Key { get; internal set; }

        public virtual Socket Instance { get; protected set; }

        public virtual IPEndPoint LocalEndPoint { get; protected set; }

        public virtual IPEndPoint RemoteEndPoint { get; protected set; }

        public virtual SocketEventHandler<NetClientEventArgs<TConnection>> OnConnected { get; internal set; }

        public virtual SocketEventHandler<NetClientReceivedEventArgs<TConnection>> OnReceived { get; internal set; }

        public virtual SocketEventHandler<NetClientEventArgs<TConnection>> OnDisconnected { get; internal set; }

        public virtual SocketEventHandler<TEventArgs> OnStarted { get; internal set; }

        public virtual SocketEventHandler<TEventArgs> OnStopped { get; internal set; }

        public virtual SocketEventHandler<NetClientEventArgs<TConnection>> OnException { get; internal set; }

        public abstract void Start();

        public abstract void Stop();

        public abstract void Dispose();
    }
}
