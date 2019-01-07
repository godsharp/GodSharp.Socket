using System.Net;
using System.Net.Sockets;

namespace GodSharp.Sockets
{
    public static class SocketExtensions
    {
        /// <summary>
        /// Gets the TCP connection information.
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <returns></returns>
        //public static ConnectionInformation GetConnectionInformation(this Socket socket)
        //{
        //    IPEndPoint remote = socket.RemoteEndPoint as IPEndPoint;
        //    IPEndPoint local = socket.LocalEndPoint as IPEndPoint;

        //    return NetworkHelper.GetTcpConnectionInformation(local?.Port ?? 0, remote?.Port ?? 0) as ConnectionInformation;
        //}
    }
}