using GodSharp.Sockets.Internal;
using GodSharp.Sockets.Internal.Util;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace GodSharp.Sockets
{
    internal class Listener
    {
        /// <summary>
        /// Gets the unique identifier.
        /// </summary>
        /// <value>
        /// The unique identifier.
        /// </value>
        public Guid Guid { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Listener"/> is running.
        /// </summary>
        /// <value>
        ///   <c>true</c> if running; otherwise, <c>false</c>.
        /// </value>
        public bool Running { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Listener"/> is connected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if connected; otherwise, <c>false</c>.
        /// </value>
        public bool Connected => Socket.Connected;

        /// <summary>
        /// Gets the remote end point.
        /// </summary>
        /// <value>
        /// The remote end point.
        /// </value>
        public EndPoint RemoteEndPoint { get; private set; }

        /// <summary>
        /// Gets the local end point.
        /// </summary>
        /// <value>
        /// The local end point.
        /// </value>
        public EndPoint LocalEndPoint { get; private set; }

        /// <summary>
        /// Gets the sender.
        /// </summary>
        /// <value>
        /// The sender.
        /// </value>
        public Sender Sender { get; private set; }

        /// <summary>
        /// Gets the socket.
        /// </summary>
        /// <value>
        /// The socket.
        /// </value>
        internal Socket Socket { get; private set; }

        private SocketBase parent;
        private Thread threadHandle = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="Listener"/> class.
        /// </summary>
        /// <param name="_base">The base.</param>
        /// <param name="socket">The socket.</param>
        /// <param name="type">The type.</param>
        internal Listener(SocketBase _base,Socket socket, ListenerType type)
        {
            parent = _base;

            byte[] gb = Utils.Md5(type == ListenerType.Server ? socket.RemoteEndPoint.ToString() : socket.LocalEndPoint.ToString());

            this.Guid = new Guid(gb);
            this.Socket = socket;

            RemoteEndPoint = this.Socket.RemoteEndPoint;
            LocalEndPoint = this.Socket.LocalEndPoint;

            Sender = new Sender(this, parent.Encoding);
        }

        /// <summary>
        /// Start Socket client.
        /// </summary>
        public void Start()
        {
            try
            {
                if (!Socket.Connected)
                {
#if DEBUG
                    Console.WriteLine("Socket server not connected.");
#endif
                    return;
                }

                if (threadHandle != null)
                {
                    return;
                }

                Running = true;
                threadHandle = new Thread(Loop)
                {
                    IsBackground = true
                };
                threadHandle.Start();
#if DEBUG
                Console.WriteLine($"Socket client {Socket.LocalEndPoint} started");
#endif
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message);
#endif
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            if (threadHandle == null)
            {
                return;
            }

            Running = false;

            if (Connected)
            {
                Socket.Shutdown(SocketShutdown.Both);
                Socket.Disconnect(false);
                Socket.Close();
            }

            if (threadHandle.IsAlive)
            {
                try
                {
                    threadHandle.Abort();
                }
                catch (Exception)
                {
                }
                threadHandle = null;
            }
        }

        /// <summary>
        /// Polling method.
        /// </summary>
        private void Loop()
        {
            try
            {
                // ReSharper disable once TooWideLocalVariableScope
                byte[] data;
                // ReSharper disable once TooWideLocalVariableScope
                int length;
                while (Running && Connected)
                {
                    if (Socket.Poll(-1, SelectMode.SelectRead))
                    {
                        try
                        {
                            data = new byte[1024];
                            length = -1;
                            length = Socket.Receive(data);

                            if (length == 0)
                            {
                                break;
                            }

                            byte[] tmp = new byte[length];
                            Buffer.BlockCopy(data, 0, tmp, 0, length);

                            parent.OnData?.Invoke(Sender, tmp);
                        }
                        catch (SocketException ex)
                        {
#if DEBUG
                            Console.WriteLine(ex.Message);
#endif
                            parent.OnException?.Invoke(Sender, ex);

                            if (ex.SocketErrorCode == SocketError.ConnectionReset || ex.SocketErrorCode == SocketError.ConnectionAborted)
                            {
#if DEBUG
                                Console.WriteLine($"Server {RemoteEndPoint} offline by {(SocketError)ex.SocketErrorCode}.");
#endif
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
#if DEBUG
                            Console.WriteLine(ex.Message);
#endif
                            parent.OnException?.Invoke(Sender, ex);
                            continue;
                        }

                        Thread.Sleep(10);
                    }
                }

                Running = false;

                if (Socket?.Connected==true)
                {
                    Socket.Shutdown(SocketShutdown.Both);
                    Socket.Disconnect(true);
                    Socket.Close(); 
                }

#if DEBUG
                Console.WriteLine($"Server {RemoteEndPoint} offline.");
#endif

                parent.OnClosed?.Invoke(Sender);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}