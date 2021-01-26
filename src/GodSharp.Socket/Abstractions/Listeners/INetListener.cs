using System;

namespace GodSharp.Sockets.Abstractions
{
    public interface INetListener<TConnection> : IDisposable where TConnection : INetConnection
    {
        TConnection Connection { get; }

        bool Running { get; }

        void Start();

        void Stop();

        void BeginReceive();

        void ReceivedCallback(IAsyncResult result);

        //void KeepAlive(bool keepAlive = true, int interval = 5000, int span = 1000);
    }
}
