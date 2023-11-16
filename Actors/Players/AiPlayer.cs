using DetravHockey.Actors.Players;
using Godot;
using System;

public partial class AiPlayer : PlayerBase
{

    private Vector2 startPosition;


    private AI_Player_State attackState = new AI_Player_State(1, 0.5, 1);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        startPosition = Position;

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.


    protected override void ProcessVelocity(double delta)
    {
        base.ProcessVelocity(delta);

        attackState.Tick(delta);

        if (ball.Position.Y < 0 && attackState.CanStart)
        {
            attackState.Start();
        }


        var target = Vector2.Zero;
        float speed = 0;

        if (attackState.IsProcessed)
        {
            target = ball.Position;
            speed = Speed * 2;
        }
        else
        {
            var ballPosition = ball.Position;
            //ballPosition.Y *= 2;
            target = startPosition + (ballPosition - startPosition).Normalized() * 280;
            speed = Speed;
        }







        Velocity = (target - Position) / (float)delta;

        if (Velocity.Length() > speed)
        {
            Velocity = Velocity.Normalized() * speed;
        }

        //if (ball.Position.Y < 0)
        //{
        //    Velocity = distanceVector.Normalized() * 1000;

        //}
        //else
        //{
        //    var newPosition = new Vector2(ball.Position.X / 3 * 2, startPosition.Y);

        //    Velocity = (newPosition - Position) / (float)delta;

        //    if (Velocity.Length() > Speed)
        //    {
        //        Velocity = Velocity.Normalized() * Speed;
        //    }
        //}
    }


}
