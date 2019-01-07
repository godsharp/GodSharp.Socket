using GodSharp.Sockets;
using System;
using System.Linq;

namespace GodSharp.Socket.TcpServerSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello GodSharp.Socket.TcpServerSample!");

            TcpServer server = new TcpServer()
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
                    Console.WriteLine($"{c.LocalEndPoint} started.");
                },
                OnStopped = (c) =>
                {
                    Console.WriteLine($"{c.LocalEndPoint} stopped.");
                },
                OnException = (c) =>
                {
                    Console.WriteLine($"{c.RemoteEndPoint} exception:{c.Exception.StackTrace.ToString()}.");
                }
            };

            server.Start();

            Console.WriteLine("GodSharp.Socket.TcpServer Started!");

            Console.ReadLine();

            server.Stop();
            Console.WriteLine("GodSharp.Socket.TcpServer Stopped!");
        }
    }
}
