using GodSharp.Sockets;
using GodSharp.Sockets.Tcp;

using System;
using System.Linq;
using System.Net;

namespace GodSharp.Socket.TcpClientSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("GodSharp.TcpClient!");
            TcpClient client = new TcpClient("192.168.8.131", 5000,10002)
            {
                //ConnectTimeout = 2000,
                //ReconnectEnable = true,
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
                    Console.WriteLine($"{c.RemoteEndPoint} exception:Message:{c.Exception.Message},StackTrace:{c.Exception.StackTrace.ToString()}.");
                },
                OnTryConnecting = (c) =>
                {
                    Console.WriteLine($"try connect {c.Counter} ...");
                }
            };

            // set keep alive
            client.UseKeepAlive(true, 500, 500);

            // tr connnect stratagy
            // client.UseTryConnectionStrategy(new DefaultTryConnectionStrategy());

            try
            {
                client.Start();

                Console.WriteLine("Started");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message:{ex.Message},StackTrace:{ex.StackTrace.ToString()}");
            }
            
            Console.ReadLine();
            
            client.Stop();

            Console.WriteLine("Stopped");
            Console.ReadLine();
        }
    }
}
