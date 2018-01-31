using GodSharp.Sockets.Internal.Util;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace GodSharp.Sockets
{
    /// <summary>
    /// Socket udp client
    /// </summary>
    public partial class UdpClient
    {
        private bool initialized;

        /// <summary>
        /// The socket
        /// </summary>
        internal Socket socket = null;

        /// <summary>
        /// The listener
        /// </summary>
        private UdpListener listener;

        private AddressFamily family { get; set; }

        #region Properties
        /// <summary>
        /// Gets or sets the local port.
        /// </summary>
        /// <value>
        /// The local port, default is 3030.
        /// </value>
        public int LocalPort { get; set; }

        /// <summary>
        /// Gets or sets the remote host.
        /// </summary>
        /// <value>
        /// The remote host.
        /// </value>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the remote port.
        /// </summary>
        /// <value>
        /// The remote port.
        /// </value>
        public int Port { get; set; }

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
        public EndPoint LocalEndPoint => socket.LocalEndPoint;

        /// <summary>
        /// Gets the available.
        /// </summary>
        /// <value>
        /// The available.
        /// </value>
        public int Available => socket.Available;

        /// <summary>
        /// Gets or sets the encoding.
        /// </summary>
        /// <value>
        /// The encoding.
        /// </value>
        public Encoding Encoding { get; set; } = Encoding.Default;

        /// <summary>
        /// Gets or sets the sender.
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
        public Action<UdpSender, byte[]> OnData { get; set; } = null;

        /// <summary>
        /// Gets or sets the on exception.
        /// </summary>
        /// <value>
        /// The on exception.
        /// </value>
        public Action<UdpSender, Exception> OnException { get; set; } = null;

        /// <summary>
        /// Gets or sets the on stopped.
        /// </summary>
        /// <value>
        /// The on stopped.
        /// </value>
        public Action<UdpSender> OnStopped { get; set; } = null; 
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="UdpClient"/> class.
        /// </summary>
        public UdpClient() : this(AddressFamily.InterNetwork)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UdpClient"/> class.
        /// </summary>
        public UdpClient(AddressFamily addressFamily)
        {
            LocalPort = 0;
            initialized = false;

            SetAddressFamily(addressFamily);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UdpClient"/> class.
        /// </summary>
        /// <param name="localPort">The local port.</param>
        public UdpClient(int localPort) : this(AddressFamily.InterNetwork, localPort)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UdpClient"/> class.
        /// </summary>
        /// <param name="addressFamily">The address family.</param>
        /// <param name="localPort">The local port.</param>
        public UdpClient(AddressFamily addressFamily, int localPort) : this()
        {
            SetAddressFamily(addressFamily);

            SetLocalPort(localPort);

            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UdpClient"/> class.
        /// </summary>
        /// <param name="host">The remote host.</param>
        /// <param name="port">The remote port.</param>
        public UdpClient(string host, int port) : this(AddressFamily.InterNetwork, host, port)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UdpClient"/> class.
        /// </summary>
        /// <param name="addressFamily">The address family.</param>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        public UdpClient(AddressFamily addressFamily, string host, int port) : this()
        {
            SetAddressFamily(addressFamily);

            SetHost(host);

            SetPort(port);

            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UdpClient"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        /// <param name="localPort"></param>
        public UdpClient(string host, int port, int localPort) : this(AddressFamily.InterNetwork, host, port, localPort)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UdpClient"/> class.
        /// </summary>
        /// <param name="addressFamily">The address family.</param>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        /// <param name="localPort">The local port.</param>
        public UdpClient(AddressFamily addressFamily, string host, int port, int localPort) : this()
        {
            SetAddressFamily(addressFamily);

            SetHost(host);

            SetPort(port);

            SetLocalPort(localPort);

            Initialize();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">LocalPort</exception>
        /// <exception cref="System.InvalidCastException">Host</exception>
        private void Initialize()
        {
            try
            {
                if (LocalPort < 1 && Port < 1)
                {
                    throw new InvalidOperationException($"{nameof(LocalPort)} and {nameof(Port)} value invalid.");
                }

                if (Port > 0 && (string.IsNullOrEmpty(Host) || Host.Trim() == ""))
                {
                    throw new InvalidCastException($"{nameof(Host)} value invalid.");
                }

                if (socket == null)
                {
                    socket = new Socket(family, SocketType.Dgram, ProtocolType.Udp);

                    if (LocalPort > 0)
                    {
                        socket.Bind(new IPEndPoint(socket.AddressFamily == AddressFamily.InterNetworkV6 ? IPAddress.IPv6Any : IPAddress.Any, LocalPort));
                    }

                    if (Port > 0)
                    {
                        RemoteEndPoint = new IPEndPoint(IPAddress.Parse(Host), Port);
                        socket.Connect(RemoteEndPoint);
                    }
                }

                initialized = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
        #endregion

        /// <summary>
        /// Start Socket.
        /// </summary>
        public void Start()
        {
            if (!initialized)
            {
                Initialize();
            }

            listener = new UdpListener(socket,RemoteEndPoint, Encoding);
            listener.OnStarted = OnStarted;
            listener.OnData = OnData;
            listener.OnException = OnException;
            listener.OnStopped = OnStopped;

            listener.Start();

            Sender = listener.Sender;
        }

        /// <summary>
        /// Stop Socket.
        /// </summary>
        public void Stop()
        {
            listener?.Stop();
        }
    }
}
