using System;

namespace GodSharp.Sockets.Abstractions
{
    public interface IEvent<TConnection, TEventArgs>
        where TConnection : INetConnection
        where TEventArgs : NetEventArgs
    {
        /// <summary>
        /// Gets the on connected.
        /// </summary>
        /// <value>
        /// The on connected.
        /// </value>
        SocketEventHandler<NetClientEventArgs<TConnection>> OnConnected { get; }

        /// <summary>
        /// Gets the on received.
        /// </summary>
        /// <value>
        /// The on received.
        /// </value>
        SocketEventHandler<NetClientReceivedEventArgs<TConnection>> OnReceived { get; }

        /// <summary>
        /// Gets the on disconnected.
        /// </summary>
        /// <value>
        /// The on disconnected.
        /// </value>
        SocketEventHandler<NetClientEventArgs<TConnection>> OnDisconnected { get; }

        /// <summary>
        /// Gets the on started.
        /// </summary>
        /// <value>
        /// The on started.
        /// </value>
        SocketEventHandler<TEventArgs> OnStarted { get; }

        /// <summary>
        /// Gets the on stopped.
        /// </summary>
        /// <value>
        /// The on stopped.
        /// </value>
        SocketEventHandler<TEventArgs> OnStopped { get; }

        /// <summary>
        /// Gets the on exception.
        /// </summary>
        /// <value>
        /// The on exception.
        /// </value>
        SocketEventHandler<NetClientEventArgs<TConnection>> OnException { get; }
    }
}
