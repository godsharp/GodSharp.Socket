using GodSharp.Sockets;
using System;

namespace GodSharp.SocketServerSample
{
    class Program
    {
        static void Main()
        {
            Random random = new Random();
            SocketServer server = new SocketServer()
            {
                OnConnected = (sender) =>
                {
                    Console.WriteLine($"Client {sender.RemoteEndPoint.ToString()} connected");
                }
            };

            server.OnData = (sender, data) =>
            {
                //get client data
                string message = server.Encoding.GetString(data, 0, data.Length);
                Console.WriteLine($"server received data from {sender.RemoteEndPoint}：{message}");

                //message = "server repley " + message;
                message = random.Next(100000000, 999999999).ToString();
                sender.Send(message);

                Console.WriteLine($"server send data to {sender.RemoteEndPoint}：{message}");
            };

            server.Listen();
            server.Start();

            Console.ReadKey();
        }
    }
}
