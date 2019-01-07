using GodSharp.Sockets;
using System;
using System.Linq;

namespace GodSharp.Socket.TcpClientSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("GodSharp.TcpClient!");
            TcpClient client = new TcpClient("127.0.0.1", 4001)
            {
                OnConnected = (c) =>
                {
                    Console.WriteLine($"{c.RemoteEndPoint} connected.");
                },
                OnReceived = (c) =>
                {
                    Console.WriteLine($"Received from {c.RemoteEndPoint}:");
                    Console.WriteLine(string.Join(" ", c.Buffers.Select(x => x.ToString("X2")).ToArray()));

                    c.NetConnection.Send(c.Buffers);
                },
                OnDisconnected = (c) =>
                {
                    Console.WriteLine($"{c.RemoteEndPoint} disconnected.");
                },
                OnStarted = (c) =>
                {
                    Console.WriteLine($"{c.RemoteEndPoint} started.");
                },
                OnStopped = (c) =>
                {
                    Console.WriteLine($"{c.RemoteEndPoint} stopped.");
                },
                OnException = (c) =>
                {
                    Console.WriteLine($"{c.RemoteEndPoint} exception:{c.Exception.StackTrace.ToString()}.");
                }
            };

            client.Start();
            Console.WriteLine("Started");
            
            Console.ReadLine();
            
            client.Stop();

            Console.WriteLine("Stopped");
            Console.ReadLine();
        }
    }
}
