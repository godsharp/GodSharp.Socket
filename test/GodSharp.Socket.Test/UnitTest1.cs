using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GodSharp.Sockets.Internal.Util;

namespace GodSharp.Socket.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Md5ByteTestMethod()
        {
            string str = "127.0.0.1:1203";
            byte[] gb = Utils.Md5(str);
            Console.WriteLine(gb.Length);
            Guid guid = new Guid(gb);
            Console.WriteLine(guid.ToString());
        }
    }
}
