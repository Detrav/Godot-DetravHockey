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


    public override void _PhysicsProcess(double delta)
    {
        attackState.Tick(delta);

        if (IsInstanceValid(ball))
        {


            if (ball.Position.Y < 0 && attackState.CanStart)
            {
                attackState.Start();
            }


            Vector2 target;

            if (attackState.IsProcessed)
            {
                target = ball.Position;
                Speed = 1000;
            }
            else
            {
                var ballPosition = ball.Position;
                target = startPosition + (ballPosition - startPosition).Normalized() * 280;
                Speed = 500;
            }
            TargetPosition = target;
        }






        base._PhysicsProcess(delta);
    }


}
