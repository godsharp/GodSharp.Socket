using System;

namespace GodSharp.Sockets.Extensions
{
    public static class IComparableExtensions
    {
        /// <summary>
        /// Ins the specified minimum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns></returns>
        public static bool In<T>(this T value, T min, T max) where T : struct, IComparable<T> => !NotIn(value, min, max);

        /// <summary>
        /// Nots the in.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns></returns>
        public static bool NotIn<T>(this T value, T min, T max) where T : struct, IComparable<T> => (value.CompareTo(min) < 0 || value.CompareTo(max) > 0);
    }
}
