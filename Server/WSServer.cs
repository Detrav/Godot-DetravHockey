using DetravHockey.Server;
using DetravHockey.Server.Packets;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

public partial class WSServer : Node
{
    public Dictionary<string, Lobby> Lobbies { get; } = new Dictionary<string, Lobby>();

    const int PORT = 9080;
    TcpServer tcp_server = new TcpServer();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (tcp_server.Listen(PORT) != Error.Ok)
        {
            SetProcess(false);
            LogMessage("Unable to start server.");
        }

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        while (tcp_server.IsConnectionAvailable())
        {
            var conn = tcp_server.TakeConnection();
            if (conn != null)
            {
                var socket = new WebSocketPeer();
                socket.AcceptStream(conn);
                WSServerClient client = new WSServerClient();
                // FUCK the leak
                client.OnMessage += Client_OnMessage;
                //client.Connection = conn;
                client.Socket = socket;
                AddChild(client);
            }
        }
    }

    private void Client_OnMessage(object sender, DetravHockey.Server.Packets.PacketBase packet)
    {
        var client = sender as WSServerClient;
        switch (packet)
        {
            case PingPacket pingPacket:
                client.SendPacket(new PongPacket() { Time = pingPacket.Time });
                break;
            case CreateLobbyPacket:

                client.Lobby = new Lobby();

                client.Lobby.Name = (Lobby.LastId++).ToString();
                client.Lobby.Players.Add(client);
                Lobbies[client.Lobby.Name] = client.Lobby;
                client.SendPacket(new SetLobbyPacket()
                {
                    Name = client.Lobby.Name,
                });
                break;
            case GetLobiesListPacket:
                client.SendPacket(new SetLobiesListPacket()
                {
                    Names = Lobbies.Keys.ToArray()
                });
                break;

            case ConnectToLobbyPacket connectToLobbyPacket:
                if (Lobbies.TryGetValue(connectToLobbyPacket.Name, out var lobby))
                {
                    client.Lobby = lobby;
                    client.Lobby.Players.Add(client);

                    foreach (var player in client.Lobby.Players)
                    {
                        player.SendPacket(new StartGamePacket());
                    }
                }
                break;

            case BallPositionPacket:
            case PlayerPositionPacket:

                if (client.Lobby != null)
                {
                    foreach (var player in client.Lobby.Players)
                    {
                        if (player != client)
                        {
                            player.SendPacket(packet);
                        }
                    }
                }

                break;
        }
    }

    public static void LogMessage(string message)
    {
        GD.Print(Time.GetTimeStringFromSystem() + ": " + message);
    }
}
