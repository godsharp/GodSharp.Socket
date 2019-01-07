using System;

namespace GodSharp.Sockets.Abstractions
{
    public abstract class NetBase<TConnection,TEventArgs> : INetBase<TConnection>, IEvent<TConnection, TEventArgs>, IDisposable 
        where TConnection : INetConnection
        where TEventArgs : NetEventArgs
    {
        public virtual SocketEventHandler<NetClientEventArgs<TConnection>> OnConnected { get; set; }
        public SocketEventHandler<NetClientReceivedEventArgs<TConnection>> OnReceived { get; set; }
        public SocketEventHandler<NetClientEventArgs<TConnection>> OnDisconnected { get; set; }

        public SocketEventHandler<TEventArgs> OnStarted { get; set; }

        public SocketEventHandler<TEventArgs> OnStopped { get; set; }

        public SocketEventHandler<NetClientEventArgs<TConnection>> OnException { get; set; }
        
        public virtual int Id { get; internal set; }

        public virtual string Name { get; internal set; }

        public virtual string Key { get; internal set; }

        public virtual bool Running { get; protected set; }

        public abstract void Start();

        public abstract void Stop();

        protected virtual void OnConnectedHandler(NetClientEventArgs<TConnection> args) => OnConnected?.Invoke(args);

        protected virtual void OnReceivedHandler(NetClientReceivedEventArgs<TConnection> args) => OnReceived?.Invoke(args);

        protected virtual void OnDisconnectedHandler(NetClientEventArgs<TConnection> args) => OnDisconnected?.Invoke(args);

        protected virtual void OnStartedHandler(TEventArgs args) => OnStarted?.Invoke(args);

        protected virtual void OnStoppedHandler(TEventArgs args) => OnStopped?.Invoke(args);

        protected virtual void OnExceptionHandler(NetClientEventArgs<TConnection> args) => OnException?.Invoke(args);

        public abstract void Dispose();
    }
}
