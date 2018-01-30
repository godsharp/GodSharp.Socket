using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace GodSharp.Sockets.Internal.Extension
{
    internal static class SocketExtension
    {
        /// <summary>
        /// Sends data to a connected <see cref="Socket"/>.
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <param name="data">An string of type <see cref="string" /> that contains the data to be sent.</param>
        /// <param name="encoding">The <see cref="Encoding"/> for data.</param>
        /// <returns>The number of bytes sent to the <see cref="Socket"/>.</returns>
        public static int Send(this Socket socket, string data, Encoding encoding)
        {
            byte[] buffers = encoding.GetBytes(data);
            return socket.Send(buffers);
        }

        /// <summary>
        /// Sends data to a connected <see cref="Socket" /> using the specified <see cref="SocketFlags" />.
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <param name="data">An string of type <see cref="string" /> that contains the data to be sent.</param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="SocketFlags" /> values.</param>
        /// <param name="encoding">The <see cref="Encoding"/> for data.</param>
        /// <returns>The number of bytes sent to the <see cref="Socket"/>.</returns>
        public static int Send(this Socket socket, string data, SocketFlags socketFlags, Encoding encoding)
        {
            byte[] buffers = encoding.GetBytes(data);
            return socket.Send(buffers, socketFlags);
        }

        /// <summary>
        ///  Sends the specified number of bytes of data to a connected <see cref="Socket" />, starting at the specified offset, and using the specified <see cref="SocketFlags" />.
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <param name="data">An string of type <see cref="string" /> that contains the data to be sent.</param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="SocketFlags" /> values.</param>
        /// <param name="socketError">A <see cref="SocketError" /> object that stores the socket error.</param>
        /// <param name="encoding">The <see cref="Encoding"/> for data.</param>
        /// <returns>The number of bytes sent to the <see cref="Socket"/>.</returns>
        public static int Send(this Socket socket, string data, SocketFlags socketFlags, out SocketError socketError, Encoding encoding)
        {
            byte[] buffers = encoding.GetBytes(data);
            return socket.Send(buffers, 0, buffers.Length, socketFlags, out socketError);
        }
        
        /// <summary>
        /// Sends data to a <see cref="Socket"/>.
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <param name="data">An string of type <see cref="string" /> that contains the data to be sent.</param>
        /// <param name="endPoint">The end point.</param>
        /// <param name="encoding">The <see cref="Encoding"/> for data.</param>
        /// <returns>The number of bytes sent to the <see cref="Socket"/>.</returns>
        public static int SendTo(this Socket socket, string data, EndPoint endPoint, Encoding encoding)
        {
            byte[] buffers = encoding.GetBytes(data);
            return socket.SendTo(buffers, endPoint);
        }

        /// <summary>
        ///  Sends the specified number of bytes of data to a <see cref="Socket" />, using the specified <see cref="SocketFlags" />.
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <param name="data">An string of type <see cref="string" /> that contains the data to be sent.</param>
        /// <param name="socketFlags">The socket flags.</param>
        /// <param name="endPoint">The end point.</param>
        /// <param name="encoding">The <see cref="Encoding"/> for data.</param>
        /// <returns>The number of bytes sent to the <see cref="Socket"/>.</returns>
        public static int SendTo(this Socket socket, string data, SocketFlags socketFlags, EndPoint endPoint, Encoding encoding)
        {
            byte[] buffers = encoding.GetBytes(data);
            return socket.SendTo(buffers, socketFlags, endPoint);
        }

        /// <summary>
        /// Set <see cref="Socket"/> keep-alive option.
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <param name="on">if set to <c>true</c> [on].</param>
        /// <param name="time">The time.</param>
        /// <param name="interval">The interval.</param>
        /// <returns>return <see cref="bool"/> value of KeepAlive value.</returns>
        public static bool KeepAlive(this Socket socket, bool on = true, uint time = 5000, uint interval = 5000)
        {
            try
            {
                byte[] inOptionValues = new byte[12];

                uint off = (uint)(on ? 1 : 0);

                BitConverter.GetBytes(off).CopyTo(inOptionValues, 0);
                BitConverter.GetBytes(interval).CopyTo(inOptionValues, 4);
                BitConverter.GetBytes(interval).CopyTo(inOptionValues, 8);

                socket.IOControl(IOControlCode.KeepAliveValues, inOptionValues, null);

                object obj = socket.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive);

                return Convert.ToInt32(obj) == 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Determines whether this instance is connected.
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <returns>
        ///   <c>true</c> if the specified socket is connected; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">socket</exception>
        public static bool IsConnected(this Socket socket)
        {
            if (socket == null)
            {
                throw new ArgumentNullException(nameof(socket));
            }

            // This is how you can determine whether a socket is still connected.
            bool blockingState = socket.Blocking;

            try
            {
                byte[] tmp = new byte[1];

                socket.Blocking = false;
                socket.Send(tmp, 0, 0);

                return true;
            }
            catch (SocketException e)
            {
                // 10035 == WSAEWOULDBLOCK
                if (e.NativeErrorCode.Equals(10035))
                {
#if DEBUG
                    Console.WriteLine("Still Connected, but the Send would block");
#endif
                    return true;
                }
                else
                {
#if DEBUG
                    Console.WriteLine("Disconnected: error code {0}!", e.NativeErrorCode);
#endif
                    return false;
                }
            }
            finally
            {
                socket.Blocking = blockingState;
            }
        }
    }
}
