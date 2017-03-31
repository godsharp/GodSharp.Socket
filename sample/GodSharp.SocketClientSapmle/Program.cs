using System;

namespace GodSharp.SocketClientSapmle
{
    class Program
    {
        static void Main()
        {
            SocketClient client = new SocketClient("127.0.0.1", 7788);

            client.OnData = (socket, data) =>
            {
                //get server data
                string message = client.Encoding.GetString(data, 0, data.Length);
                Console.WriteLine($"client received data from {socket.RemoteEndPoint.ToString()}: {message}");
            };
            client.Connect();

            client.Start();

            string msg = Console.ReadLine();

            while (msg.ToLower() != "q")
            {
                client.Send(msg);
                Console.WriteLine($"client send data to {client.RemoteEndPoint.ToString()}: {msg}");
                msg = Console.ReadLine();
            }
        }
    }
}
