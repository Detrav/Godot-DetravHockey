using DetravHockey.Server;
using DetravHockey.Server.Packets;
using Godot;
using System;
using System.Linq;
using System.Text;
using System.Text.Json;

public partial class WSServerClient : Node
{
    private static JsonSerializerOptions options = new JsonSerializerOptions()
    {
        TypeInfoResolver = new MyCustomResolver()
    };
    private WSServer server;

    public event EventHandler<PacketBase> OnMessage;
    public WebSocketPeer Socket { get; internal set; }
    public bool Connected { get; private set; }
    public Lobby Lobby { get; set; }



    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        server = GetParent() as WSServer;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        Socket.Poll();
        switch (Socket.GetReadyState())
        {
            case WebSocketPeer.State.Connecting:
                break;
            case WebSocketPeer.State.Open:
                Connected = true;


                while (Socket.GetAvailablePacketCount() > 0)
                {
                    var packet = JsonSerializer.Deserialize<PacketBase>(Encoding.UTF8.GetString(Socket.GetPacket()), options);
                    Process(packet);
                }
                break;
            case WebSocketPeer.State.Closing:
                Connected = false;
                QueueFree();
                break;
            case WebSocketPeer.State.Closed:
                QueueFree();
                Connected = false;
                break;
        }
    }

    public override void _ExitTree()
    {
        if (Lobby != null)
        {
            if (server.Lobbies.Remove(Lobby.Name))
            {
                foreach (var player in Lobby.Players)
                {
                    player.QueueFree();
                }
            }

        }
        Connected = false;
        Socket.Close();
    }

    public void SendPacket(PacketBase packet)
    {
        var json = JsonSerializer.Serialize(packet, options);
        var bytes = Encoding.UTF8.GetBytes(json);
        Socket.Send(bytes);
    }

    public void Process(PacketBase packet)
    {

        OnMessage?.Invoke(this, packet);

    }
}
