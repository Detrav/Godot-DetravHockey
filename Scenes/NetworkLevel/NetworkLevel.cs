using Godot;
using Godot.Collections;
using System;

public partial class NetworkLevel : BaseLevel
{
    private Ball ball;
    private Node rtc;
    private bool isServer;
    private PlayerBase localPlayer;
    private PlayerBase remotePlayer;

    public override void _Ready()
    {
        rtc = GetNode<Node>("/root/WebRtcSingleton");
        base._Ready();
        ball = GetNode<Ball>("Ball");
        rtc.Connect("package_received", Callable.From<string>(PackageReceived));
        isServer = rtc.Get("b_is_server").AsBool();

        localPlayer = GetNode<PlayerBase>("MousePlayer");
        remotePlayer = GetNode<PlayerBase>("NetworkPlayer");
    }

    public void PackageReceived(string package)
    {
        var dict = Json.ParseString(package).AsGodotDictionary();
        //GD.Print(dict);
        if (dict["id"].AsString() == "ball")
        {
            ball.Position = -new Vector2(dict["X"].AsSingle(), dict["Y"].AsSingle());
            ball.Direction = -new Vector2(dict["DX"].AsSingle(), dict["DY"].AsSingle());
            ball.Speed = dict["S"].AsSingle();
        }
        else if (dict["id"].AsString() == "player")
        {
            remotePlayer.Position = - new Vector2(dict["X"].AsSingle(), dict["Y"].AsSingle());
            remotePlayer.CurrentSpeed = - new Vector2(dict["DX"].AsSingle(), dict["DY"].AsSingle());
        }
    }

    public void SendPackage(string package)
    {
        rtc.Call("send_package", package);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (isServer)
        {
            Dictionary ballDictonary = new Dictionary()
            {
                ["id"] = "ball",
                ["X"] = ball.Position.X,
                ["Y"] = ball.Position.Y,
                ["DX"] = ball.Direction.X,
                ["DY"] = ball.Direction.Y,
                ["S"] = ball.Speed
            };

            SendPackage(Json.Stringify(ballDictonary));
        }

        Dictionary playerDictionary = new Dictionary()
        {
            ["id"] = "player",
            ["X"] = localPlayer.Position.X,
            ["Y"] = localPlayer.Position.Y,
            ["DX"] = localPlayer.CurrentSpeed.X,
            ["DY"] = localPlayer.CurrentSpeed.Y,
        };

        SendPackage(Json.Stringify(playerDictionary));

    }
}
