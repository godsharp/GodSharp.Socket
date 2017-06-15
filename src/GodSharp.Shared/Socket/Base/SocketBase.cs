using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using GodSharp.Protocol;
// ReSharper disable ArrangeThisQualifier
// ReSharper disable InconsistentNaming

namespace GodSharp
{
    public abstract class SocketBase
    {
        protected Thread threadHandle;
        protected SocketStatus status;
        protected bool loopFlag;
        protected Socket socket;
        protected string host;
        protected int port;
        protected AddressFamily addressFamily;
        protected SocketType socketType;
        protected ProtocolType protocolType;
        protected Encoding encoding;
        protected EndPoint remoteEndPoint;


        /// <summary>Gets the address family of the <see cref="T:System.Net.Sockets.Socket" />.</summary>
        /// <returns>One of the <see cref="T:System.Net.Sockets.AddressFamily" /> values.</returns>
        // ReSharper disable once UnusedMember.Local
        public AddressFamily AddressFamily { get => addressFamily; private set => addressFamily = value; }

        /// <summary>Gets the type of the <see cref="T:System.Net.Sockets.Socket" />.</summary>
        /// <returns>One of the <see cref="T:System.Net.Sockets.SocketType" /> values.</returns>
        // ReSharper disable once UnusedMember.Local
        public SocketType SocketType { get => socketType; private set => socketType = value; }

        /// <summary>Gets the protocol type of the <see cref="T:System.Net.Sockets.Socket" />.</summary>
        /// <returns>One of the <see cref="T:System.Net.Sockets.ProtocolType" /> values.</returns>
        // ReSharper disable once UnusedMember.Local
        public ProtocolType ProtocolType { get => protocolType; private set => protocolType = value; }

        /// <summary>
        /// The number of port that an instance of the <see cref="System.Net.Sockets.Socket"/> class bind.
        /// </summary>
        public int Port { get => port; set => port = value; }

        /// <summary>
        /// The encoding for socket.
        /// </summary>
        public Encoding Encoding { get => encoding; set => encoding = value; }

        /// <summary>
        /// Gets the remote endpoint.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        public EndPoint RemoteEndPoint { get => remoteEndPoint; protected set => remoteEndPoint = value; }

        /// <summary>
        /// Gets the local endpoint.
        /// </summary>
        public EndPoint LocalEndPoint => socket.LocalEndPoint;

        /// <summary>
        /// ctor
        /// </summary>
        internal SocketBase()
        {
            host = null;
            port = 7788;
            addressFamily = AddressFamily.InterNetwork;
            socketType = SocketType.Stream;
            protocolType = ProtocolType.Tcp;
            loopFlag = true;
            status = SocketStatus.Null;
            //onData = null;
            encoding = Encoding.UTF8;
        }


        /// <summary>Begins to asynchronously receive data from a connected <see cref="T:System.Net.Sockets.Socket" />.</summary>
        /// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous read.</returns>
        /// <param name="buffers">An array of type <see cref="T:System.Byte" /> that is the storage location for the received data.</param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
        /// <param name="callback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the operation is complete.</param>
        /// <param name="state">A user-defined object that contains information about the receive operation. This object is passed to the <see cref="M:System.Net.Sockets.Socket.EndReceive(System.IAsyncResult)" /> delegate when the operation is complete.</param>
        public IAsyncResult BeginReceive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, AsyncCallback callback, object state)
        {
            return socket.BeginReceive(buffers, socketFlags, callback, state);
        }

