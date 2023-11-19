using Godot;
using System;
using System.Net.Sockets;

public partial class WSServerClient : Node
{
    public StreamPeerTcp Connection { get; internal set; }
    public WebSocketPeer Socket { get; internal set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        switch (Socket.GetReadyState())
        {
            case WebSocketPeer.State.Open:
                Socket.Poll();
                break;
            case WebSocketPeer.State.Connecting:
                break;
            case WebSocketPeer.State.Closing:
                QueueFree();
                break;
            case WebSocketPeer.State.Closed:
                QueueFree();
                break;
        }
	}

    public override void _ExitTree()
    {
        Socket.Close();
    }
}
