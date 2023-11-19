using DetravHockey.Server.Packets;
using Godot;
using System;

public partial class StartLobbyMenu : Control
{
    private WSServerClient client;
    private bool requestLobby;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        WSClientSingleton.Instance.IsServer = true;
        var socket = new WebSocketPeer();
        socket.ConnectToUrl("ws://20.203.167.235:9080");
        client = new WSServerClient();
        client.Socket = socket;
        WSClientSingleton.Instance.AddChild(client);
        WSClientSingleton.Instance.Client = client;

        client.OnMessage += Client_OnMessage;
    }

    private void Client_OnMessage(object sender, DetravHockey.Server.Packets.PacketBase e)
    {
        switch (e)
        {
            case SetLobbyPacket setLobbyPacket:
                GetNode<Label>("CenterContainer/VBoxContainer/Label2").Text = setLobbyPacket.Name;
                break;
            case StartGamePacket:

                GetTree().ChangeSceneToFile("res://Scenes/NetworkLevel/NetworkLevel.tscn");
                break;
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
        if (client.Connected && !requestLobby)
        {
            client.SendPacket(new CreateLobbyPacket());
            requestLobby = true;
        }
    }

    public void _on_button_back_pressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/MainMenu/MainMenu.tscn");
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        client.OnMessage -= Client_OnMessage;
    }
}
