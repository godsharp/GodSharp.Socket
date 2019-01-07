using GodSharp.Sockets.Abstractions;
using System;
using System.Net;
using System.Net.Sockets;

namespace GodSharp.Sockets.Udp
{
    internal sealed class UdpListener : NetListener<IUdpConnection>, INetListener<IUdpConnection>, IUdpListener
    {
        public UdpListener(IUdpConnection connection) : base(connection)
        {
        }

        protected override void OnBeginReceive(ref byte[] buffers)
        {
            EndPoint point = Connection.ListenEndPoint.As();

            Connection.Instance.BeginReceiveFrom(buffers, 0, buffers.Length, SocketFlags.None, ref point, ReceivedCallback, point);
            //Connection.Instance.BeginReceive(buffers, 0, buffers.Length, SocketFlags.None, ReceivedCallback, point);
        }
        
        protected override T OnEndReceive<T>(IAsyncResult result)
        {
            EndPoint point = result.AsyncState as EndPoint;

            return new ReceiveResult(Connection.Instance.EndReceiveFrom(result, ref point), point.As()) as T;
            //return new ReceiveResult(Connection.Instance.EndReceive(result), point.As()) as T;
        }

        protected override void OnReceiveHandling(byte[] buffers, IPEndPoint remote = null, IPEndPoint local = null) => Connection.OnReceived?.Invoke(new NetClientReceivedEventArgs<IUdpConnection>(Connection, buffers, remote, local));

        protected override void OnStop(Exception exception) => Connection.OnDisconnected?.Invoke(new NetClientEventArgs<IUdpConnection>(Connection) { Exception = exception });

        protected override void OnException(Exception exception) => Connection.OnException?.Invoke(new NetClientEventArgs<IUdpConnection>(Connection) { Exception = exception });

        public override void Dispose()
        {
            Connection = null;
        }
    }
}
