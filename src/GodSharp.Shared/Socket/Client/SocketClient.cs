﻿using GodSharp.Protocol;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace GodSharp
{
    public class SocketClient : SocketBase, IDisposable
    {
        /// <summary>Gets a value that indicates whether a <see cref="T:System.Net.Sockets.Socket" /> is connected to a remote host as of the last <see cref="M:System.Net.Sockets.Socket.Send" /> or <see cref="M:System.Net.Sockets.Socket.Receive" /> operation.</summary>
        /// <returns>true if the <see cref="T:System.Net.Sockets.Socket" /> was connected to a remote resource as of the most recent operation; otherwise, false.</returns>
        public bool Connected => _socket != null && _socket.Connected;

        /// <summary>Gets or sets a value that specifies the size of the receive buffer of the <see cref="T:System.Net.Sockets.Socket" />.</summary>
        /// <returns>An <see cref="T:System.Int32" /> that contains the size, in bytes, of the receive buffer. The default is 8192.</returns>
        public int ReceiveBufferSize
        {
            get { return _socket == null ? 8192 : _socket.ReceiveBufferSize; }
            set { if (_socket != null) _socket.ReceiveBufferSize = value; }
        }

        /// <summary>Gets or sets a value that specifies the size of the send buffer of the <see cref="T:System.Net.Sockets.Socket" />.</summary>
        /// <returns>An <see cref="T:System.Int32" /> that contains the size, in bytes, of the send buffer. The default is 8192.</returns>
        public int SendBufferSize
        {
            get { return _socket == null ? 8192 : _socket.SendBufferSize; }
            set { if (_socket != null) _socket.SendBufferSize = value; }
        }

        /// <summary>Gets or sets a value that specifies the amount of time after which a synchronous <see cref="M:System.Net.Sockets.Socket.Receive" /> call will time out.</summary>
        /// <returns>The time-out value, in milliseconds. The default value is 0, which indicates an infinite time-out period. Specifying -1 also indicates an infinite time-out period.</returns>
        public int ReceiveTimeout
        {
            get { return _socket == null ? 0 : _socket.ReceiveTimeout; }
            set { if (_socket != null) _socket.ReceiveTimeout = value; }
        }

        /// <summary>Gets or sets a value that specifies the amount of time after which a synchronous <see cref="M:System.Net.Sockets.Socket.Send" /> call will time out.</summary>
        /// <returns>The time-out value, in milliseconds. If you set the property with a value between 1 and 499, the value will be changed to 500. The default value is 0, which indicates an infinite time-out period. Specifying -1 also indicates an infinite time-out period.</returns>
        public int SendTimeout
        {
            get { return _socket == null ? 0 : _socket.SendTimeout; }
            set { if (_socket != null) _socket.SendTimeout = value; }
        }

        // This is generated by the compiler and can be safely removed
        /// <summary>
        /// Initializes a new instance of the <see cref="SocketClient" /> class.
        /// </summary>
        private SocketClient()
        {
            _host = "127.0.0.1";
        }

        /// <summary>
        ///Initializes a new instance of the <see cref="SocketClient" /> class using specified port.
        /// </summary>
        /// <param name="host">The remote host.</param>
        /// <param name="port">The int number of server port.</param>
        public SocketClient(string host = "127.0.0.1", uint port = 7788) : this()
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

            _socket = new Socket(_addressFamily, _socketType, _protocolType);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketClient" /> class.
        /// </summary>
        /// <param name="host">The remote host.</param>
        /// <param name="port">The int number of server port.</param>
        /// <param name="addressFamily">The address family of the <see cref="T:Socket" />.</param>
        /// <param name="socketType">The type of the <see cref="T:Socket" />.</param>
        /// <param name="protocolType">The protocol type of the <see cref="T:Socket" />.</param>
        public SocketClient(string host = "127.0.0.1", uint port = 7788, AddressFamily addressFamily = AddressFamily.InterNetwork, SocketType socketType = SocketType.Stream, ProtocolType protocolType = ProtocolType.Tcp) : this()
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
            _addressFamily = addressFamily;
            _socketType = socketType;
            _protocolType = protocolType;

            _socket = new Socket(_addressFamily, _socketType, _protocolType);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketClient" /> class.
        /// </summary>
        /// <param name="port">The int number of server port.</param>
        /// <param name="addressFamily">The address family of the <see cref="T:Socket" />.</param>
        /// <param name="socketType">The type of the <see cref="T:Socket" />.</param>
        /// <param name="protocolType">The protocol type of the <see cref="T:Socket" />.</param>
        public SocketClient(uint port = 7788, AddressFamily addressFamily = AddressFamily.InterNetwork, SocketType socketType = SocketType.Stream, ProtocolType protocolType = ProtocolType.Tcp) : this()
        {
            _port = port;
            _addressFamily = addressFamily;
            _socketType = socketType;
            _protocolType = protocolType;

            _socket = new Socket(_addressFamily, _socketType, _protocolType);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketClient" /> class.
        /// </summary>
        /// <param name="addressFamily">The address family of the <see cref="T:Socket" />.</param>
        /// <param name="socketType">The type of the <see cref="T:Socket" />.</param>
        /// <param name="protocolType">The protocol type of the <see cref="T:Socket" />.</param>
        public SocketClient(AddressFamily addressFamily = AddressFamily.InterNetwork, SocketType socketType = SocketType.Stream, ProtocolType protocolType = ProtocolType.Tcp)
        {
            _addressFamily = addressFamily;
            _socketType = socketType;
            _protocolType = protocolType;
            _socket = new Socket(_addressFamily, _socketType, _protocolType);
        }

        ~SocketClient()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                ///TODO:xxx.Dispose();
            }
        }


        /// <summary>Begins an asynchronous request for a remote host connection.</summary>
        /// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous connection.</returns>
        /// <param name="remoteEP">An <see cref="T:System.Net.EndPoint" /> that represents the remote host. </param>
        /// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate. </param>
        /// <param name="state">An object that contains state information for this request. </param>
        public IAsyncResult BeginConnect(EndPoint remoteEP, AsyncCallback callback, object state)
        {
            return _socket.BeginConnect(remoteEP, callback, state);
        }

        /// <summary>Begins an asynchronous request for a remote host connection. The host is specified by a host name and a port number.</summary>
        /// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous connection.</returns>
        /// <param name="host">The name of the remote host.</param>
        /// <param name="port">The port number of the remote host.</param>
        /// <param name="requestCallback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the connect operation is complete. </param>
        /// <param name="state">A user-defined object that contains information about the connect operation. This object is passed to the <paramref name="requestCallback" /> delegate when the operation is complete.</param>
        public IAsyncResult BeginConnect(string host, int port, AsyncCallback requestCallback, object state)
        {
            return _socket.BeginConnect(host, port, requestCallback, state);
        }

        /// <summary>Begins an asynchronous request for a remote host connection. The host is specified by an <see cref="T:System.Net.IPAddress" /> and a port number.</summary>
        /// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous connection.</returns>
        /// <param name="address">The <see cref="T:System.Net.IPAddress" /> of the remote host.</param>
        /// <param name="port">The port number of the remote host.</param>
        /// <param name="requestCallback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the connect operation is complete. </param>
        /// <param name="state">A user-defined object that contains information about the connect operation. This object is passed to the <paramref name="requestCallback" /> delegate when the operation is complete.</param>
        public IAsyncResult BeginConnect(IPAddress address, int port, AsyncCallback requestCallback, object state)
        {
            return _socket.BeginConnect(address, port, requestCallback, state);
        }

        /// <summary>Begins an asynchronous request for a remote host connection. The host is specified by an <see cref="T:System.Net.IPAddress" /> array and a port number.</summary>
        /// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous connections.</returns>
        /// <param name="addresses">At least one <see cref="T:System.Net.IPAddress" />, designating the remote host.</param>
        /// <param name="port">The port number of the remote host.</param>
        /// <param name="requestCallback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the connect operation is complete. </param>
        /// <param name="state">A user-defined object that contains information about the connect operation. This object is passed to the <paramref name="requestCallback" /> delegate when the operation is complete.</param>
        public IAsyncResult BeginConnect(IPAddress[] addresses, int port, AsyncCallback requestCallback, object state)
        {
            return _socket.BeginConnect(addresses, port, requestCallback, state);
        }

        /// <summary>Begins an asynchronous request to disconnect from a remote endpoint.</summary>
        /// <returns>An <see cref="T:System.IAsyncResult" /> object that references the asynchronous operation.</returns>
        /// <param name="reuseSocket">true if this socket can be reused after the connection is closed; otherwise, false. </param>
        /// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate. </param>
        /// <param name="state">An object that contains state information for this request. </param>
        public IAsyncResult BeginDisconnect(bool reuseSocket, AsyncCallback callback, object state)
        {
            return _socket.BeginDisconnect(reuseSocket, callback, state);
        }

        /// <summary>Establishes a connection to a remote host.</summary>
        /// <param name="remoteEP">An <see cref="T:System.Net.EndPoint" /> that represents the remote device. </param>
        public void Connect(EndPoint remoteEP)
        {
            _socket.Connect(remoteEP);
        }

        /// <summary>Establishes a connection to a remote host. The host is specified by an IP address and a port number.</summary>
        /// <param name="address">The IP address of the remote host.</param>
        /// <param name="port">The port number of the remote host.</param>
        public void Connect(IPAddress address, int port)
        {
            _socket.Connect(address, port);
        }

        /// <summary>Establishes a connection to a remote host. The host is specified by a host name and a port number.</summary>
        /// <param name="host">The name of the remote host.</param>
        /// <param name="port">The port number of the remote host.</param>
        public void Connect(string host, int port)
        {
            _socket.Connect(host, port);
        }

        /// <summary>Establishes a connection to a remote host. The host is specified by an array of IP addresses and a port number.</summary>
        /// <param name="addresses">The IP addresses of the remote host.</param>
        /// <param name="port">The port number of the remote host.</param>
        public void Connect(IPAddress[] addresses, int port)
        {
            _socket.Connect(addresses, port);
        }

        /// <summary>Begins an asynchronous request for a remote host connection.</summary>
        /// <returns>Returns true if the I/O operation is pending. The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will be raised upon completion of the operation. Returns false if the I/O operation completed synchronously. In this case, The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will not be raised and the <paramref name="e" /> object passed as a parameter may be examined immediately after the method call returns to retrieve the result of the operation. </returns>
        /// <param name="e">The <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object to use for this asynchronous socket operation.</param>
        public bool ConnectAsync(SocketAsyncEventArgs e)
        {
            return _socket.ConnectAsync(e);
        }

        /// <summary>Closes the socket connection and allows reuse of the socket.</summary>
        /// <param name="reuseSocket">true if this socket can be reused after the current connection is closed; otherwise, false. </param>
        public void Disconnect(bool reuseSocket)
        {
            _socket.Disconnect(reuseSocket);
        }

        /// <summary>Begins an asynchronous request to disconnect from a remote endpoint.</summary>
        /// <returns>Returns true if the I/O operation is pending. The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will be raised upon completion of the operation. Returns false if the I/O operation completed synchronously. In this case, The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will not be raised and the <paramref name="e" /> object passed as a parameter may be examined immediately after the method call returns to retrieve the result of the operation.</returns>
        /// <param name="e">The <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object to use for this asynchronous socket operation.</param>
        public bool DisconnectAsync(SocketAsyncEventArgs e)
        {
            return _socket.DisconnectAsync(e);
        }

        /// <summary>Ends a pending asynchronous connection request.</summary>
        /// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> that stores state information and any user defined data for this asynchronous operation. </param>
        public void EndConnect(IAsyncResult asyncResult)
        {
            _socket.EndConnect(asyncResult);
        }

        /// <summary>Ends a pending asynchronous disconnect request.</summary>
        /// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> object that stores state information and any user-defined data for this asynchronous operation. </param>
        public void EndDisconnect(IAsyncResult asyncResult)
        {
            _socket.EndDisconnect(asyncResult);
        }

        /// <summary>Determines the status of the <see cref="T:System.Net.Sockets.Socket" />.</summary>
        /// <returns>The status of the <see cref="T:System.Net.Sockets.Socket" /> based on the polling mode value passed in the <paramref name="mode" /> parameter.Mode Return Value <see cref="F:System.Net.Sockets.SelectMode.SelectRead" />true if <see cref="M:System.Net.Sockets.Socket.Listen(System.Int32)" /> has been called and a connection is pending; -or- true if data is available for reading; -or- true if the connection has been closed, reset, or terminated; otherwise, returns false. <see cref="F:System.Net.Sockets.SelectMode.SelectWrite" />true, if processing a <see cref="M:System.Net.Sockets.Socket.Connect(System.Net.EndPoint)" />, and the connection has succeeded; -or- true if data can be sent; otherwise, returns false. <see cref="F:System.Net.Sockets.SelectMode.SelectError" />true if processing a <see cref="M:System.Net.Sockets.Socket.Connect(System.Net.EndPoint)" /> that does not block, and the connection has failed; -or- true if <see cref="F:System.Net.Sockets.SocketOptionName.OutOfBandInline" /> is not set and out-of-band data is available; otherwise, returns false. </returns>
        /// <param name="microSeconds">The time to wait for a response, in microseconds. </param>
        /// <param name="mode">One of the <see cref="T:System.Net.Sockets.SelectMode" /> values. </param>
        /// <exception cref="T:System.NotSupportedException">The <paramref name="mode" /> parameter is not one of the <see cref="T:System.Net.Sockets.SelectMode" /> values. </exception>
        public bool Poll(int microSeconds, SelectMode mode)
        {
            return _socket.Poll(microSeconds, mode);
        }

        ///------------------------------------------

        /// <summary>
        /// Connect to server.
        /// </summary>
        public void Connect()
        {
            try
            {
                IPAddress ip = string.IsNullOrEmpty(_host) ? IPAddress.Any : IPAddress.Parse(_host);

                _socket.Connect(new IPEndPoint(ip, (int)_port));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Start Socket client.
        /// </summary>
        public void Start()
        {
            try
            {
                if (!_socket.Connected)
                {
                    Console.WriteLine("Socket server not connected.");
                    return;
                }

                if (_threadHandle != null)
                {
                    return;
                }

                _threadHandle = new Thread(new ThreadStart(Loop))
                {
                    IsBackground = true
                };
                _threadHandle.Start();
                Console.WriteLine($"Socket client {_socket.LocalEndPoint.ToString()} started");
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
            try
            {
                byte[] data;
                SocketResult result;
                int length;
                while (_loopFlag)
                {
                    try
                    {
                        data = new byte[1024];
                        length = -1;
                        length = _socket.Receive(data);

                        if (length < 1)
                        {
                            continue;
                        }

                        byte[] tmp = new byte[length];
                        Buffer.BlockCopy(data, 0, tmp, 0, length);

                        result = null;
                        result = new SocketResult() { Socket = _socket, Bytes = tmp };

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
                            Console.WriteLine($"Server {_socket.RemoteEndPoint.ToString()} offline.");
                            _socket.Shutdown(SocketShutdown.Both);
                            _socket.Close();
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
