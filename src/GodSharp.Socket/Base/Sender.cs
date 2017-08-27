using GodSharp.Sockets.Internal.Extension;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace GodSharp.Sockets
{
    public class Sender
    {
        private Listener listener;
        private Encoding encoding;

        /// <summary>
        /// Gets the unique identifier.
        /// </summary>
        /// <value>
        /// The unique identifier.
        /// </value>
        public Guid Guid => this.listener.Guid;

        /// <summary>
        /// Gets the remote end point.
        /// </summary>
        /// <value>
        /// The remote end point.
        /// </value>
        public EndPoint RemoteEndPoint => this.listener.RemoteEndPoint;

        /// <summary>
        /// Gets the local end point.
        /// </summary>
        /// <value>
        /// The local end point.
        /// </value>
        public EndPoint LocalEndPoint => this.listener.LocalEndPoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="Sender"/> class.
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <param name="encoding">The encoding.</param>
        internal Sender(Listener listener,Encoding encoding)
        {
            this.listener = listener;
            this.encoding = encoding;
        }

        /// <summary>
        /// Sends the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public int Send(string data)
        {
            try
            {
                ValidateConnected();

                return this.listener.Socket.Send(data, this.encoding);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Sends the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        public int Send(byte[] buffer)
        {
            try
            {
                ValidateConnected();

                return this.listener.Socket.Send(buffer);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Sends the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="socketFlags">The socket flags.</param>
        /// <returns></returns>
        public int Send(string data, SocketFlags socketFlags)
        {
            try
            {
                ValidateConnected();

                return this.listener.Socket.Send(data, socketFlags, this.encoding);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Sends the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="socketFlags">The socket flags.</param>
        /// <returns></returns>
        public int Send(byte[] buffer, SocketFlags socketFlags)
        {
            try
            {
                ValidateConnected();

                return this.listener.Socket.Send(buffer, socketFlags);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Sends the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="socketFlags">The socket flags.</param>
        /// <param name="socketError">The socket error.</param>
        /// <returns></returns>
        public int Send(string data, SocketFlags socketFlags, out SocketError socketError)
        {
            try
            {
                ValidateConnected();

                return this.listener.Socket.Send(data, socketFlags, out socketError, this.encoding);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Sends the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="socketFlags">The socket flags.</param>
        /// <param name="socketError">The socket error.</param>
        /// <returns></returns>
        public int Send(byte[] buffer, SocketFlags socketFlags, out SocketError socketError)
        {
            try
            {
                ValidateConnected();

                return this.listener.Socket.Send(buffer, 0, buffer.Length, socketFlags, out socketError);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates the connected.
        /// </summary>
        /// <exception cref="SocketException"></exception>
        private void ValidateConnected()
        {
            if (!this.listener.Connected)
            {
                throw new SocketException((int)SocketError.NotConnected);
            }
        }
    }
}