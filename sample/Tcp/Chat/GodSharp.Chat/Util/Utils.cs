using System;
using System.Linq;
using System.Text;

namespace GodSharp.Chat.Util
{
    public class Utils
    {
        /// <summary>
        /// MD5s the specified string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="bits">The bits.</param>
        /// <param name="lower">if set to <c>true</c> [lower].</param>
        /// <returns></returns>
        public static string Md5(string str, int bits = 32, bool lower = true)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return null;
            }

            string result = null;
            dynamic md5;
            switch (bits)
            {
                case 16:
                    md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                    string t2 = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(str)), 4, 8);
                    result = t2.Replace("-", "");
                    break;
                default:

                    md5 = System.Security.Cryptography.MD5.Create();

                    byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                    
                    result = s.Aggregate(result, (current, t) => current + t.ToString("X2"));

                    break;
            }

            if (result != null)
            {
                result = lower ? result.ToLower() : result.ToUpper();
            }

            return result;
        }
    }
}
