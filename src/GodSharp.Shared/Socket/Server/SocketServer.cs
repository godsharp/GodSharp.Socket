using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using System.Threading;
// ReSharper disable ArrangeThisQualifier

namespace GodSharp.Sockets
{
    public class SocketServer : SocketBase, IDisposable
    {

        private int backlog;
        private Action<Socket> onOpen;

        /// <summary>
        /// The client list.
        /// </summary>
        internal List<Socket> SocketClients;
        public Dictionary<KeyValuePair<string,int>,Socket> clients;

        /// <summary>
        /// The execute method when client connected.
        /// </summary>

        /// <summary>
        /// Gets the remote endpoint.
        /// </summary>
        //public EndPoint RemoteEndPoint { get => remoteEndPoint; private set => remoteEndPoint = value; }

        public Action<Socket> OnOpen { get => onOpen; set => onOpen = value; }

        /// <summary>
        /// The execute method when received data.
        /// </summary>
        public Action<SocketServer, byte[]> OnData { get; set; }

        /// <summary>
        /// The maximum length of the pending connections queue.
        /// </summary>
        public int Backlog { get => backlog; set => backlog = value; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketServer" /> class.
        /// </summary>
        private SocketServer()
        {
            SocketClients = new List<Socket>();
            clients = new Dictionary<KeyValuePair<string, int>, Socket>();
            onOpen = null;
            OnData = null;
        }

        /// <summary>
        ///Initializes a new instance of the <see cref="SocketServer" /> class using specified port.
        /// </summary>
        /// <param name="port">The socket server port.</param>
        public SocketServer(int port = 7788) : this()
        {
            this.port = port;

            socket = new Socket(addressFamily, socketType, protocolType);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketServer" /> class.
        /// </summary>
        /// <param name="host">The socket server host ip address.</param>
        /// <param name="port">The socket server port.</param>
        /// <param name="backlog">The socket server max number to listening.</param>
        /// <param name="addressFamily">The address family of the <see cref="T:Socket" />.</param>
        /// <param name="socketType">The type of the <see cref="T:Socket" />.</param>
        /// <param name="protocolType">The protocol type of the <see cref="T:Socket" />.</param>
        public SocketServer(string host = null, int port = 7788, int backlog = 1024, AddressFamily addressFamily = AddressFamily.InterNetwork, SocketType socketType = SocketType.Stream, ProtocolType protocolType = ProtocolType.Tcp) : this()
        {
            if (!string.IsNullOrEmpty(host))
            {
                bool flag = IPAddress.TryParse(host, out IPAddress _);

                if (!flag)
                {
                    throw new InvalidCastException("host must be a ip address string.");
                }
            }
            else
            {
                host = null;
            }

            this.host = host ?? this.host;
            this.port = port;
            this.backlog = backlog;
            this.addressFamily = addressFamily;
            this.socketType = socketType;
            this.protocolType = protocolType;

            socket = new Socket(this.addressFamily, this.socketType, this.protocolType);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketServer" /> class.
        /// </summary>
        /// <param name="port">The socket server port.</param>
        /// <param name="backlog">The socket server max number to listening.</param>
        /// <param name="addressFamily">The address family of the <see cref="T:Socket" />.</param>
        /// <param name="socketType">The type of the <see cref="T:Socket" />.</param>
        /// <param name="protocolType">The protocol type of the <see cref="T:Socket" />.</param>
        public SocketServer(int port = 7788, int backlog = 1024, AddressFamily addressFamily = AddressFamily.InterNetwork, SocketType socketType = SocketType.Stream, ProtocolType protocolType = ProtocolType.Tcp) : this()
        {
            this.port = port;
            this.backlog = backlog;
            this.addressFamily = addressFamily;
            this.socketType = socketType;
            this.protocolType = protocolType;

            socket = new Socket(this.addressFamily, this.socketType, this.protocolType);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketServer" /> class.
        /// </summary>
        /// <param name="addressFamily">The address family of the <see cref="T:Socket" />.</param>
        /// <param name="socketType">The type of the <see cref="T:Socket" />.</param>
        /// <param name="protocolType">The protocol type of the <see cref="T:Socket" />.</param>
        public SocketServer(AddressFamily addressFamily = AddressFamily.InterNetwork, SocketType socketType = SocketType.Stream, ProtocolType protocolType = ProtocolType.Tcp)
        {
            this.addressFamily = addressFamily;
            this.socketType = socketType;
            this.protocolType = protocolType;
            socket = new Socket(this.addressFamily, this.socketType, this.protocolType);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="SocketServer" />, and optionally disposes of the managed resources.
        /// </summary>
        ~SocketServer()
        {
            Dispose(false);
        }

        /// <summary>Creates a new <see cref="T:System.Net.Sockets.Socket" /> for a newly created connection.</summary>
        /// <returns>A <see cref="T:System.Net.Sockets.Socket" /> for a newly created connection.</returns>
        public Socket Accept()
        {
            return socket.Accept();
        }

        /// <summary>Begins an asynchronous operation to accept an incoming connection attempt.</summary>
        /// <returns>Returns true if the I/O operation is pending. The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will be raised upon completion of the operation.Returns false if the I/O operation completed synchronously. The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will not be raised and the <paramref name="e" /> object passed as a parameter may be examined immediately after the method call returns to retrieve the result of the operation.</returns>
        /// <param name="e">The <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object to use for this asynchronous socket operation.</param>
        public bool AcceptAsync(SocketAsyncEventArgs e)
        {

            return socket.AcceptAsync(e);
        }

        /// <summary>Begins an asynchronous operation to accept an incoming connection attempt.</summary>
        /// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous <see cref="T:System.Net.Sockets.Socket" /> creation.</returns>
        /// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate. </param>
        /// <param name="state">An object that contains state information for this request. </param>
        public IAsyncResult BeginAccept(AsyncCallback callback, object state)
        {
            return socket.BeginAccept(callback, state);
        }

        /// <summary>Begins an asynchronous operation to accept an incoming connection attempt and receives the first block of data sent by the client application.</summary>
        /// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous <see cref="T:System.Net.Sockets.Socket" /> creation.</returns>
        /// <param name="receiveSize">The number of bytes to accept from the sender. </param>
        /// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate. </param>
        /// <param name="state">An object that contains state information for this request. </param>
        public IAsyncResult BeginAccept(int receiveSize, AsyncCallback callback, object state)
        {
            return socket.BeginAccept(receiveSize, callback, state);
        }

        /// <summary>Begins an asynchronous operation to accept an incoming connection attempt from a specified socket and receives the first block of data sent by the client application.</summary>
        /// <returns>An <see cref="T:System.IAsyncResult" /> object that references the asynchronous <see cref="T:System.Net.Sockets.Socket" /> object creation.</returns>
        /// <param name="acceptSocket">The accepted <see cref="T:System.Net.Sockets.Socket" /> object. This value may be null. </param>
        /// <param name="receiveSize">The maximum number of bytes to receive. </param>
        /// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate. </param>
        /// <param name="state">An object that contains state information for this request. </param>
        public IAsyncResult BeginAccept(Socket acceptSocket, int receiveSize, AsyncCallback callback, object state)
        {
            return socket.BeginAccept(acceptSocket, receiveSize, callback, state);
        }

        /// <summary>Asynchronously accepts an incoming connection attempt and creates a new <see cref="T:System.Net.Sockets.Socket" /> to handle remote host communication.</summary>
        /// <returns>A <see cref="T:System.Net.Sockets.Socket" /> to handle communication with the remote host.</returns>
        /// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> that stores state information for this asynchronous operation as well as any user defined data. </param>
        public Socket EndAccept(IAsyncResult asyncResult)
        {
            return socket.EndAccept(asyncResult);
        }

        /// <summary>Asynchronously accepts an incoming connection attempt and creates a new <see cref="T:System.Net.Sockets.Socket" /> object to handle remote host communication. This method returns a buffer that contains the initial data transferred.</summary>
        /// <returns>A <see cref="T:System.Net.Sockets.Socket" /> object to handle communication with the remote host.</returns>
        /// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the bytes transferred. </param>
        /// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> object that stores state information for this asynchronous operation as well as any user defined data. </param>
        public Socket EndAccept(out byte[] buffer, IAsyncResult asyncResult)
        {
            return socket.EndAccept(out buffer, asyncResult);
        }

        /// <summary>Asynchronously accepts an incoming connection attempt and creates a new <see cref="T:System.Net.Sockets.Socket" /> object to handle remote host communication. This method returns a buffer that contains the initial data and the number of bytes transferred.</summary>
        /// <returns>A <see cref="T:System.Net.Sockets.Socket" /> object to handle communication with the remote host.</returns>
        /// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the bytes transferred. </param>
        /// <param name="bytesTransferred">The number of bytes transferred. </param>
        /// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> object that stores state information for this asynchronous operation as well as any user defined data. </param>
        public Socket EndAccept(out byte[] buffer, out int bytesTransferred, IAsyncResult asyncResult)
        {
            return socket.EndAccept(out buffer, out bytesTransferred, asyncResult);
        }

        /// <summary>Places a <see cref="T:System.Net.Sockets.Socket" /> in a listening state.</summary>
        /// <param name="backlog">The maximum length of the pending connections queue. </param>
        // ReSharper disable once ParameterHidesMember
        public void Listen(int backlog)
        {
            socket.Listen(backlog);
        }

        /// <summary>Releases the unmanaged resources used by the <see cref="SocketServer" />, and optionally disposes of the managed resources.</summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>Releases the unmanaged resources used by the <see cref="SocketServer" />, and optionally disposes of the managed resources.</summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to releases only unmanaged resources. </param>
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Releases managed resources,like xxx.Dispose();
            }
            // Releases managed resources
            if (threadHandle != null && threadHandle.ThreadState == ThreadState.Running)
            {
                try
                {
                    threadHandle.Abort();
                    threadHandle = null;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }

            if (socket != null)
            {
                try
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Disconnect(false);
                    socket.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Init server.
        /// </summary>
        private void Init()
        {
            try
            {
                IPAddress ip = string.IsNullOrEmpty(host) ? IPAddress.Any : IPAddress.Parse(host);

                socket.Bind(new IPEndPoint(ip, port));
                socket.Listen(backlog);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Start Socket server.
        /// </summary>
        public void Start()
        {
            try
            {
                Init();
                
                if (threadHandle != null)
                {
                    return;
                }

                threadHandle = new Thread(Loop)
                {
                    IsBackground = true
                };
                threadHandle.Start();

                Console.WriteLine($"Socket server {socket.LocalEndPoint} started.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message,ex);
            }
        }
        
        /// <summary>
        /// Polling method.
        /// </summary>
        internal void Loop()
        {
            while (loopFlag)
            {
                try
                {
                    Socket sock = this.socket.Accept();
                    Thread processThread = new Thread(ProcessConnection)
                    {
                        IsBackground = true
                    };
                    processThread.Start(sock);
                    Thread.Sleep(10);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            this.status = SocketStatus.Stop;
        }

        /// <summary>
        /// The method for processing connection.
        /// </summary>
        /// <param name="s"></param>
        private void ProcessConnection(object s)
        {
            try
            {
                Socket sock = s as Socket;
                if (sock == null)
                {
                    return;
                }
                if (!sock.Connected)
                {
                    Console.WriteLine($"Client {sock.RemoteEndPoint} not connected to server.");
                    return;
                }

                Console.WriteLine($"Client {sock.RemoteEndPoint} connected to server.");

                IPEndPoint iPEnd = sock.RemoteEndPoint as IPEndPoint;
                KeyValuePair<string, int> k = new KeyValuePair<string, int>(iPEnd.Address.ToString(), iPEnd.Port);
                
                if (clients.ContainsKey(k))
                {
                    return;
                }
                clients.Add(k,sock);

                Thread thread = new Thread(ProcessDatas)
                {
                    IsBackground = true
                };
                thread.Start(sock);

                this.onOpen?.Invoke(sock);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Polling method for processing data.
        /// </summary>
        [SuppressMessage("ReSharper", "RedundantAssignment")]
        private void ProcessDatas(object s)
        {
            Socket sock = s as Socket;
            if (sock == null)
            {
                return;
            }
            //socket.ReceiveTimeout = 100;
            try
            {
                // ReSharper disable once TooWideLocalVariableScope
                byte[] data;
                // ReSharper disable once TooWideLocalVariableScope
                SocketResult<SocketServer> result;
                // ReSharper disable once TooWideLocalVariableScope
                int length;
                while (loopFlag && sock.Poll(-1, SelectMode.SelectRead))
                {
                    length = -1;
                    data = new byte[1024];

                    try
                    {
                        length = sock.Receive(data);

                        if (length < 1)
                        {
                            this.SocketClients.Remove(sock);
                            Console.WriteLine($"Client {sock.RemoteEndPoint} offline.");
                            break;
                        }

                        byte[] tmp = new byte[length];
                        Buffer.BlockCopy(data, 0, tmp, 0, length);

                        result = null;
                        this.RemoteEndPoint = sock.RemoteEndPoint;
                        result = new SocketResult<SocketServer>() { Socket = this, Bytes = tmp, OnData = OnData };

                        Thread processThread = new Thread(ProcessData<SocketServer>)
                        {
                            IsBackground = true
                        };
                        processThread.Start(result);
                    }
                    catch (SocketException ex)
                    {
                        Console.WriteLine(ex.Message);
                        if (ex.SocketErrorCode == SocketError.ConnectionReset || ex.SocketErrorCode == SocketError.ConnectionAborted)
                        {
                            IPEndPoint iPEnd = sock.RemoteEndPoint as IPEndPoint;
                            KeyValuePair<string, int> k = new KeyValuePair<string, int>(iPEnd.Address.ToString(), iPEnd.Port);
                            this.clients.Remove(k);
                            //this.SocketClients.Remove(sock);
                            Console.WriteLine($"Client {sock.RemoteEndPoint} offline.");
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        continue;
                    }

                    Thread.Sleep(10);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            this.status = SocketStatus.Stop;
        }

        /// <summary>
        /// Gets the <see cref="Socket"/> with the specified host.
        /// </summary>
        /// <value>
        /// The <see cref="Socket"/>.
        /// </value>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        /// <returns></returns>
        public Socket this[string host,int port]
        {
            get
            {
                KeyValuePair<string, int> k = new KeyValuePair<string, int>(host, port);

                if (clients?.ContainsKey(k) ==true)
                {
                    return clients[k];
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
