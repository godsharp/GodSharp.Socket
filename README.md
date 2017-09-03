# GodSharp.Socket
A easy to use socket server and client for .NET.

[![AppVeyor build status](https://img.shields.io/appveyor/ci/seayxu/godsharp-socket.svg?label=appveyor&style=flat-square)](https://ci.appveyor.com/project/seayxu/godsharp-socket/) [![NuGet](https://img.shields.io/nuget/v/GodSharp.Socket.svg?label=nuget&style=flat-square)](https://www.nuget.org/packages/GodSharp.Socket/) [![MyGet](https://img.shields.io/myget/seay/v/GodSharp.Socket.svg?label=myget&style=flat-square)](https://www.myget.org/Package/Details/seay?packageType=nuget&packageId=GodSharp.Socket)

# Getting Started

1. Server

```
static void Main()
{
    Random random = new Random();
    SocketServer server = new SocketServer(port:7788)
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
        Console.WriteLine($"server received data from {sender.RemoteEndPoint}£º{message}");

        //message = "server repley " + message;
        message = random.Next(100000000, 999999999).ToString();
        sender.Send(message);

        Console.WriteLine($"server send data to {sender.RemoteEndPoint}£º{message}");
    };

    server.Listen();
    server.Start();

    Console.ReadKey();
}
```

2. Client

```
static void Main()
{
    Console.ReadKey();

    SocketClient client = new SocketClient("127.0.0.1", 7788);

    client.OnData = (sender, data) =>
    {
        //get server data
        string message = client.Encoding.GetString(data, 0, data.Length);
        Console.WriteLine($"client received data from {sender.RemoteEndPoint.ToString()}: {message}");
    };

    client.Connect();

    client.Start();

    string msg = Console.ReadLine();

    while (msg.ToLower() != "q")
    {
        client.Sender.Send(msg);
        Console.WriteLine($"client send data to {client.RemoteEndPoint.ToString()}: {msg}");
        msg = Console.ReadLine();
    }
}
```

# Todo
support async