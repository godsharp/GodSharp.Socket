using GodSharp.Sockets;
using System;

namespace GodSharp.SocketClientSapmle
{
    class Program
    {
        static void Main()
        {
            Console.ReadKey();

            SocketClient client = new SocketClient("127.0.0.1", 7788);

            client.OnData = (sender, data) =>
            {
                //get server data
                string message = client.Encoding.GetString(data, 0, data.Length);
                Console.WriteLine($"client received data from {sender.RemoteEndPoint.ToString()}: {message}");
            };

            client.Connect();

            client.Start();

            string msg = Console.ReadLine();

            while (msg.ToLower() != "q")
            {
                client.Sender.Send(msg);
                Console.WriteLine($"client send data to {client.RemoteEndPoint.ToString()}: {msg}");
                msg = Console.ReadLine();
            }
        }
    }
}
