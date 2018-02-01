using System.Net;

namespace GodSharp.Sockets
{
    internal static class EndPointExtension
    {
        /// <summary>
        /// Gets the host.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns></returns>
        public static string GetHost(this EndPoint point)
        {
            IPEndPoint ipoint = point as IPEndPoint;
            return ipoint.Address.ToString();
        }

        /// <summary>
        /// Gets the port.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns></returns>
        public static int GetPort(this EndPoint point)
        {
            IPEndPoint ipoint = point as IPEndPoint;
            return ipoint.Port;
        }
    }
}
