using GodSharp.Sockets;
using System;
using System.Linq;

namespace GodSharp.Socket.UdpClientSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("GodSharp.UdpClient!");
            IUdpClient client = new UdpClient("10.0.0.10",10500)
            {
                OnReceived = (c) =>
                {
                    Console.WriteLine($"Received from {c.RemoteEndPoint}:");
                    Console.WriteLine(string.Join(" ", c.Buffers.Select(x => x.ToString("X2")).ToArray()));

                    c.NetConnection.SendTo(c.Buffers, c.RemoteEndPoint);
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

            client.UseKeepAlive(true, 500, 500);
            
            client.Start();
            Console.WriteLine("Started");

            Console.ReadLine();
            client.Connection.SendTo(new byte[] { 0x30, 0x31 }, client.Connection.RemoteEndPoint);

            Console.ReadLine();

            client.Stop();

            Console.WriteLine("Stopped");
            Console.ReadLine();
        }
    }
}
