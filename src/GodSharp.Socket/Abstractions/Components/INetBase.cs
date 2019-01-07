using System;

namespace GodSharp.Sockets.Abstractions
{
    public interface INetBase<TConnection> : IDisposable where TConnection : INetConnection
    {
        int Id { get; }

        string Name { get; }

        string Key { get; }

        bool Running { get; }

        void Start();

        void Stop();
    }
}
