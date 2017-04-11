# GodSharp.Socket
A easy to use socket server and client for .NET.

[![AppVeyor build status](https://img.shields.io/appveyor/ci/seayxu/godsharp-socket.svg?label=appveyor&style=flat-square)](https://ci.appveyor.com/project/seayxu/godsharp-socket/) [![NuGet](https://img.shields.io/nuget/v/GodSharp.Socket.svg?label=nuget&style=flat-square)](https://www.nuget.org/packages/GodSharp.Socket/) [![MyGet](https://img.shields.io/myget/seay/v/GodSharp.Socket.svg?label=myget&style=flat-square)](https://www.myget.org/Package/Details/seay?packageType=nuget&packageId=GodSharp.Socket)

# Getting Started

1. Server

```
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
	Console.WriteLine($"server received data from {socket.LocalEndPoint}£º{message}");

	message = "server repley " + message;

	socket.Send(message);

	Console.WriteLine($"server send data to {socket.LocalEndPoint}£º{message}");
    };

    server.Start();

    Console.ReadKey();
}
```

2. Client

```
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
```