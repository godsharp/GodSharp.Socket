using System;
using System.Net.Sockets;

namespace GodSharp.Sockets
{
    /// <summary>
    /// Extension class.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Determines whether this instance is connected.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <returns>
        ///   <c>true</c> if the specified client is connected; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsConnected(this SocketBase client)
        {
            return client.socket.IsConnected();
        }

        /// <summary>
        /// Set <see cref="SocketClient"/> keep-alive option.
        /// </summary>
        /// <param name="client">The socket client.</param>
        /// <param name="on">if set to <c>true</c> [on].</param>
        /// <param name="time">The time.</param>
        /// <param name="interval">The interval.</param>
        /// <returns>return <see cref="bool"/> value of KeepAlive value.</returns>
        public static bool KeepAlive(this SocketBase client, bool on = true, uint time = 5000, uint interval = 5000)
        {
            try
            {
                byte[] inOptionValues = new byte[12];

                uint off = (uint)(on ? 1 : 0);

                BitConverter.GetBytes(off).CopyTo(inOptionValues, 0);
                BitConverter.GetBytes(interval).CopyTo(inOptionValues, 4);
                BitConverter.GetBytes(interval).CopyTo(inOptionValues, 8);

                client.socket.IOControl(IOControlCode.KeepAliveValues, inOptionValues, null);

                object obj = client.socket.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive);

                return Convert.ToInt32(obj) == 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
