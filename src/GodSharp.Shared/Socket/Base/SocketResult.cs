using System.Net.Sockets;

namespace GodSharp.Protocol
{
    public class SocketResult
    {
        public Socket Socket { get; set; }
        public byte[] Bytes { get; set; }
    }
}
