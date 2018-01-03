# GodSharp.Socket

[![license][li]][l] [![GitHub code size in bytes][si]][0]

A easy to use socket server and client for .NET.

|Branch|Status|
|---|---|
|master|[![Build status](https://ci.appveyor.com/api/projects/status/xfg3uf232fdrgtib/branch/master?svg=true&style=flat-square)](https://ci.appveyor.com/project/seayxu/godsharp-socket/branch/master)|
|dev|[![Build status](https://ci.appveyor.com/api/projects/status/xfg3uf232fdrgtib/branch/dev?svg=true)](https://ci.appveyor.com/project/seayxu/godsharp-socket/branch/dev)|
|release|[![Build status](https://ci.appveyor.com/api/projects/status/xfg3uf232fdrgtib/branch/release?svg=true)](https://ci.appveyor.com/project/seayxu/godsharp-socket/branch/release)|

|Name|Stable|Preview|
|---|:---:|:---:|
|GodSharp.Socket| [![MyGet][mi1]][m1] [![NuGet][ni1]][n1] | [![MyGet][mi2]][m1] [![NuGet][ni2]][n1] |

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
        Console.WriteLine($"server received data from {sender.RemoteEndPoint}: {message}");

        //message = "server repley " + message;
        message = random.Next(100000000, 999999999).ToString();
        sender.Send(message);

        Console.WriteLine($"server send data to {sender.RemoteEndPoint}: {message}");
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


[0]: https://github.com/godsharp/GodSharp.Socket
[si]: https://img.shields.io/github/languages/code-size/godsharp/GodSharp.Socket.svg?style=flat-square

[li]: https://img.shields.io/badge/license-MIT-blue.svg?label=license&style=flat-square
[l]: https://github.com/godsharp/GodSharp.Socket/blob/master/LICENSE

[m1]: https://www.myget.org/Package/Details/godsharp?packageType=nuget&packageId=GodSharp.Socket

[mi1]: https://img.shields.io/myget/godsharp/v/GodSharp.Socket.svg?label=myget&style=flat-square
[mi2]: https://img.shields.io/myget/godsharp/vpre/GodSharp.Socket.svg?label=myget&style=flat-square

[n1]: https://www.nuget.org/packages/GodSharp.Socket

[ni1]: https://img.shields.io/nuget/v/GodSharp.Socket.svg?label=nuget&style=flat-square
[ni2]: https://img.shields.io/nuget/vpre/GodSharp.Socket.svg?label=nuget&style=flat-square