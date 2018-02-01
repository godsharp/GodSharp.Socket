using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace GodSharp.Sockets
{
    /// <summary>
    /// 
    /// </summary>
    internal class UdpListener
    {
        /// <summary>
        /// Gets the unique identifier.
        /// </summary>
        /// <value>
        /// The unique identifier.
        /// </value>
        public Guid Guid { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="TcpListener"/> is running.
        /// </summary>
        /// <value>
        ///   <c>true</c> if running; otherwise, <c>false</c>.
        /// </value>
        public bool Running { get; private set; }

        /// <summary>
        /// Gets the remote end point.
        /// </summary>
        /// <value>
        /// The remote end point.
        /// </value>
        public EndPoint RemoteEndPoint { get; internal set; }

        /// <summary>
        /// Gets the local end point.
        /// </summary>
        /// <value>
        /// The local end point.
        /// </value>
        public EndPoint LocalEndPoint { get; internal set; }

        /// <summary>
        /// Gets the sender.
        /// </summary>
        /// <value>
        /// The sender.
        /// </value>
        public UdpSender Sender { get; private set; }

        /// <summary>
        /// Gets or sets the on started.
        /// </summary>
        /// <value>
        /// The on started.
        /// </value>
        public Action<UdpSender> OnStarted { get; set; } = null;

        /// <summary>
        /// Gets or sets the on data.
        /// </summary>
        /// <value>
        /// The on data.
        /// </value>
        public Action<UdpSender, byte[]> OnData { get; set; }

        /// <summary>
        /// Gets or sets the on exception.
        /// </summary>
        /// <value>
        /// The on exception.
        /// </value>
        public Action<UdpSender, Exception> OnException { get; set; }

        /// <summary>
        /// Gets or sets the on stopped.
        /// </summary>
        /// <value>
        /// The on stopped.
        /// </value>
        public Action<UdpSender> OnStopped { get; set; } = null;

        /// <summary>
        /// Gets the socket.
        /// </summary>
        /// <value>
        /// The socket.
        /// </value>
        private Socket socket { get; set; }

        /// <summary>
        /// The thread handle
        /// </summary>
        private Thread threadHandle = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="UdpListener"/> class.
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <param name="remote">The remote.</param>
        /// <param name="encoding">The encoding.</param>
        internal UdpListener(Socket socket, EndPoint remote, Encoding encoding)
        {
            this.socket = socket;

            LocalEndPoint = socket.LocalEndPoint;
            RemoteEndPoint = remote;

            byte[] gb = Utils.Md5(LocalEndPoint.ToString());

            this.Guid = new Guid(gb);

            Sender = new UdpSender(socket, remote, this.Guid, encoding);
        }

        /// <summary>
        /// Start Socket client.
        /// </summary>
        public void Start()
        {
            try
            {
                if (socket == null)
                {
                    throw new NullReferenceException($"socket is null");
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
                Console.WriteLine($"Socket client {socket.LocalEndPoint} started");
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

            socket?.Close();

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
                EndPoint endPoint;
                while (Running)
                {
                    try
                    {
                        data = new byte[socket.ReceiveBufferSize];
                        endPoint = new IPEndPoint(socket.AddressFamily == AddressFamily.InterNetworkV6 ? IPAddress.IPv6Any : IPAddress.Any, 0);

                        length = socket.ReceiveFrom(data, ref endPoint);

                        if (length==0)
                        {
                            continue;
                        }

                        byte[] tmp = new byte[length];
                        Buffer.BlockCopy(data, 0, tmp, 0, length);
                        
                        Sender.RemoteEndPoint = endPoint;

                        OnData?.Invoke(Sender, tmp);
                    }
                    catch (SocketException ex)
                    {
#if DEBUG
                        Console.WriteLine(ex.Message);
#endif
                        OnException?.Invoke(Sender, ex);

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
                        OnException?.Invoke(Sender, ex);
                        continue;
                    }

                    Thread.Sleep(10);
                }

#if DEBUG
                Console.WriteLine($"Server {RemoteEndPoint} offline.");
#endif

                OnStopped?.Invoke(Sender);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}