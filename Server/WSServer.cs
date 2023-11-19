using Godot;
using System;

public partial class WSServer : Node
{
    const int PORT = 9080;
    TcpServer tcp_server = new TcpServer();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (tcp_server.Listen(PORT) != Error.Ok)
        {
            SetProcess(false);
            LogMessage("Unable to start server.")
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

            }
        }
    }

    public static void LogMessage(string message)
    {
        GD.Print(Time.GetTimeStringFromSystem() + ": " + message);
    }
}
