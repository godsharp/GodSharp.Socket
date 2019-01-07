using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace GodSharp.Sockets
{
    public static class SocketExtensions
    {
        /// <summary>
        /// Sends data to a connected <see cref="Socket"/>.
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <param name="data">An string of type <see cref="string" /> that contains the data to be sent.</param>
        /// <param name="encoding">The <see cref="Encoding"/> for data, default is <see cref="Encoding.UTF8"/>.</param>
        /// <returns>The number of bytes sent to the <see cref="Socket"/>.</returns>
        public static int Send(this Socket socket, string data, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;

            return socket.Send(encoding.GetBytes(data));
        }

        /// <summary>
        /// Begins the send.
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <param name="data">The data.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="state">The state.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public static IAsyncResult BeginSend(this Socket socket, string data, AsyncCallback callback, object state, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            byte[] buffers = encoding.GetBytes(data);
            return socket.BeginSend(buffers, 0, buffers.Length, SocketFlags.None, callback, state);
        }

        /// <summary>
        /// Sends data to a connected <see cref="Socket" /> using the specified <see cref="SocketFlags" />.
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <param name="data">An string of type <see cref="string" /> that contains the data to be sent.</param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="SocketFlags" /> values.</param>
        /// <param name="encoding">The <see cref="Encoding"/> for data, default is <see cref="Encoding.UTF8"/>.</param>
        /// <returns>The number of bytes sent to the <see cref="Socket"/>.</returns>
        public static int Send(this Socket socket, string data, SocketFlags socketFlags, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            
            return socket.Send(encoding.GetBytes(data), socketFlags);
        }

        /// <summary>
        ///  Sends the specified number of bytes of data to a connected <see cref="Socket" />, starting at the specified offset, and using the specified <see cref="SocketFlags" />.
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <param name="data">An string of type <see cref="string" /> that contains the data to be sent.</param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="SocketFlags" /> values.</param>
        /// <param name="socketError">A <see cref="SocketError" /> object that stores the socket error.</param>
        /// <param name="encoding">The <see cref="Encoding"/> for data, default is <see cref="Encoding.UTF8"/>.</param>
        /// <returns>The number of bytes sent to the <see cref="Socket"/>.</returns>
        public static int Send(this Socket socket, string data, SocketFlags socketFlags, out SocketError socketError, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            byte[] buffers = encoding.GetBytes(data);
            return socket.Send(buffers, 0, buffers.Length, socketFlags, out socketError);
        }

        /// <summary>
        /// Sends data to a <see cref="Socket"/>.
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <param name="data">An string of type <see cref="string" /> that contains the data to be sent.</param>
        /// <param name="endPoint">The end point.</param>
        /// <param name="encoding">The <see cref="Encoding"/> for data, default is <see cref="Encoding.UTF8"/>.</param>
        /// <returns>The number of bytes sent to the <see cref="Socket"/>.</returns>
        public static int SendTo(this Socket socket, string data, EndPoint endPoint, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;

            return socket.SendTo(encoding.GetBytes(data), endPoint);
        }

        /// <summary>
        ///  Sends the specified number of bytes of data to a <see cref="Socket" />, using the specified <see cref="SocketFlags" />.
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <param name="data">An string of type <see cref="string" /> that contains the data to be sent.</param>
        /// <param name="socketFlags">The socket flags.</param>
        /// <param name="endPoint">The end point.</param>
        /// <param name="encoding">The <see cref="Encoding"/> for data, default is <see cref="Encoding.UTF8"/>.</param>
        /// <returns>The number of bytes sent to the <see cref="Socket"/>.</returns>
        public static int SendTo(this Socket socket, string data, SocketFlags socketFlags, EndPoint endPoint, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;

            return socket.SendTo(encoding.GetBytes(data), socketFlags, endPoint);
        }

        /// <summary>
        /// Begins the send to.
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <param name="buffers">The buffers.</param>
        /// <param name="remoteEP">The remote ep.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        public static IAsyncResult BeginSendTo(this Socket socket, byte[] buffers, EndPoint remoteEP, AsyncCallback callback, object state) => socket.BeginSendTo(buffers, 0, buffers.Length, SocketFlags.None, remoteEP, callback, state);

        /// <summary>
        /// Begins the send to.
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <param name="data">The data.</param>
        /// <param name="remoteEP">The remote ep.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="state">The state.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public static IAsyncResult BeginSendTo(this Socket socket, string data, EndPoint remoteEP, AsyncCallback callback, object state, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            byte[] buffers = encoding.GetBytes(data);
            return socket.BeginSendTo(buffers, 0, buffers.Length, SocketFlags.None, remoteEP, callback, state);
        }
    }
}