using GodSharp.Sockets;
using System;
using System.Threading;

namespace GodSharp.SocketServerSample
{
    class Program
    {
        static void Main()
        {
            Random random = new Random();
            SocketServer server = new SocketServer(port:7788)
            {
                OnConnected = (socket) =>
                {
                    Console.WriteLine($"Client {socket.RemoteEndPoint.ToString()} connected");
                }
            };

            server.OnData = (socket, data) =>
            {
                //get client data
                string message = server.Encoding.GetString(data, 0, data.Length);
                Console.WriteLine($"server received data from {socket.RemoteEndPoint}：{message}");

                //message = "server repley " + message;
                message = random.Next(100000000, 999999999).ToString();
                socket.Send(message);

                Console.WriteLine($"server send data to {socket.RemoteEndPoint}：{message}");

                //using (NetworkStream ns = new NetworkStream(client))
                //{
                //    if (!ns.DataAvailable)
                //    {
                //        continue;
                //    }
                //    using (StreamReader sr = new StreamReader(ns))
                //    {
                //        if (sr.Peek() < 0)
                //        {
                //            continue;
                //        }
                //        var recStr = sr.ReadToEnd();
                //        Console.WriteLine("服务端接收数据：" + recStr);
                //        using (StreamWriter sw = new StreamWriter(ns))
                //        {
                //            var sd = "server repley " + recStr;
                //            sw.Write(sd);
                //            sw.Flush();
                //            //socket.Send(sendData);
                //            Console.WriteLine("服务端发送数据：" + sd);
                //        }
                //    }
                //}
            };

            server.Listen();
            server.Start();

            //while (true)
            //{
            //    if (server.clients?.Count>0)
            //    {
            //        string data = random.Next(100000000, 999999999).ToString();

            //        foreach (var item in server.clients)
            //        {
            //            item.Value.Send(data);
            //        }
            //    }

            //    Thread.Sleep(1000);
            //}

            Console.ReadKey();
        }
    }
}
