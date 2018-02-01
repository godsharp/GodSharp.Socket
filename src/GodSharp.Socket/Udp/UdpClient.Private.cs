using System;
using System.Net.Sockets;

namespace GodSharp.Sockets
{
    /// <summary>
    /// Socket udp client
    /// </summary>
    public partial class UdpClient
    {
        /// <summary>
        /// Sets the host.
        /// </summary>
        /// <param name="host">The host.</param>
        private void SetHost(string host)
        {
            Exception ex = Utils.ValidateHost(host);
            if (ex == null)
            {
                Host = host;
            }
            else
            {
                throw ex;
            }
        }

        /// <summary>
        /// Sets the local port.
        /// </summary>
        /// <param name="port">The port.</param>
        private void SetLocalPort(int port)
        {
            Exception ex = Utils.ValidatePort(port);
            if (ex == null)
            {
                LocalPort = port;
            }
            else
            {
                throw ex;
            }
        }

        /// <summary>
        /// Sets the port.
        /// </summary>
        /// <param name="port">The port.</param>
        private void SetPort(int port)
        {
            Exception ex = Utils.ValidatePort(port);
            if (ex == null)
            {
                Port = port;
            }
            else
            {
                throw ex;
            }
        }

        /// <summary>
        /// Sets the address family.
        /// </summary>
        /// <param name="addressFamily">The address family.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">addressFamily - AddressFamily</exception>
        private void SetAddressFamily(AddressFamily addressFamily)
        {
            if (addressFamily== AddressFamily.InterNetwork || addressFamily== AddressFamily.InterNetworkV6)
            {
                family = addressFamily;
                return;
            }

            throw new ArgumentOutOfRangeException(nameof(addressFamily), $"{nameof(AddressFamily)} only support {nameof(AddressFamily.InterNetwork)} and {nameof(AddressFamily.InterNetworkV6)}");
        }
    }
}
