namespace GodSharp.Sockets.Abstractions
{
    public interface ITryConnectionStrategy
    {
        int Handle(int counter);
    }
}
