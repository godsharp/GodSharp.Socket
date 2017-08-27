using System;
using System.Net;

namespace GodSharp.Sockets.Internal
{
    public class Utils
    {
        /// <summary>
        /// Validates the port.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <returns></returns>
        internal static Exception ValidatePort(int port)
        {
            if (port > 0)
            {
                return null;
            }
            else
            {
                return new FormatException("port must be greater than 0");
            }
        }

        /// <summary>
        /// Validates the host.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <returns></returns>
        internal static Exception ValidateHost(string host)
        {
            if (string.IsNullOrEmpty(host))
            {
                return new ArgumentNullException(nameof(host));
            }

            IPAddress address = null;
            if (IPAddress.TryParse(host, out address))
            {
                return null;
            }
            else
            {
                return new FormatException("host format is incorrect");
            }
        }
    }
}
