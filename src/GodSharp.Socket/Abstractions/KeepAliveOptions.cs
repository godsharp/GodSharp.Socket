namespace GodSharp.Sockets
{
    /// <summary>
    /// Keep alive options
    /// </summary>
    public class KeepAliveOptions
    {
        /// <summary>
        /// Whether keep alive
        /// </summary>
        public bool KeepAlive { get; set; } = true;

        /// <summary>
        /// The value of interval to check connection.
        /// </summary>
        public int Interval { get; set; } = 5000;

        /// <summary>
        /// The value of span to check connection.
        /// </summary>
        public int Span { get; set; } = 1000;

        /// <summary>
        /// Keep alive options
        /// </summary>
        /// <param name="keepAlive">Whether keep alive</param>
        /// <param name="interval">The value of interval to check connection.</param>
        /// <param name="span">The value of span to check connection.</param>
        public KeepAliveOptions(bool keepAlive = true, int interval = 5000, int span = 1000)
        {
            KeepAlive = keepAlive;
            Interval = interval;
            Span = span;
        }
    }
}
