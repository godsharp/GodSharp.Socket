using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Linq;

namespace GodSharp
{
    public static class SocketExtension
    {
        /// <summary>
        /// Sends data to a connected <see cref="Socket"/>.
        /// </summary>
        /// <param name="data">An string of type <see cref="string" /> that contains the data to be sent.</param>
        /// <returns>The number of bytes sent to the <see cref="Socket"/>.</returns>
        public static int Send(this Socket socket, string data)
        {
            byte[] buffers = Encoding.UTF8.GetBytes(data);
            return socket.Send(buffers);
        }

        /// <summary>
        /// Sends data to a connected <see cref="Socket" /> using the specified <see cref="SocketFlags" />.
        /// </summary>
        /// <param name="data">An string of type <see cref="string" /> that contains the data to be sent.</param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="SocketFlags" /> values.</param>
        /// <returns>The number of bytes sent to the <see cref="Socket"/>.</returns>
        public static int Send(this Socket socket, string data, SocketFlags socketFlags)
        {
            byte[] buffers = Encoding.UTF8.GetBytes(data);
            return socket.Send(buffers, socketFlags);
        }

        /// <summary>
        ///  Sends the specified number of bytes of data to a connected <see cref="Socket" />, starting at the specified offset, and using the specified <see cref="SocketFlags" />.
        /// </summary>
        /// <param name="data">An string of type <see cref="string" /> that contains the data to be sent.</param>
        /// <param name="size">The number of bytes to send.</param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="SocketFlags" /> values.</param>
        /// <returns>The number of bytes sent to the <see cref="Socket"/>.</returns>
        public static int Send(this Socket socket, string data, int size, SocketFlags socketFlags)
        {
            byte[] buffers = Encoding.UTF8.GetBytes(data);
            return socket.Send(buffers, size, socketFlags);
        }

        /// <summary>
        ///  Sends the specified number of bytes of data to a connected <see cref="Socket" />, starting at the specified offset, and using the specified <see cref="SocketFlags" />.
        /// </summary>
        /// <param name="data">An string of type <see cref="string" /> that contains the data to be sent.</param>
        /// <param name="offset">The position in the data buffer at which to begin sending data.</param>
        /// <param name="size">The number of bytes to send.</param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="SocketFlags" /> values.</param>
        /// <returns>The number of bytes sent to the <see cref="Socket"/>.</returns>
        public static int Send(this Socket socket, string data, int offset, int size, SocketFlags socketFlags)
        {
            byte[] buffers = Encoding.UTF8.GetBytes(data);
            return socket.Send(buffers, offset, size, socketFlags);
        }

        /// <summary>
        ///  Sends the specified number of bytes of data to a connected <see cref="Socket" />, starting at the specified offset, and using the specified <see cref="SocketFlags" />.
        /// </summary>
        /// <param name="data">An string of type <see cref="string" /> that contains the data to be sent.</param>
        /// <param name="offset">The position in the data buffer at which to begin sending data.</param>
        /// <param name="size">The number of bytes to send.</param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="SocketFlags" /> values.</param>
        /// <param name="socketError">A <see cref="SocketError" /> object that stores the socket error.</param>
        /// <returns>The number of bytes sent to the <see cref="Socket"/>.</returns>
        public static int Send(this Socket socket, string data, int offset, int size, SocketFlags socketFlags, out SocketError socketError)
        {
            byte[] buffers = Encoding.UTF8.GetBytes(data);
            return socket.Send(buffers, offset, size, socketFlags, out socketError);
        }
    }
}
