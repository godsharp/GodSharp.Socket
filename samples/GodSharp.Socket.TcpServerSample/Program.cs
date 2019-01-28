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

            ITcpServer server = new TcpServer()
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
                },
                OnServerException= (c) =>
                {
                    Console.WriteLine($"{c.LocalEndPoint} exception:{c.Exception.StackTrace.ToString()}.");
                }
            };
            
            server.Start();

            Console.WriteLine("GodSharp.Socket.TcpServer Started!");

            try
            {
                while (true)
                {
                    Console.ReadLine();
                    Check(server);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                server.Stop();
                Console.WriteLine("GodSharp.Socket.TcpServer Stopped!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();
        }

        private static void Check(ITcpServer server)
        {
            try
            {
                foreach (var item in server.Connections)
                {
                    int ret = -1;
                    try
                    {
                        ret = item.Value.Send(new byte[0]);
                    }
                    catch (Exception ex)
                    {
                        ret = -2;
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        Console.WriteLine($"{item.Key}:{ret}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
