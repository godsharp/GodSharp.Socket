using GodSharp.Sockets.Abstractions;
using GodSharp.Sockets.Extensions;
using System;
using System.Net;
using System.Net.Sockets;

namespace GodSharp.Sockets.Udp
{
    public sealed class UdpConnection : NetConnection<IUdpConnection, NetClientEventArgs<IUdpConnection>>, IUdpConnection, IDisposable
    {
        public override SocketEventHandler<NetClientEventArgs<IUdpConnection>> OnConnected => throw new NotSupportedException();

        public IUdpListener Listener { get; private set; }

        public IPEndPoint ListenEndPoint { get; private set; }

        private bool SpecializedListenEndPoint { get; set; } = false;

        internal UdpConnection(IPEndPoint remote, IPEndPoint local, AddressFamily family = AddressFamily.InterNetwork) => OnConstructing(remote, local, family);
        
        private void OnConstructing(IPEndPoint remote, IPEndPoint local, AddressFamily family = AddressFamily.InterNetwork)
        {
            try
            {
                if (remote == null && local == null) throw new ArgumentNullException($"{nameof(remote)},{nameof(local)}");

                if (remote != null && remote.Port.NotIn(IPEndPoint.MinPort, IPEndPoint.MaxPort)) throw new ArgumentOutOfRangeException(nameof(remote), $"The {nameof(remote)} port must between {IPEndPoint.MinPort} to {IPEndPoint.MaxPort}.");
                if (local != null && local.Port.NotIn(IPEndPoint.MinPort, IPEndPoint.MaxPort)) throw new ArgumentOutOfRangeException(nameof(local), $"The {nameof(remote)} port must between {IPEndPoint.MinPort} to {IPEndPoint.MaxPort}.");

                if (remote != null && local != null)
                {
                    if (remote.Port < 1 && local.Port < 1) throw new ArgumentNullException($"{nameof(remote.Port)}, {nameof(local.Port)}");
                }
                else
                {
                    if (remote != null && remote.Port < 1) throw new ArgumentOutOfRangeException(nameof(remote.Port));
                    if (local != null && local.Port < 1) throw new ArgumentOutOfRangeException(nameof(local.Port));
                }

                switch (family)
                {
                    case AddressFamily.InterNetwork:
                    case AddressFamily.InterNetworkV6:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(family), $"The AddressFamily only support AddressFamily.InterNetwork and AddressFamily.InterNetworkV6.");
                }

                if (remote != null && remote.AddressFamily != family) throw new ArgumentException($"The {nameof(remote)} and {nameof(family)} not match.");
                if (local != null && local.AddressFamily != family) throw new ArgumentException($"The {nameof(local)} and {nameof(family)} not match.");

                SpecializedListenEndPoint = remote != null && remote.Port > 0;

                ListenEndPoint = SpecializedListenEndPoint ? remote : new IPEndPoint(family == AddressFamily.InterNetworkV6 ? IPAddress.IPv6Any : IPAddress.Any, 0);
                

                Instance = new Socket(family, SocketType.Dgram, ProtocolType.Udp);
                if (local != null && local.Port > 0) Instance.Bind(local);

                LocalEndPoint = local;
                RemoteEndPoint = ListenEndPoint;

                this.Key = local.ToString();
                this.Name = this.Name ?? this.Key;
            }
            catch (Exception ex)
            {
                OnException?.Invoke(new NetClientEventArgs<IUdpConnection>(this) { Exception = ex });
                throw ex;
            }
        }

        public override void Start()
        {
            try
            {
                if (Listener?.Running == true) return;

                Listener = new UdpListener(this);
                Listener.Start();
                OnStarted?.Invoke(new NetClientEventArgs<IUdpConnection>(this));
            }
            catch (Exception ex)
            {
                OnException?.Invoke(new NetClientEventArgs<IUdpConnection>(this) { Exception = ex });
            }
        }

        public override void Stop()
        {
            if (Listener == null) return;
            if (!Listener.Running) return;

            try
            {
                Listener?.Stop();
                OnStopped?.Invoke(new NetClientEventArgs<IUdpConnection>(this));
            }
            catch (Exception ex)
            {
                OnException?.Invoke(new NetClientEventArgs<IUdpConnection>(this) { Exception = ex });
            }
        }

        public override void Dispose() => Listener?.Dispose();
    }
}
