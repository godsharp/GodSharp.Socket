using System;

namespace GodSharp.SocketServerSample
{
    class Program
    {
        static void Main()
        {
            SocketServer server = new SocketServer(7788)
            {
                OnOpen = (socket) =>
                {
                    Console.WriteLine($"Client {socket.RemoteEndPoint.ToString()} connecte {socket.Connected}");
                }
            };

            server.OnData = (socket, data) =>
            {
                //get client data
                string message = server.Encoding.GetString(data, 0, data.Length);
                Console.WriteLine($"server received data from {socket.LocalEndPoint}：{message}");

                message = "server repley " + message;

                socket.Send(message);

                Console.WriteLine($"server send data to {socket.LocalEndPoint}：{message}");

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

            server.Start();

            Console.ReadKey();
        }
    }
}
