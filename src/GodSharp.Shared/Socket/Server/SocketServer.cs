using GodSharp.Protocol;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace GodSharp
{
    public class SocketServer : SocketBase, IDisposable
    {

        private uint _backlog;
        private Action<Socket> onOpen;

        /// <summary>
        /// The client list.
        /// </summary>
        internal List<Socket> _socketClients;

        /// <summary>
        /// The execute method when client connected.
        /// </summary>

        public Action<Socket> OnOpen { get => onOpen; set => onOpen = value; }

        /// <summary>
        /// The maximum length of the pending connections queue.
        /// </summary>
        public uint Backlog { get => _backlog; set => _backlog = value; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketServer" /> class.
        /// </summary>
        private SocketServer()
        {
            _socketClients = new List<Socket>();
            OnOpen = null;
        }

        /// <summary>
        ///Initializes a new instance of the <see cref="SocketServer" /> class using specified port.
        /// </summary>
        /// <param name="port">The socket server port.</param>
        public SocketServer(uint port = 7788) : this()
        {
            _port = port;

            _socket = new Socket(_addressFamily, _socketType, _protocolType);
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
        public SocketServer(string host = null, uint port = 7788, uint backlog = 1024, AddressFamily addressFamily = AddressFamily.InterNetwork, SocketType socketType = SocketType.Stream, ProtocolType protocolType = ProtocolType.Tcp) : this()
        {
            if (!string.IsNullOrEmpty(host))
            {
                bool flag = IPAddress.TryParse(host, out IPAddress ip);

                if (!flag)
                {
                    throw new InvalidCastException("host must be a ip address string.");
                }
            }
            else
            {
                host = null;
            }

            _host = host ?? _host;
            _port = port;
            _backlog = backlog;
            _addressFamily = addressFamily;
            _socketType = socketType;
            _protocolType = protocolType;

            _socket = new Socket(_addressFamily, _socketType, _protocolType);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketServer" /> class.
        /// </summary>
        /// <param name="port">The socket server port.</param>
        /// <param name="backlog">The socket server max number to listening.</param>
        /// <param name="addressFamily">The address family of the <see cref="T:Socket" />.</param>
        /// <param name="socketType">The type of the <see cref="T:Socket" />.</param>
        /// <param name="protocolType">The protocol type of the <see cref="T:Socket" />.</param>
        public SocketServer(uint port = 7788, uint backlog = 1024, AddressFamily addressFamily = AddressFamily.InterNetwork, SocketType socketType = SocketType.Stream, ProtocolType protocolType = ProtocolType.Tcp) : this()
        {
            _port = port;
            _backlog = backlog;
            _addressFamily = addressFamily;
            _socketType = socketType;
            _protocolType = protocolType;

            _socket = new Socket(_addressFamily, _socketType, _protocolType);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketServer" /> class.
        /// </summary>
        /// <param name="addressFamily">The address family of the <see cref="T:Socket" />.</param>
        /// <param name="socketType">The type of the <see cref="T:Socket" />.</param>
        /// <param name="protocolType">The protocol type of the <see cref="T:Socket" />.</param>
        public SocketServer(AddressFamily addressFamily = AddressFamily.InterNetwork, SocketType socketType = SocketType.Stream, ProtocolType protocolType = ProtocolType.Tcp)
        {
            _addressFamily = addressFamily;
            _socketType = socketType;
            _protocolType = protocolType;
            _socket = new Socket(_addressFamily, _socketType, _protocolType);
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
            return _socket.Accept();
        }

        /// <summary>Begins an asynchronous operation to accept an incoming connection attempt.</summary>
        /// <returns>Returns true if the I/O operation is pending. The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will be raised upon completion of the operation.Returns false if the I/O operation completed synchronously. The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will not be raised and the <paramref name="e" /> object passed as a parameter may be examined immediately after the method call returns to retrieve the result of the operation.</returns>
        /// <param name="e">The <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object to use for this asynchronous socket operation.</param>
        public bool AcceptAsync(SocketAsyncEventArgs e)
        {

            return _socket.AcceptAsync(e);
        }

        /// <summary>Begins an asynchronous operation to accept an incoming connection attempt.</summary>
        /// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous <see cref="T:System.Net.Sockets.Socket" /> creation.</returns>
        /// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate. </param>
        /// <param name="state">An object that contains state information for this request. </param>
        public IAsyncResult BeginAccept(AsyncCallback callback, object state)
        {
            return _socket.BeginAccept(callback, state);
        }

        /// <summary>Begins an asynchronous operation to accept an incoming connection attempt and receives the first block of data sent by the client application.</summary>
        /// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous <see cref="T:System.Net.Sockets.Socket" /> creation.</returns>
        /// <param name="receiveSize">The number of bytes to accept from the sender. </param>
        /// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate. </param>
        /// <param name="state">An object that contains state information for this request. </param>
        public IAsyncResult BeginAccept(int receiveSize, AsyncCallback callback, object state)
        {
            return _socket.BeginAccept(receiveSize, callback, state);
        }

        /// <summary>Begins an asynchronous operation to accept an incoming connection attempt from a specified socket and receives the first block of data sent by the client application.</summary>
        /// <returns>An <see cref="T:System.IAsyncResult" /> object that references the asynchronous <see cref="T:System.Net.Sockets.Socket" /> object creation.</returns>
        /// <param name="acceptSocket">The accepted <see cref="T:System.Net.Sockets.Socket" /> object. This value may be null. </param>
        /// <param name="receiveSize">The maximum number of bytes to receive. </param>
        /// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate. </param>
        /// <param name="state">An object that contains state information for this request. </param>
        public IAsyncResult BeginAccept(Socket acceptSocket, int receiveSize, AsyncCallback callback, object state)
        {
            return _socket.BeginAccept(acceptSocket, receiveSize, callback, state);
        }

        /// <summary>Asynchronously accepts an incoming connection attempt and creates a new <see cref="T:System.Net.Sockets.Socket" /> to handle remote host communication.</summary>
        /// <returns>A <see cref="T:System.Net.Sockets.Socket" /> to handle communication with the remote host.</returns>
        /// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> that stores state information for this asynchronous operation as well as any user defined data. </param>
        public Socket EndAccept(IAsyncResult asyncResult)
        {
            return _socket.EndAccept(asyncResult);
        }

        /// <summary>Asynchronously accepts an incoming connection attempt and creates a new <see cref="T:System.Net.Sockets.Socket" /> object to handle remote host communication. This method returns a buffer that contains the initial data transferred.</summary>
        /// <returns>A <see cref="T:System.Net.Sockets.Socket" /> object to handle communication with the remote host.</returns>
        /// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the bytes transferred. </param>
        /// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> object that stores state information for this asynchronous operation as well as any user defined data. </param>
        public Socket EndAccept(out byte[] buffer, IAsyncResult asyncResult)
        {
            return _socket.EndAccept(out buffer, asyncResult);
        }

        /// <summary>Asynchronously accepts an incoming connection attempt and creates a new <see cref="T:System.Net.Sockets.Socket" /> object to handle remote host communication. This method returns a buffer that contains the initial data and the number of bytes transferred.</summary>
        /// <returns>A <see cref="T:System.Net.Sockets.Socket" /> object to handle communication with the remote host.</returns>
        /// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the bytes transferred. </param>
        /// <param name="bytesTransferred">The number of bytes transferred. </param>
        /// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> object that stores state information for this asynchronous operation as well as any user defined data. </param>
        public Socket EndAccept(out byte[] buffer, out int bytesTransferred, IAsyncResult asyncResult)
        {
            return _socket.EndAccept(out buffer, out bytesTransferred, asyncResult);
        }

        /// <summary>Places a <see cref="T:System.Net.Sockets.Socket" /> in a listening state.</summary>
        /// <param name="backlog">The maximum length of the pending connections queue. </param>
        public void Listen(int backlog)
        {
            _socket.Listen(backlog);
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
            if (_threadHandle != null && _threadHandle.ThreadState == System.Threading.ThreadState.Running)
            {
                try
                {
                    _threadHandle.Abort();
                    _threadHandle = null;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }

            if (_socket != null)
            {
                try
                {
                    _socket.Shutdown(SocketShutdown.Both);
                    _socket.Disconnect(false);
                    _socket.Close();
                    _socket = null;
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
                IPAddress ip = string.IsNullOrEmpty(_host) ? IPAddress.Any : IPAddress.Parse(_host);

                _socket.Bind(new IPEndPoint(ip, (int)_port));
                _socket.Listen((int)_backlog);
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

                if (_threadHandle != null)
                {
                    return;
                }

                _threadHandle = new Thread(new ThreadStart(Loop))
                {
                    IsBackground = true
                };
                _threadHandle.Start();

                Console.WriteLine($"Socket server {_socket.LocalEndPoint.ToString()} started.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }


        /// <summary>
        /// Polling method.
        /// </summary>
        internal void Loop()
        {
            while (_loopFlag)
            {
                try
                {
                    Socket socket = _socket.Accept();
                    Thread processThread = new Thread(new ParameterizedThreadStart(ProcessConnection))
                    {
                        IsBackground = true
                    };
                    processThread.Start(socket);
                    Thread.Sleep(10);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            this._status = SocketStatus.Stop;
        }

        /// <summary>
        /// The method for processing connection.
        /// </summary>
        /// <param name="s"></param>
        private void ProcessConnection(object s)
        {
            try
            {
                Socket socket = s as Socket;
                if (socket == null)
                {
                    return;
                }
                if (!socket.Connected)
                {
                    Console.WriteLine($"Client {socket.RemoteEndPoint.ToString()} not connected to server.");
                    return;
                }

                Console.WriteLine($"Client {socket.RemoteEndPoint.ToString()} connected to server.");

                if (_socketClients.Contains(socket))
                {
                    return;
                }
                _socketClients.Add(socket);

                Thread thread = new Thread(new ParameterizedThreadStart(ProcessDatas))
                {
                    IsBackground = true
                };
                thread.Start(socket);

                this.onOpen?.Invoke(socket);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Polling method for processing data.
        /// </summary>
        private void ProcessDatas(object s)
        {
            Socket socket = s as Socket;
            if (socket == null)
            {
                return;
            }
            //socket.ReceiveTimeout = 100;
            try
            {
                byte[] data;
                SocketResult result;
                int length;
                while (_loopFlag && socket.Poll(-1, SelectMode.SelectRead))
                {
                    length = -1;
                    data = new byte[1024];

                    try
                    {
                        length = socket.Receive(data);

                        if (length < 1)
                        {
                            this._socketClients.Remove(socket);
                            Console.WriteLine($"Client {socket.RemoteEndPoint.ToString()} offline.");
                            break;
                        }

                        byte[] tmp = new byte[length];
                        Buffer.BlockCopy(data, 0, tmp, 0, length);

                        result = null;
                        result = new SocketResult() { Socket = socket, Bytes = tmp };

                        Thread processThread = new Thread(ProcessData)
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
                            this._socketClients.Remove(socket);
                            Console.WriteLine($"Client {socket.RemoteEndPoint.ToString()} offline.");
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

            this._status = SocketStatus.Stop;
        }
    }
}
