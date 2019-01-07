using System;
using System.Net;

namespace GodSharp.Sockets
{
    public static class EndPointExtensions
    {
        /// <summary>
        /// Ases the specified end point.
        /// </summary>
        /// <param name="endPoint">The end point.</param>
        /// <returns></returns>
        public static IPEndPoint As(this EndPoint endPoint) => endPoint as IPEndPoint;

        /// <summary>
        /// Ases the specified end point.
        /// </summary>
        /// <param name="endPoint">The end point.</param>
        /// <returns></returns>
        public static EndPoint As(this IPEndPoint endPoint) => endPoint as EndPoint;
    }
}
