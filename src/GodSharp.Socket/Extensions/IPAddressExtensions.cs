using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace GodSharp.Sockets.Extensions
{
    public static class IPAddressExtensions
    {
        /// <summary>
        /// Ases the specified port.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="port">The port.</param>
        /// <returns></returns>
        public static IPEndPoint As(this IPAddress address, int port) => new IPEndPoint(address, port);

        /// <summary>
        /// Ases the specified port.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="port">The port.</param>
        /// <returns></returns>
        public static IEnumerable<IPEndPoint> As(this IPAddress[] address, int port) => address?.Select(x => new IPEndPoint(x, port));
    }
}
