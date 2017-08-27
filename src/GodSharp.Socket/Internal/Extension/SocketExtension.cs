using System.Net.Sockets;
using System.Text;

namespace GodSharp.Sockets.Internal.Extension
{
    internal static class SocketExtension
    {
        /// <summary>
        /// Sends data to a connected <see cref="Socket"/>.
        /// </summary>
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
    }
}
