namespace GodSharp.Sockets
{
    public delegate void SocketEventHandler<TEventArgs>(TEventArgs e) where TEventArgs : NetEventArgs;
}