using GodSharp.Sockets.Abstractions;

namespace GodSharp.Sockets.Tcp
{
    public class DefaultTryConnectionStrategy : ITryConnectionStrategy
    {
        public int Handle(int counter)
        {
            return (counter % 20) * 3000;
        }
    }
}