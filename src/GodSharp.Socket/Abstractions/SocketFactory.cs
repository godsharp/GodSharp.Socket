using System.Net;
using System.Net.Sockets;

namespace GodSharp.Sockets
{
    public class SocketFactory : ISocketFactory
    {
        /// <summary>
        /// Creates the TCP client.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public ITcpClient CreateTcpClient(TcpClientOptions options) => new TcpClient(options);

        /// <summary>
        /// Creates the TCP client.
        /// </summary>
        /// <param name="remoteHost">The remote host.</param>
        /// <param name="remotePort">The remote port.</param>
        /// <param name="localPort">The local port.</param>
        /// <param name="localHost">The local host.</param>
        /// <param name="connectTimeout">The connect timeout.</param>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ITcpClient CreateTcpClient(string remoteHost, int remotePort, int localPort = 0, string localHost = null, int connectTimeout = 3000, string name = null, int id = 0) => new TcpClient(remoteHost, remotePort, localPort, localHost, connectTimeout, name, id);

        /// <summary>
        /// Creates the TCP server.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public ITcpServer CreateTcpServer(TcpServerOptions options) => new TcpServer(options);

        /// <summary>
        /// Creates the TCP server.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <param name="host">The host.</param>
        /// <param name="backlog">The backlog.</param>
        /// <param name="family">The family.</param>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ITcpServer CreateTcpServer(int port = 7788, string host = null, int backlog = int.MaxValue, AddressFamily family = AddressFamily.InterNetwork, string name = null, int id = 0) => new TcpServer(port, host, backlog, family, name, id);

        /// <summary>
        /// Creates the UDP client.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public IUdpClient CreateUdpClient(UdpClientOptions options) => new UdpClient(options);

        /// <summary>
        /// Creates the UDP client.
        /// </summary>
        /// <param name="remote">The remote.</param>
        /// <param name="local">The local.</param>
        /// <param name="family">The family.</param>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public IUdpClient CreateUdpClient(IPEndPoint remote, IPEndPoint local, AddressFamily family = AddressFamily.InterNetwork, string name = null, int id = 0) => new UdpClient(remote, local, family, name, id);

        /// <summary>
        /// Creates the UDP client.
        /// </summary>
        /// <param name="remoteHost">The remote host.</param>
        /// <param name="remotePort">The remote port.</param>
        /// <param name="localPort">The local port.</param>
        /// <param name="localHost">The local host.</param>
        /// <param name="family">The family.</param>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public IUdpClient CreateUdpClient(string remoteHost, int remotePort, int localPort = 0, string localHost = null, AddressFamily family = AddressFamily.InterNetwork, string name = null, int id = 0) => new UdpClient(remoteHost, remotePort, localPort, localHost, family, name, id);
    }
}
