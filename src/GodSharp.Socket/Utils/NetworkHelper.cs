using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace GodSharp.Sockets.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public class NetworkHelper
    {
        /// <summary>
        /// Locals the port used.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <returns></returns>
        public static bool LocalPortUsed(int port)
        {
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();

            if (properties.GetActiveTcpConnections().FirstOrDefault(x => x.LocalEndPoint.Port == port) != null) return true;
            if (properties.GetActiveTcpListeners().FirstOrDefault(x => x.Port == port) != null) return true;
            if (properties.GetActiveUdpListeners().FirstOrDefault(x => x.Port == port) != null) return true;

            return false;
        }

        /// <summary>
        /// Gets the TCP connection information.
        /// </summary>
        /// <param name="local">The local.</param>
        /// <param name="remote">The remote.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static TcpConnectionInformation GetTcpConnectionInformation(int local, int remote = 0)
        {
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();

            Func<TcpConnectionInformation, bool> func = null;

            if (local > 0 && remote > 0) func = x => x.LocalEndPoint.Port == local && x.RemoteEndPoint.Port == remote;
            else if (local > 0) func = x => x.LocalEndPoint.Port == local;
            else if (remote > 0) func = x => x.RemoteEndPoint.Port == remote;
            else throw new ArgumentException($"{nameof(local)} and {nameof(remote)} is invalid.");

            return properties?.GetActiveTcpConnections()?.FirstOrDefault(func);
        }

        /// <summary>
        /// Gets the host addresses.
        /// </summary>
        /// <param name="hostName">Name of the host.</param>
        /// <returns></returns>
        public static IPAddress[] GetHostAddresses(string hostName = null) => Dns.GetHostAddresses(hostName ?? Dns.GetHostName());
    }
}