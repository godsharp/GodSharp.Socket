namespace GodSharp.Sockets
{
    public class KeepAliveOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="KeepAliveOptions"/> is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        public uint Time { get; set; } = 5000;

        /// <summary>
        /// Gets or sets the interval.
        /// </summary>
        /// <value>
        /// The interval.
        /// </value>
        public uint Interval { get; set; } = 5000;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeepAliveOptions"/> class.
        /// </summary>
        public KeepAliveOptions()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeepAliveOptions"/> class.
        /// </summary>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        /// <param name="time">The time.</param>
        /// <param name="interval">The interval.</param>
        public KeepAliveOptions(bool enabled = true, uint time = 5000, uint interval = 5000)
        {
            Enabled = enabled;
            Time = time;
            Interval = interval;
        }
    }
}
