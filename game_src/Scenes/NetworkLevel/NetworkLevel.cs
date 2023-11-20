using DetravHockey.Server.Packets;
using Godot;
using Godot.Collections;
using System;

public partial class NetworkLevel : BaseLevel
{
    private Ball ball;
    
    private PlayerBase localPlayer;
    private PlayerBase remotePlayer;

    public override void _Ready()
    {
        
        base._Ready();
        ball = GetNode<Ball>("Ball");
        ball.OnBounce += Ball_OnBounce;
        WSClientSingleton.Instance.Client.OnMessage += Client_OnMessage;

        localPlayer = GetNode<PlayerBase>("MousePlayer");
        remotePlayer = GetNode<PlayerBase>("NetworkPlayer");
    }

    private void Ball_OnBounce(object sender, GodotObject e)
    {
        
            
        if(ball.Position.Y > 0 || Math.Abs(ball.Position.Y) < 20)
        {
            GD.Print(ball.Position);

            WSClientSingleton.Instance.Client.SendPacket(new BallPositionPacket()
            {
                X = ball.Position.X,
                Y = ball.Position.Y,
                DX = ball.Direction.X,
                DY = ball.Direction.Y,
                S = ball.Speed
            });
        }
    }

    protected override void NewLevel(Ball ball)
    {
        //if (WSClientSingleton.Instance.IsServer)
        //{
        //    WSClientSingleton.Instance.Client.SendPacket(new BallPositionPacket()
        //    {
        //        X = ball.Position.X,
        //        Y = ball.Position.Y,
        //        DX = ball.Direction.X,
        //        DY = ball.Direction.Y,
        //        S = ball.Speed
        //    });
        //}
        base.NewLevel(ball);
    }

    private void Client_OnMessage(object sender, DetravHockey.Server.Packets.PacketBase e)
    {
        switch (e)
        {
            case PlayerPositionPacket playerPositionPacket:
                remotePlayer.TargetPosition = -new Vector2(playerPositionPacket.X, playerPositionPacket.Y);
                //remotePlayer.CurrentSpeed = -new Vector2(playerPositionPacket.DX, playerPositionPacket.DY);
                break;

            case BallPositionPacket ballPositionPacket:
                ball.Position = -new Vector2(ballPositionPacket.X, ballPositionPacket.Y);
                ball.Direction = -new Vector2(ballPositionPacket.DX, ballPositionPacket.DY);
                ball.Speed = ballPositionPacket.S;
                break;
        }
    }



    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
       

        var mouse = GetGlobalMousePosition();

        WSClientSingleton.Instance.Client.SendPacket(new PlayerPositionPacket()
        {
            X = mouse.X,
            Y = mouse.Y,
            DX = localPlayer.CurrentSpeed.X,
            DY = localPlayer.CurrentSpeed.Y,

        });

    }
}
