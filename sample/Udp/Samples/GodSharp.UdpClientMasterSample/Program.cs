using GodSharp.Sockets;
using System;

namespace GodSharp.UdpClientMasterSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start Udp Master!");

            UdpClient client = new UdpClient(6190)
            {
                OnData = (s, b) =>
                {
                    Console.WriteLine($"received from slave {s.RemoteEndPoint.ToString()}");
                    Console.WriteLine(s.Encoding.GetString(b));
                }
            };

            client.Start();

            string str = null;

            while (str?.ToLower() != "q")
            {
                str = Console.ReadLine();

                client.Sender.Send(str);
            }

            client.Stop();
        }
    }
}
