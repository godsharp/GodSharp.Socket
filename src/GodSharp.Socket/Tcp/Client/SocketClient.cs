using System;
using System.Net;
using System.Net.Sockets;

namespace GodSharp.Sockets
{
    /// <summary>
    /// Socket client
    /// </summary>
    public partial class SocketClient:SocketBase
    {
        private TcpListener listener = null;

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        /// <value>
        /// The client identifier.
        /// </value>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets the sender.
        /// </summary>
        /// <value>
        /// The sender.
        /// </value>
        public TcpSender Sender => listener?.Sender;

        /// <summary>
        /// Gets the remote end point.
        /// </summary>
        /// <value>
        /// The remote end point.
        /// </value>
        public EndPoint RemoteEndPoint => socket.RemoteEndPoint;

        /// <summary>
        /// Local Port
        /// </summary>
        public int LocalPort { get; set; } = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketClient"/> class.
        /// </summary>
        public SocketClient()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketClient"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        public SocketClient(string host,int port)
        {
            SetHost(host);

            SetPort(port);

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketClient"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        /// <param name="localPort">The local port.</param>
        public SocketClient(string host, int port, int localPort)
        {
            SetHost(host);

            SetPort(port);

            Exception ex = Utils.ValidatePort(localPort);
            if (ex == null)
            {
                LocalPort = localPort;
            }
            else
            {
                throw ex;
            }

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        /// <summary>
        /// Connects this instance.
        /// </summary>
        public void Connect()
        {
            try
            {
                if (Connected)
                {
                    return;
                }

                CheckHostAndPort();

                socket.Connect(Host, Port);

                listener?.Stop();

                listener = new TcpListener(this, socket, TcpListenerType.Client);

                SetOnConnectedFun();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Connects the specified end point.
        /// </summary>
        /// <param name="endPoint">The end point.</param>
        /// <exception cref="ArgumentNullException">endPoint</exception>
        public void Connect(EndPoint endPoint)
        {
            try
            {
                if (Connected)
                {
                    return;
                }

                if (endPoint == null)
                {
                    throw new ArgumentNullException(nameof(endPoint));
                }

                this.Host = endPoint.GetHost();
                this.Port = endPoint.GetPort();

                socket.Connect(endPoint);

                listener?.Stop();

                listener = new TcpListener(this, socket, TcpListenerType.Client);

                SetOnConnectedFun();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Connects the specified host.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        public void Connect(string host, int port)
        {
            try
            {
                if (Connected)
                {
                    return;
                }

                SetHost(host);

                SetPort(port);

                // bind local port
                if (LocalPort > 0)
                {
                    EndPoint endPoint = new IPEndPoint(IPAddress.Any, LocalPort);

                    socket.Bind(endPoint);
                }

                socket.Connect(Host, Port);

                listener?.Stop();

                listener = new TcpListener(this, socket, TcpListenerType.Client);

                SetOnConnectedFun();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Connects the specified address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="port">The port.</param>
        /// <exception cref="ArgumentNullException">address</exception>
        public void Connect(IPAddress address, int port)
        {
            try
            {
                if (Connected)
                {
                    return;
                }

                if (address == null)
                {
                    throw new ArgumentNullException(nameof(address));
                }

                Host = address.ToString();

                SetPort(port);

                socket.Connect(address, Port);

                listener?.Stop();

                listener = new TcpListener(this, socket, TcpListenerType.Client);

                SetOnConnectedFun();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /// <summary>
        /// Start Socket client.
        /// </summary>
        public override void Start()
        {
            try
            {
                if (listener?.Running==true)
                {
                    return;
                }

                CheckHostAndPort();

                if (!Connected)
                {
                    throw new SocketException((int)SocketError.NotConnected);
                }

                listener.Start();
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
        public override void Stop()
        {
            if (listener?.Running == true)
            {
                listener.Stop();
            }
        }
        
        /// <summary>
        /// Sets the on connected fun.
        /// </summary>
        private void SetOnConnectedFun()
        {
            if (Connected)
            {
                OnConnected?.Invoke(listener.Sender);
            }
        }
    }
}