        /// <summary>Begins to asynchronously receive data from a connected <see cref="T:System.Net.Sockets.Socket" />.</summary>
        /// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous read.</returns>
        /// <param name="buffers">An array of type <see cref="T:System.Byte" /> that is the storage location for the received data.</param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values. </param>
        /// <param name="errorCode">A <see cref="T:System.Net.Sockets.SocketError" /> object that stores the socket error.</param>
        /// <param name="callback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the operation is complete.</param>
        /// <param name="state">A user-defined object that contains information about the receive operation. This object is passed to the <see cref="M:System.Net.Sockets.Socket.EndReceive(System.IAsyncResult)" /> delegate when the operation is complete.</param>
        public IAsyncResult BeginReceive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback, object state)
        {
            return socket.BeginReceive(buffers, socketFlags, out errorCode, callback, state);
        }

        /// <summary>Begins to asynchronously receive data from a connected <see cref="T:System.Net.Sockets.Socket" />.</summary>
        /// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous read.</returns>
        /// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for the received data. </param>
        /// <param name="offset">The zero-based position in the <paramref name="buffer" /> parameter at which to store the received data. </param>
        /// <param name="size">The number of bytes to receive. </param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values. </param>
        /// <param name="callback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the operation is complete. </param>
        /// <param name="state">A user-defined object that contains information about the receive operation. This object is passed to the <see cref="M:System.Net.Sockets.Socket.EndReceive(System.IAsyncResult)" /> delegate when the operation is complete.</param>
        public IAsyncResult BeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback, object state)
        {
            return socket.BeginReceive(buffer, offset, size, socketFlags, callback, state);
        }

        /// <summary>Begins to asynchronously receive data from a connected <see cref="T:System.Net.Sockets.Socket" />.</summary>
        /// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous read.</returns>
        /// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for the received data.</param>
        /// <param name="offset">The location in <paramref name="buffer" /> to store the received data. </param>
        /// <param name="size">The number of bytes to receive. </param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
        /// <param name="errorCode">A <see cref="T:System.Net.Sockets.SocketError" /> object that stores the socket error.</param>
        /// <param name="callback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the operation is complete.</param>
        /// <param name="state">A user-defined object that contains information about the receive operation. This object is passed to the <see cref="M:System.Net.Sockets.Socket.EndReceive(System.IAsyncResult)" /> delegate when the operation is complete.</param>
        public IAsyncResult BeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback, object state)
        {
            return socket.BeginReceive(buffer, offset, size, socketFlags, out errorCode, callback, state);
        }

        /// <summary>Begins to asynchronously receive data from a specified network device.</summary>
        /// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous read.</returns>
        /// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for the received data. </param>
        /// <param name="offset">The zero-based position in the <paramref name="buffer" /> parameter at which to store the data. </param>
        /// <param name="size">The number of bytes to receive. </param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values. </param>
        /// <param name="remoteEp">An <see cref="T:System.Net.EndPoint" /> that represents the source of the data. </param>
        /// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate. </param>
        /// <param name="state">An object that contains state information for this request. </param>
        public IAsyncResult BeginReceiveFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEp, AsyncCallback callback, object state)
        {
            return socket.BeginReceiveFrom(buffer, offset, size, socketFlags, ref remoteEp, callback, state);
        }

        /// <summary>Begins to asynchronously receive the specified number of bytes of data into the specified location of the data buffer, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />, and stores the endpoint and packet information..</summary>
        /// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous read.</returns>
        /// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for the received data. </param>
        /// <param name="offset">The zero-based position in the <paramref name="buffer" /> parameter at which to store the data.</param>
        /// <param name="size">The number of bytes to receive. </param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
        /// <param name="remoteEp">An <see cref="T:System.Net.EndPoint" /> that represents the source of the data.</param>
        /// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate.</param>
        /// <param name="state">An object that contains state information for this request.</param>
        public IAsyncResult BeginReceiveMessageFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEp, AsyncCallback callback, object state)
        {
            return socket.BeginReceiveMessageFrom(buffer, offset, size, socketFlags, ref remoteEp, callback, state);
        }

        /// <summary>Sends data asynchronously to a connected <see cref="T:System.Net.Sockets.Socket" />.</summary>
        /// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous send.</returns>
        /// <param name="buffers">An array of type <see cref="T:System.Byte" /> that contains the data to send. </param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values. </param>
        /// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate. </param>
        /// <param name="state">An object that contains state information for this request. </param>
        public IAsyncResult BeginSend(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, AsyncCallback callback, object state)
        {
            return socket.BeginSend(buffers, socketFlags, callback, state);
        }

        /// <summary>Sends data asynchronously to a connected <see cref="T:System.Net.Sockets.Socket" />.</summary>
        /// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous send.</returns>
        /// <param name="buffers">An array of type <see cref="T:System.Byte" /> that contains the data to send. </param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
        /// <param name="errorCode">A <see cref="T:System.Net.Sockets.SocketError" /> object that stores the socket error.</param>
        /// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate. </param>
        /// <param name="state">An object that contains state information for this request. </param>
        public IAsyncResult BeginSend(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback, object state)
        {
            return socket.BeginSend(buffers, socketFlags, out errorCode, callback, state);
        }

        /// <summary>Sends data asynchronously to a connected <see cref="T:System.Net.Sockets.Socket" />.</summary>
        /// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous send.</returns>
        /// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the data to send. </param>
        /// <param name="offset">The zero-based position in the <paramref name="buffer" /> parameter at which to begin sending data. </param>
        /// <param name="size">The number of bytes to send. </param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values. </param>
        /// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate. </param>
        /// <param name="state">An object that contains state information for this request. </param>
        public IAsyncResult BeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback, object state)
        {
            return socket.BeginSend(buffer, offset, size, socketFlags, callback, state);
        }

        /// <summary>Sends data asynchronously to a connected <see cref="T:System.Net.Sockets.Socket" />.</summary>
        /// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous send.</returns>
        /// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the data to send. </param>
        /// <param name="offset">The zero-based position in the <paramref name="buffer" /> parameter at which to begin sending data. </param>
        /// <param name="size">The number of bytes to send. </param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values. </param>
        /// <param name="errorCode">A <see cref="T:System.Net.Sockets.SocketError" /> object that stores the socket error.</param>
        /// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate. </param>
        /// <param name="state">An object that contains state information for this request. </param>
        public IAsyncResult BeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback, object state)
        {
            return socket.BeginSend(buffer, offset, size, socketFlags, out errorCode, callback, state);
        }

        /// <summary>Sends the file <paramref name="fileName" /> to a connected <see cref="T:System.Net.Sockets.Socket" /> object using the <see cref="F:System.Net.Sockets.TransmitFileOptions.UseDefaultWorkerThread" /> flag.</summary>
        /// <returns>An <see cref="T:System.IAsyncResult" /> object that represents the asynchronous send.</returns>
        /// <param name="fileName">A string that contains the path and name of the file to send. This parameter can be null. </param>
        /// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate. </param>
        /// <param name="state">An object that contains state information for this request. </param>
        public IAsyncResult BeginSendFile(string fileName, AsyncCallback callback, object state)
        {
            return socket.BeginSendFile(fileName, callback, state);
        }

        /// <summary>Sends a file and buffers of data asynchronously to a connected <see cref="T:System.Net.Sockets.Socket" /> object.</summary>
        /// <returns>An <see cref="T:System.IAsyncResult" /> object that represents the asynchronous operation.</returns>
        /// <param name="fileName">A string that contains the path and name of the file to be sent. This parameter can be null. </param>
        /// <param name="preBuffer">A <see cref="T:System.Byte" /> array that contains data to be sent before the file is sent. This parameter can be null. </param>
        /// <param name="postBuffer">A <see cref="T:System.Byte" /> array that contains data to be sent after the file is sent. This parameter can be null. </param>
        /// <param name="flags">A bitwise combination of <see cref="T:System.Net.Sockets.TransmitFileOptions" /> values. </param>
        /// <param name="callback">An <see cref="T:System.AsyncCallback" /> delegate to be invoked when this operation completes. This parameter can be null. </param>
        /// <param name="state">A user-defined object that contains state information for this request. This parameter can be null. </param>
        public IAsyncResult BeginSendFile(string fileName, byte[] preBuffer, byte[] postBuffer, TransmitFileOptions flags, AsyncCallback callback, object state)
        {
            return socket.BeginSendFile(fileName, preBuffer, postBuffer, flags, callback, state);
        }

        /// <summary>Sends data asynchronously to a specific remote host.</summary>
        /// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous send.</returns>
        /// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the data to send. </param>
        /// <param name="offset">The zero-based position in <paramref name="buffer" /> at which to begin sending data. </param>
        /// <param name="size">The number of bytes to send. </param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values. </param>
        /// <param name="remoteEp">An <see cref="T:System.Net.EndPoint" /> that represents the remote device. </param>
        /// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate. </param>
        /// <param name="state">An object that contains state information for this request. </param>
        public IAsyncResult BeginSendTo(byte[] buffer, int offset, int size, SocketFlags socketFlags, EndPoint remoteEp, AsyncCallback callback, object state)
        {
            return socket.BeginSendTo(buffer, offset, size, socketFlags, remoteEp, callback, state);
        }

        /// <summary>Closes the <see cref="T:System.Net.Sockets.Socket" /> connection and releases all associated resources.</summary>
        public void Close()
        {
            socket.Close();
        }

        /// <summary>Closes the <see cref="T:System.Net.Sockets.Socket" /> connection and releases all associated resources with a specified timeout to allow queued data to be sent. </summary>
        /// <param name="timeout">Wait up to <paramref name="timeout" /> seconds to send any remaining data, then close the socket.</param>
        public void Close(int timeout)
        {
            socket.Close(timeout);
        }

        /// <summary>Ends a pending asynchronous read.</summary>
        /// <returns>The number of bytes received.</returns>
        /// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> that stores state information and any user defined data for this asynchronous operation. </param>
        public int EndReceive(IAsyncResult asyncResult)
        {
            return socket.EndReceive(asyncResult);
        }

        /// <summary>Ends a pending asynchronous read.</summary>
        /// <returns>The number of bytes received.</returns>
        /// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> that stores state information and any user defined data for this asynchronous operation.</param>
        /// <param name="errorCode">A <see cref="T:System.Net.Sockets.SocketError" /> object that stores the socket error.</param>
        public int EndReceive(IAsyncResult asyncResult, out SocketError errorCode)
        {
            return socket.EndReceive(asyncResult, out errorCode);
        }

        /// <summary>Ends a pending asynchronous read from a specific endpoint.</summary>
        /// <returns>If successful, the number of bytes received. If unsuccessful, returns 0.</returns>
        /// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> that stores state information and any user defined data for this asynchronous operation. </param>
        /// <param name="endPoint">The source <see cref="T:System.Net.EndPoint" />. </param>
        public int EndReceiveFrom(IAsyncResult asyncResult, ref EndPoint endPoint)
        {
            return socket.EndReceiveFrom(asyncResult, ref endPoint);
        }

        /// <summary>Ends a pending asynchronous read from a specific endpoint. This method also reveals more information about the packet than <see cref="M:System.Net.Sockets.Socket.EndReceiveFrom(System.IAsyncResult,System.Net.EndPoint@)" />.</summary>
        /// <returns>If successful, the number of bytes received. If unsuccessful, returns 0.</returns>
        /// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> that stores state information and any user defined data for this asynchronous operation.</param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values for the received packet.</param>
        /// <param name="endPoint">The source <see cref="T:System.Net.EndPoint" />.</param>
        /// <param name="ipPacketInformation">The <see cref="T:System.Net.IPAddress" /> and interface of the received packet.</param>
        public int EndReceiveMessageFrom(IAsyncResult asyncResult, ref SocketFlags socketFlags, ref EndPoint endPoint, out IPPacketInformation ipPacketInformation)
        {
            return socket.EndReceiveMessageFrom(asyncResult, ref socketFlags, ref endPoint, out ipPacketInformation);
        }

        /// <summary>Ends a pending asynchronous send.</summary>
        /// <returns>If successful, the number of bytes sent to the <see cref="T:System.Net.Sockets.Socket" />; otherwise, an invalid <see cref="T:System.Net.Sockets.Socket" /> error.</returns>
        /// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> that stores state information for this asynchronous operation. </param>
        public int EndSend(IAsyncResult asyncResult)
        {
            return socket.EndSend(asyncResult);
        }

        /// <summary>Ends a pending asynchronous send.</summary>
        /// <returns>If successful, the number of bytes sent to the <see cref="T:System.Net.Sockets.Socket" />; otherwise, an invalid <see cref="T:System.Net.Sockets.Socket" /> error.</returns>
        /// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> that stores state information for this asynchronous operation.</param>
        /// <param name="errorCode">A <see cref="T:System.Net.Sockets.SocketError" /> object that stores the socket error.</param>
        public int EndSend(IAsyncResult asyncResult, out SocketError errorCode)
        {
            return socket.EndSend(asyncResult, out errorCode);
        }

        /// <summary>Ends a pending asynchronous send of a file.</summary>
        /// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> object that stores state information for this asynchronous operation. </param>
        public void EndSendFile(IAsyncResult asyncResult)
        {
            socket.EndSendFile(asyncResult);
        }

        /// <summary>Ends a pending asynchronous send to a specific location.</summary>
        /// <returns>If successful, the number of bytes sent; otherwise, an invalid <see cref="T:System.Net.Sockets.Socket" /> error.</returns>
        /// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> that stores state information and any user defined data for this asynchronous operation. </param>
        public int EndSendTo(IAsyncResult asyncResult)
        {
            return socket.EndSendTo(asyncResult);
        }

        /// <summary>
        /// Sends data to a connected <see cref="System.Net.Sockets.Socket"/>.
        /// </summary>
        /// <param name="buffer">An array of type <see cref="Byte" /> that contains the data to be sent.</param>
        /// <returns>The number of bytes sent to the <see cref="System.Net.Sockets.Socket"/>.</returns>
        public int Send(byte[] buffer)
        {
            return socket.Send(buffer);
        }

        /// <summary>
        /// Sends data to a connected <see cref="System.Net.Sockets.Socket" /> using the specified <see cref="SocketFlags" />.
        /// </summary>
        /// <param name="buffer">An array of type <see cref="Byte" /> that contains the data to be sent.</param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="SocketFlags" /> values.</param>
        /// <returns>The number of bytes sent to the <see cref="System.Net.Sockets.Socket"/>.</returns>
        public int Send(byte[] buffer, SocketFlags socketFlags)
        {
            return socket.Send(buffer, socketFlags);
        }

        /// <summary>
        ///  Sends the specified number of bytes of data to a connected <see cref="System.Net.Sockets.Socket" />, starting at the specified offset, and using the specified <see cref="SocketFlags" />.
        /// </summary>
        /// <param name="buffer">An array of type <see cref="Byte" /> that contains the data to be sent.</param>
        /// <param name="size">The number of bytes to send.</param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="SocketFlags" /> values.</param>
        /// <returns>The number of bytes sent to the <see cref="System.Net.Sockets.Socket"/>.</returns>
        public int Send(byte[] buffer, int size, SocketFlags socketFlags)
        {
            return socket.Send(buffer, size, socketFlags);
        }

        /// <summary>
        ///  Sends the specified number of bytes of data to a connected <see cref="System.Net.Sockets.Socket" />, starting at the specified offset, and using the specified <see cref="SocketFlags" />.
        /// </summary>
        /// <param name="buffer">An array of type <see cref="Byte" /> that contains the data to be sent.</param>
        /// <param name="offset">The position in the data buffer at which to begin sending data.</param>
        /// <param name="size">The number of bytes to send.</param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="SocketFlags" /> values.</param>
        /// <returns>The number of bytes sent to the <see cref="System.Net.Sockets.Socket"/>.</returns>
        public int Send(byte[] buffer, int offset, int size, SocketFlags socketFlags)
        {
            return socket.Send(buffer, offset, size, socketFlags);
        }

        /// <summary>
        ///  Sends the specified number of bytes of data to a connected <see cref="System.Net.Sockets.Socket" />, starting at the specified offset, and using the specified <see cref="SocketFlags" />.
        /// </summary>
        /// <param name="buffer">An array of type <see cref="Byte" /> that contains the data to be sent.</param>
        /// <param name="offset">The position in the data buffer at which to begin sending data.</param>
        /// <param name="size">The number of bytes to send.</param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="SocketFlags" /> values.</param>
        /// <param name="socketError">A <see cref="SocketError" /> object that stores the socket error.</param>
        /// <returns>The number of bytes sent to the <see cref="System.Net.Sockets.Socket"/>.</returns>
        public int Send(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError socketError)
        {
            return socket.Send(buffer, offset, size, socketFlags, out socketError);
        }

        /// <summary>Sends the set of buffers in the list to a connected <see cref="T:System.Net.Sockets.Socket" />.</summary>
        /// <param name="buffers">A list of <see cref="T:System.ArraySegment`1" />s of type <see cref="T:System.Byte" /> that contains the data to be sent. </param>
        /// <returns>The number of bytes sent to the <see cref="T:System.Net.Sockets.Socket" />.</returns>
        public int Send(IList<ArraySegment<byte>> buffers)
        {
            return socket.Send(buffers, SocketFlags.None);
        }

        /// <summary>Sends the set of buffers in the list to a connected <see cref="T:System.Net.Sockets.Socket" />, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
        /// <param name="buffers">A list of <see cref="T:System.ArraySegment`1" />s of type <see cref="T:System.Byte" /> that contains the data to be sent.</param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
        /// <returns>The number of bytes sent to the <see cref="T:System.Net.Sockets.Socket" />.</returns>
        public int Send(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags)
        {
            return socket.Send(buffers, socketFlags);
        }

        /// <summary>Sends the set of buffers in the list to a connected <see cref="T:System.Net.Sockets.Socket" />, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
        /// <param name="buffers">A list of <see cref="T:System.ArraySegment`1" />s of type <see cref="T:System.Byte" /> that contains the data to be sent.</param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
        /// <param name="errorCode">A <see cref="T:System.Net.Sockets.SocketError" /> object that stores the socket error.</param>
        /// <returns>The number of bytes sent to the <see cref="T:System.Net.Sockets.Socket" />.</returns>
        public int Send(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode)
        {
            return socket.Send(buffers, socketFlags, out errorCode);
        }

        /// <summary>
        /// Sends data asynchronously to a connected <see cref="System.Net.Sockets.Socket" /> object.
        /// </summary>
        /// <param name="e">The <see cref="SocketAsyncEventArgs" /> object to use for this asynchronous socket operation.</param>
        /// <returns>Returns true if the I/O operation is pending. The <see cref="SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will be raised upon completion of the operation. Returns false if the I/O operation completed synchronously. In this case, The <see cref="SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will not be raised and the <paramref name="e" /> object passed as a parameter may be examined immediately after the method call returns to retrieve the result of the operation.</returns>
        public bool SendAsync(SocketAsyncEventArgs e)
        {
            return socket.SendAsync(e);
        }

        /// <summary>
        /// Sends the file <paramref name="fileName" /> to a connected <see cref="System.Net.Sockets.Socket" /> object with the <see cref="TransmitFileOptions.UseDefaultWorkerThread" /> transmit flag.
        /// </summary>
        /// <param name="fileName">A <see cref="String" /> that contains the path and name of the file to be sent. This parameter can be null. </param>
        public void SendFile(string fileName)
        {
            socket.SendFile(fileName);
        }

        /// <summary>
        /// Sends the file <paramref name="fileName" /> and buffers of data to a connected <see cref="System.Net.Sockets.Socket" /> object using the specified <see cref="TransmitFileOptions" /> value.
        /// </summary>
        /// <param name="fileName">A <see cref="String" /> that contains the path and name of the file to be sent. This parameter can be null. </param>
        /// <param name="preBuffer">A <see cref="Byte" /> array that contains data to be sent before the file is sent. This parameter can be null. </param>
        /// <param name="postBuffer">A <see cref="Byte" /> array that contains data to be sent after the file is sent. This parameter can be null. </param>
        /// <param name="flags">One or more of <see cref="TransmitFileOptions" /> values. </param>
        public void SendFile(string fileName, byte[] preBuffer, byte[] postBuffer, TransmitFileOptions flags)
        {
            socket.SendFile(fileName, preBuffer, postBuffer, flags);
        }

        /// <summary>Sends the specified number of bytes of data to the specified endpoint, starting at the specified location in the buffer, and using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
        /// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the data to be sent. </param>
        /// <param name="offset">The position in the data buffer at which to begin sending data. </param>
        /// <param name="size">The number of bytes to send. </param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values. </param>
        /// <param name="remoteEp">The <see cref="T:System.Net.EndPoint" /> that represents the destination location for the data. </param>
        /// <returns>The number of bytes sent.</returns>
        public int SendTo(byte[] buffer, int offset, int size, SocketFlags socketFlags, EndPoint remoteEp)
        {
            return socket.SendTo(buffer, offset, size, socketFlags, remoteEp);
        }

        /// <summary>Sends the specified number of bytes of data to the specified endpoint using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
        /// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the data to be sent. </param>
        /// <param name="size">The number of bytes to send. </param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values. </param>
        /// <param name="remoteEp">The <see cref="T:System.Net.EndPoint" /> that represents the destination location for the data. </param>
        /// <returns>The number of bytes sent.</returns>
        public int SendTo(byte[] buffer, int size, SocketFlags socketFlags, EndPoint remoteEp)
        {
            return socket.SendTo(buffer, 0, size, socketFlags, remoteEp);
        }

        /// <summary>Sends data to a specific endpoint using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
        /// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the data to be sent. </param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values. </param>
        /// <param name="remoteEp">The <see cref="T:System.Net.EndPoint" /> that represents the destination location for the data. </param>
        /// <returns>The number of bytes sent.</returns>
        public int SendTo(byte[] buffer, SocketFlags socketFlags, EndPoint remoteEp)
        {
            return socket.SendTo(buffer, 0, (buffer != null) ? buffer.Length : 0, socketFlags, remoteEp);
        }

        /// <summary>Sends data to the specified endpoint.</summary>
        /// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the data to be sent. </param>
        /// <param name="remoteEp">The <see cref="T:System.Net.EndPoint" /> that represents the destination for the data. </param>
        /// <returns>The number of bytes sent.</returns>
        public int SendTo(byte[] buffer, EndPoint remoteEp)
        {
            return socket.SendTo(buffer, 0, (buffer != null) ? buffer.Length : 0, SocketFlags.None, remoteEp);
        }

        /// <summary>Sends data asynchronously to a specific remote host.</summary>
        /// <param name="e">The <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object to use for this asynchronous socket operation.</param>
        /// <returns>Returns true if the I/O operation is pending. The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will be raised upon completion of the operation. Returns false if the I/O operation completed synchronously. In this case, The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will not be raised and the <paramref name="e" /> object passed as a parameter may be examined immediately after the method call returns to retrieve the result of the operation.</returns>
        public bool SendToAsync(SocketAsyncEventArgs e)
        {
            return socket.SendAsync(e);
        }

        /// <summary>Receives the specified number of bytes of data from a bound <see cref="T:System.Net.Sockets.Socket" /> into a receive buffer, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
        /// <returns>The number of bytes received.</returns>
        /// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for the received data. </param>
        /// <param name="size">The number of bytes to receive. </param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values. </param>
        public int Receive(byte[] buffer, int size, SocketFlags socketFlags)
        {
            return socket.Receive(buffer, 0, size, socketFlags);
        }

        /// <summary>Receives data from a bound <see cref="T:System.Net.Sockets.Socket" /> into a receive buffer, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
        /// <returns>The number of bytes received.</returns>
        /// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for the received data. </param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values. </param>
        public int Receive(byte[] buffer, SocketFlags socketFlags)
        {
            return socket.Receive(buffer, 0, (buffer != null) ? buffer.Length : 0, socketFlags);
        }

        /// <summary>Receives data from a bound <see cref="T:System.Net.Sockets.Socket" /> into a receive buffer.</summary>
        /// <returns>The number of bytes received.</returns>
        /// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for the received data. </param>
        public int Receive(byte[] buffer)
        {
            return socket.Receive(buffer, 0, (buffer != null) ? buffer.Length : 0, SocketFlags.None);
        }

        /// <summary>Receives the specified number of bytes from a bound <see cref="T:System.Net.Sockets.Socket" /> into the specified offset position of the receive buffer, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
        /// <returns>The number of bytes received.</returns>
        /// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for received data. </param>
        /// <param name="offset">The location in <paramref name="buffer" /> to store the received data. </param>
        /// <param name="size">The number of bytes to receive. </param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values. </param>
        public int Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags)
        {
            return socket.Receive(buffer, offset, size, socketFlags);
        }

        /// <summary>Receives data from a bound <see cref="T:System.Net.Sockets.Socket" /> into a receive buffer, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
        /// <returns>The number of bytes received.</returns>
        /// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for the received data.</param>
        /// <param name="offset">The position in the <paramref name="buffer" /> parameter to store the received data. </param>
        /// <param name="size">The number of bytes to receive. </param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
        /// <param name="errorCode">A <see cref="T:System.Net.Sockets.SocketError" /> object that stores the socket error.</param>
        public int Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode)
        {
            return socket.Receive(buffer, offset, size, socketFlags, out errorCode);
        }

        /// <summary>Receives data from a bound <see cref="T:System.Net.Sockets.Socket" /> into the list of receive buffers.</summary>
        /// <returns>The number of bytes received.</returns>
        /// <param name="buffers">A list of <see cref="T:System.ArraySegment`1" />s of type <see cref="T:System.Byte" /> that contains the received data.</param>
        public int Receive(IList<ArraySegment<byte>> buffers)
        {
            return socket.Receive(buffers, SocketFlags.None);
        }

        /// <summary>Receives data from a bound <see cref="T:System.Net.Sockets.Socket" /> into the list of receive buffers, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
        /// <returns>The number of bytes received.</returns>
        /// <param name="buffers">A list of <see cref="T:System.ArraySegment`1" />s of type <see cref="T:System.Byte" /> that contains the received data.</param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
        public int Receive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags)
        {
            return socket.Receive(buffers, socketFlags);
        }

        /// <summary>Receives data from a bound <see cref="T:System.Net.Sockets.Socket" /> into the list of receive buffers, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
        /// <returns>The number of bytes received.</returns>
        /// <param name="buffers">A list of <see cref="T:System.ArraySegment`1" />s of type <see cref="T:System.Byte" /> that contains the received data.</param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
        /// <param name="errorCode">A <see cref="T:System.Net.Sockets.SocketError" /> object that stores the socket error.</param>
        public int Receive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode)
        {
            return socket.Receive(buffers, socketFlags, out errorCode);
        }

        /// <summary>Receives the specified number of bytes of data into the specified location of the data buffer, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />, and stores the endpoint and packet information.</summary>
        /// <returns>The number of bytes received.</returns>
        /// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for received data.</param>
        /// <param name="offset">The position in the <paramref name="buffer" /> parameter to store the received data.</param>
        /// <param name="size">The number of bytes to receive.</param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
        /// <param name="remoteEp">An <see cref="T:System.Net.EndPoint" />, passed by reference, that represents the remote server.</param>
        /// <param name="ipPacketInformation">An <see cref="T:System.Net.Sockets.IPPacketInformation" /> holding address and interface information.</param>
        public int ReceiveMessageFrom(byte[] buffer, int offset, int size, ref SocketFlags socketFlags, ref EndPoint remoteEp, out IPPacketInformation ipPacketInformation)
        {
            return socket.ReceiveMessageFrom(buffer, offset, size, ref socketFlags, ref remoteEp, out ipPacketInformation);
        }

        /// <summary>Receives the specified number of bytes of data into the specified location of the data buffer, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />, and stores the endpoint.</summary>
        /// <returns>The number of bytes received.</returns>
        /// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for received data. </param>
        /// <param name="offset">The position in the <paramref name="buffer" /> parameter to store the received data. </param>
        /// <param name="size">The number of bytes to receive. </param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values. </param>
        /// <param name="remoteEp">An <see cref="T:System.Net.EndPoint" />, passed by reference, that represents the remote server. </param>
        public int ReceiveFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEp)
        {
            return socket.ReceiveFrom(buffer, offset, size, socketFlags, ref remoteEp);
        }

        /// <summary>Receives the specified number of bytes into the data buffer, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />, and stores the endpoint.</summary>
        /// <returns>The number of bytes received.</returns>
        /// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for received data. </param>
        /// <param name="size">The number of bytes to receive. </param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values. </param>
        /// <param name="remoteEp">An <see cref="T:System.Net.EndPoint" />, passed by reference, that represents the remote server. </param>
        public int ReceiveFrom(byte[] buffer, int size, SocketFlags socketFlags, ref EndPoint remoteEp)
        {
            return socket.ReceiveFrom(buffer, 0, size, socketFlags, ref remoteEp);
        }

        /// <summary>Receives a datagram into the data buffer, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />, and stores the endpoint.</summary>
        /// <returns>The number of bytes received.</returns>
        /// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for the received data. </param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values. </param>
        /// <param name="remoteEp">An <see cref="T:System.Net.EndPoint" />, passed by reference, that represents the remote server. </param>
        public int ReceiveFrom(byte[] buffer, SocketFlags socketFlags, ref EndPoint remoteEp)
        {
            return socket.ReceiveFrom(buffer, 0, (buffer != null) ? buffer.Length : 0, socketFlags, ref remoteEp);
        }

        /// <summary>Receives a datagram into the data buffer and stores the endpoint.</summary>
        /// <returns>The number of bytes received.</returns>
        /// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for received data. </param>
        /// <param name="remoteEp">An <see cref="T:System.Net.EndPoint" />, passed by reference, that represents the remote server. </param>
        public int ReceiveFrom(byte[] buffer, ref EndPoint remoteEp)
        {
            return socket.ReceiveFrom(buffer, 0, (buffer != null) ? buffer.Length : 0, SocketFlags.None, ref remoteEp);
        }

        /// <summary>Sends a collection of files or in memory data buffers asynchronously to a connected <see cref="T:System.Net.Sockets.Socket" /> object.</summary>
        /// <returns>Returns true if the I/O operation is pending. The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will be raised upon completion of the operation. Returns false if the I/O operation completed synchronously. In this case, The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will not be raised and the <paramref name="e" /> object passed as a parameter may be examined immediately after the method call returns to retrieve the result of the operation.</returns>
        /// <param name="e">The <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object to use for this asynchronous socket operation.</param>
        public bool SendPacketsAsync(SocketAsyncEventArgs e)
        {
            return socket.SendPacketsAsync(e);
        }

        /// <summary>Sets low-level operating modes for the <see cref="T:System.Net.Sockets.Socket" /> using numerical control codes.</summary>
        /// <returns>The number of bytes in the <paramref name="optionOutValue" /> parameter.</returns>
        /// <param name="ioControlCode">An <see cref="T:System.Int32" /> value that specifies the control code of the operation to perform. </param>
        /// <param name="optionInValue">A <see cref="T:System.Byte" /> array that contains the input data required by the operation. </param>
        /// <param name="optionOutValue">A <see cref="T:System.Byte" /> array that contains the output data returned by the operation. </param>
        public int IOControl(int ioControlCode, byte[] optionInValue, byte[] optionOutValue)
        {
            return socket.IOControl(ioControlCode, optionInValue, optionOutValue);
        }

        /// <summary>Sets low-level operating modes for the <see cref="T:System.Net.Sockets.Socket" /> using the <see cref="T:System.Net.Sockets.IOControlCode" /> enumeration to specify control codes.</summary>
        /// <returns>The number of bytes in the <paramref name="optionOutValue" /> parameter.</returns>
        /// <param name="ioControlCode">A <see cref="T:System.Net.Sockets.IOControlCode" /> value that specifies the control code of the operation to perform. </param>
        /// <param name="optionInValue">An array of type <see cref="T:System.Byte" /> that contains the input data required by the operation. </param>
        /// <param name="optionOutValue">An array of type <see cref="T:System.Byte" /> that contains the output data returned by the operation. </param>
        public int IOControl(IOControlCode ioControlCode, byte[] optionInValue, byte[] optionOutValue)
        {
            return socket.IOControl(ioControlCode, optionInValue, optionOutValue);
        }

        /// <summary>Disables sends and receives on a <see cref="T:System.Net.Sockets.Socket" />.</summary>
        /// <param name="how">One of the <see cref="T:System.Net.Sockets.SocketShutdown" /> values that specifies the operation that will no longer be allowed. </param>
        public void Shutdown(SocketShutdown how)
        {
            socket.Shutdown(how);
        }

        ////////////////////////////////////////////////////////

        /// <summary>
        /// Sends data to a connected <see cref="System.Net.Sockets.Socket"/>.
        /// </summary>
        /// <param name="data">An string of type <see cref="string" /> that contains the data to be sent.</param>
        /// <returns>The number of bytes sent to the <see cref="System.Net.Sockets.Socket"/>.</returns>
        public int Send(string data)
        {
            byte[] buffers = encoding.GetBytes(data);
            return socket.Send(buffers);
        }

        /// <summary>
        /// Sends data to a connected <see cref="System.Net.Sockets.Socket" /> using the specified <see cref="SocketFlags" />.
        /// </summary>
        /// <param name="data">An string of type <see cref="string" /> that contains the data to be sent.</param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="SocketFlags" /> values.</param>
        /// <returns>The number of bytes sent to the <see cref="System.Net.Sockets.Socket"/>.</returns>
        public int Send(string data, SocketFlags socketFlags)
        {
            byte[] buffers = encoding.GetBytes(data);
            return socket.Send(buffers, socketFlags);
        }

        /// <summary>
        ///  Sends the specified number of bytes of data to a connected <see cref="System.Net.Sockets.Socket" />, starting at the specified offset, and using the specified <see cref="SocketFlags" />.
        /// </summary>
        /// <param name="data">An string of type <see cref="string" /> that contains the data to be sent.</param>
        /// <param name="size">The number of bytes to send.</param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="SocketFlags" /> values.</param>
        /// <returns>The number of bytes sent to the <see cref="System.Net.Sockets.Socket"/>.</returns>
        public int Send(string data, int size, SocketFlags socketFlags)
        {
            byte[] buffers = encoding.GetBytes(data);
            return socket.Send(buffers, size, socketFlags);
        }

        /// <summary>
        ///  Sends the specified number of bytes of data to a connected <see cref="System.Net.Sockets.Socket" />, starting at the specified offset, and using the specified <see cref="SocketFlags" />.
        /// </summary>
        /// <param name="data">An string of type <see cref="string" /> that contains the data to be sent.</param>
        /// <param name="offset">The position in the data buffer at which to begin sending data.</param>
        /// <param name="size">The number of bytes to send.</param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="SocketFlags" /> values.</param>
        /// <returns>The number of bytes sent to the <see cref="System.Net.Sockets.Socket"/>.</returns>
        public int Send(string data, int offset, int size, SocketFlags socketFlags)
        {
            byte[] buffers = encoding.GetBytes(data);
            return socket.Send(buffers, offset, size, socketFlags);
        }

        /// <summary>
        ///  Sends the specified number of bytes of data to a connected <see cref="System.Net.Sockets.Socket" />, starting at the specified offset, and using the specified <see cref="SocketFlags" />.
        /// </summary>
        /// <param name="data">An string of type <see cref="string" /> that contains the data to be sent.</param>
        /// <param name="offset">The position in the data buffer at which to begin sending data.</param>
        /// <param name="size">The number of bytes to send.</param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="SocketFlags" /> values.</param>
        /// <param name="socketError">A <see cref="SocketError" /> object that stores the socket error.</param>
        /// <returns>The number of bytes sent to the <see cref="System.Net.Sockets.Socket"/>.</returns>
        public int Send(string data, int offset, int size, SocketFlags socketFlags, out SocketError socketError)
        {
            byte[] buffers = encoding.GetBytes(data);
            return socket.Send(buffers, offset, size, socketFlags, out socketError);
        }

        ////////////////////////////////////////////////////////


        /// <summary>
        /// The method for processing socket received.
        /// </summary>
        /// <param name="obj"></param>
        internal void ProcessData<T>(object obj) where T :SocketBase
        {
            try
            {
                SocketResult<T> result = obj as SocketResult<T>;
                if (result == null)
                {
                    return;
                }

                string data = Join(" ", result.Bytes);
                Console.WriteLine($"{result.Socket.LocalEndPoint} receive data from {result.Socket.RemoteEndPoint}: {data}");
                result.OnData?.Invoke(result.Socket, result.Bytes);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Stop Socket.
        /// </summary>
        public void Stop()
        {
            try
            {
                loopFlag = false;
                while (this.status == SocketStatus.Runing)
                {
                    Thread.Sleep(100);
                }

                if (threadHandle.ThreadState != ThreadState.Stopped)
                {
                    threadHandle.Abort();
                    threadHandle = null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static string Join(string separator, byte[] values)
        {
            if (values == null || values.Length == 0)
            {
                return string.Empty;
            }

            if (separator == null)
            {
                separator = string.Empty;
            }
            StringBuilder stringBuilder = new StringBuilder();
            string text = values[0].ToString();
            if (text != null)
            {
                stringBuilder.Append(text);
            }
            for (int i = 1; i < values.Length; i++)
            {
                stringBuilder.Append(separator);
                text = values[i].ToString();
                if (text != null)
                {
                    stringBuilder.Append(text);
                }
            }
            return stringBuilder.ToString().Trim();
        }
    }
}
