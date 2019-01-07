namespace GodSharp.Sockets.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Determines whether [is null or white space].
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>
        ///   <c>true</c> if [is null or white space] [the specified string]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrWhiteSpace(this string str)
        {
            if (str == null) return true;
#if NET35
            return string.IsNullOrEmpty(str) || str.Trim() == "";
#else
            return string.IsNullOrWhiteSpace(str);
#endif
        }
    }
}
