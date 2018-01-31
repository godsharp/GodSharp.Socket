using GodSharp.Sockets;
using System;

namespace GodSharp.UdpClientSlaveSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start Udp Slave!");

            UdpClient client = new UdpClient("127.0.0.1", 6190)
            {
                OnData = (s, b) =>
                {
                    Console.WriteLine($"received from master {s.RemoteEndPoint.ToString()}");
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
