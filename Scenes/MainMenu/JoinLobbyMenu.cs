using DetravHockey.Server.Packets;
using Godot;
using System;
using System.Linq;

public partial class JoinLobbyMenu : Control
{
    private WSServerClient client;
    private bool ready = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        WSClientSingleton.Instance.IsServer = false;
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
            case SetLobiesListPacket setLobiesListPacket:
                var list = GetNode<ItemList>("CenterContainer/VBoxContainer/ItemList");
                list.Clear();

                if (setLobiesListPacket.Names != null)
                    foreach (var item in setLobiesListPacket.Names)
                    {
                        list.AddItem(item);
                    }
                break;

            case StartGamePacket:

                GetTree().ChangeSceneToFile("res://Scenes/NetworkLevel/NetworkLevel.tscn");
                break;
        }
    }

    public void _on_button_refresh_pressed()
    {
        client.SendPacket(new GetLobiesListPacket());
    }

    public void _on_button_back_pressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/MainMenu/MainMenu.tscn");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (client.Connected && !ready)
        {
            ready = true;
            _on_button_refresh_pressed();
        }
    }

    public void _on_button_connect_pressed()
    {
        var list = GetNode<ItemList>("CenterContainer/VBoxContainer/ItemList");
        if(list.GetSelectedItems().Length > 0)
        {
            var selectedIndex = list.GetSelectedItems()[0];
            var lobby = list.GetItemText(selectedIndex);

            client.SendPacket(new ConnectToLobbyPacket()
            {
                Name = lobby
            });
        }
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        client.OnMessage -= Client_OnMessage;
    }
}
