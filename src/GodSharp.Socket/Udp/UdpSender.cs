using GodSharp.Sockets.Internal.Extension;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace GodSharp.Sockets
{
    /// <summary>
    /// Udp sender.
    /// </summary>
    public class UdpSender
    {
        private Socket socket;

        /// <summary>
        /// Gets the unique identifier.
        /// </summary>
        /// <value>
        /// The unique identifier.
        /// </value>
        public Guid Guid { get; private set; }

        /// <summary>
        /// Gets the encoding.
        /// </summary>
        /// <value>
        /// The encoding.
        /// </value>
        public Encoding Encoding { get; internal set; }

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
        public EndPoint LocalEndPoint { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UdpSender"/> class.
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <param name="remote">The remote.</param>
        /// <param name="guid">The unique identifier.</param>
        /// <param name="encoding">The encoding.</param>
        public UdpSender(Socket socket, EndPoint remote, Guid guid,Encoding encoding)
        {
            this.socket = socket;

            LocalEndPoint = socket.LocalEndPoint;
            RemoteEndPoint = remote;
            Guid = guid;
            Encoding = encoding;
        }

        #region Send with default RemoteEndPoint
        /// <summary>
        /// Sends the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public int Send(string data)
        {
            try
            {
                if (RemoteEndPoint==null)
                {
                    return -1;
                }

                return socket.SendTo(data, RemoteEndPoint, this.Encoding);
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
                if (RemoteEndPoint == null)
                {
                    return -1;
                }

                return socket.SendTo(buffer, RemoteEndPoint);
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
                if (RemoteEndPoint == null)
                {
                    return -1;
                }

                return socket.SendTo(data, socketFlags, RemoteEndPoint, this.Encoding);
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
                if (RemoteEndPoint == null)
                {
                    return -1;
                }

                return socket.SendTo(buffer, socketFlags, RemoteEndPoint);
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
        /// <param name="size">The size.</param>
        /// <param name="socketFlags">The socket flags.</param>
        /// <returns></returns>
        public int Send(byte[] buffer, int size, SocketFlags socketFlags)
        {
            try
            {
                if (RemoteEndPoint == null)
                {
                    return -1;
                }

                return socket.SendTo(buffer, size, socketFlags, RemoteEndPoint);
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
        /// <param name="offset">The offset.</param>
        /// <param name="size">The size.</param>
        /// <param name="socketFlags">The socket flags.</param>
        /// <returns></returns>
        public int Send(byte[] buffer, int offset, int size, SocketFlags socketFlags)
        {
            try
            {
                if (RemoteEndPoint == null)
                {
                    return -1;
                }

                return socket.SendTo(buffer, offset, size, socketFlags, RemoteEndPoint);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Send with the specified RemoteEndPoint
        /// <summary>
        /// Sends the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public int Send(string data, EndPoint endPoint)
        {
            try
            {
                return socket.SendTo(data, endPoint, this.Encoding);
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
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public int Send(byte[] buffer, EndPoint endPoint)
        {
            try
            {
                return socket.SendTo(buffer, endPoint);
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
        /// <param name="endPoint">The end point.</param>
        /// <returns></returns>
        public int Send(string data, SocketFlags socketFlags, EndPoint endPoint)
        {
            try
            {
                return socket.SendTo(data, socketFlags, endPoint, this.Encoding);
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
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public int Send(byte[] buffer, SocketFlags socketFlags, EndPoint endPoint)
        {
            try
            {
                return socket.SendTo(buffer, socketFlags, endPoint);
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
        /// <param name="size">The size.</param>
        /// <param name="socketFlags">The socket flags.</param>
        /// <param name="endPoint">The end point.</param>
        /// <returns></returns>
        public int Send(byte[] buffer, int size, SocketFlags socketFlags, EndPoint endPoint)
        {
            try
            {
                return socket.SendTo(buffer, size, socketFlags, endPoint);
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
        /// <param name="offset">The offset.</param>
        /// <param name="size">The size.</param>
        /// <param name="socketFlags">The socket flags.</param>
        /// <param name="endPoint">The end point.</param>
        /// <returns></returns>
        public int Send(byte[] buffer, int offset, int size, SocketFlags socketFlags, EndPoint endPoint)
        {
            try
            {
                return socket.SendTo(buffer, offset, size, socketFlags, endPoint);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
        #endregion
    }
}
