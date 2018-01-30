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
        /// <summary>
        /// The socket
        /// </summary>
        internal Socket socket = null;

        /// <summary>
        /// The listener
        /// </summary>
        private UdpListener listener;

        /// <summary>
        /// Gets or sets the local host.
        /// </summary>
        /// <value>
        /// The local host.
        /// </value>
        public string LocalHost { get; set; }

        /// <summary>
        /// Gets or sets the local port.
        /// </summary>
        /// <value>
        /// The local port.
        /// </value>
        public int LocalPort { get; set; }

        /// <summary>
        /// Gets or sets the remote host.
        /// </summary>
        /// <value>
        /// The remote host.
        /// </value>
        public string RemoteHost { get; set; }

        /// <summary>
        /// Gets or sets the remote port.
        /// </summary>
        /// <value>
        /// The remote port.
        /// </value>
        public int RemotePort { get; set; }

        /// <summary>
        /// Gets the remote end point.
        /// </summary>
        /// <value>
        /// The remote end point.
        /// </value>
        public EndPoint RemoteEndPoint => socket.RemoteEndPoint;

        /// <summary>
        /// Gets the local end point.
        /// </summary>
        /// <value>
        /// The local end point.
        /// </value>
        public EndPoint LocalEndPoint => socket.LocalEndPoint;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="UdpClient"/> class.
        /// </summary>
        public UdpClient()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UdpClient"/> class.
        /// </summary>
        /// <param name="port">The port.</param>
        public UdpClient(int port)
        {
            SetPort(port);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UdpClient"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        public UdpClient(string host,int port)
        {
            SetHost(host);

            SetPort(port);
        }

        /// <summary>
        /// Start Socket.
        /// </summary>
        public void Start()
        {
            if (socket==null)
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                
                if (LocalPort > 0)
                {
                    socket.Bind(new IPEndPoint(LocalHost == null ? IPAddress.Any : IPAddress.Parse(LocalHost), LocalPort));
                }
            }

            listener = new UdpListener(socket, Encoding);
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
            listener.Stop();
        }


        /// <summary>
        /// Sets the host.
        /// </summary>
        /// <param name="host">The host.</param>
        private void SetHost(string host)
        {
            Exception ex = Utils.ValidateHost(host);
            if (ex == null)
            {
                LocalHost = host;
            }
            else
            {
                throw ex;
            }
        }

        /// <summary>
        /// Sets the port.
        /// </summary>
        /// <param name="port">The port.</param>
        private void SetPort(int port)
        {
            Exception ex = Utils.ValidatePort(port);
            if (ex == null)
            {
                LocalPort = port;
            }
            else
            {
                throw ex;
            }
        }

        /// <summary>
        /// Checks the host and port.
        /// </summary>
        /// <exception cref="System.Exception">
        /// Host is incorrect
        /// or
        /// Port is incorrect
        /// </exception>
        private void CheckHostAndPort()
        {
            Exception exception = Utils.ValidateHost(LocalHost);

            if (exception != null)
            {
                throw new Exception("Host is incorrect", exception);
            }

            exception = Utils.ValidatePort(LocalPort);

            if (exception != null)
            {
                throw new Exception("Port is incorrect", exception);
            }
        }
    }
}
