using Godot;
using System;

public partial class PlayerBase : CharacterBody2D
{

    protected float Speed = 2000;
    protected Ball ball;


    public Vector2 CurrentSpeed { get; set; }
    public Vector2 TargetPosition { get; set; }

    public override void _Ready()
    {
    }


    public override void _Process(double delta)
    {
        if (!IsInstanceValid(ball))
        {
            ball = GetParent().GetNode<Ball>("Ball");
        }
    }


    public override void _PhysicsProcess(double delta)
    {



        var direction = TargetPosition - GlobalPosition;

        Velocity = direction / (float)delta;

        if (Velocity.Length() > Speed)
        {
            Velocity = Velocity.Normalized() * Speed;
        }

        var currentPosition = Position;
        MoveAndSlide();
        CurrentSpeed = (Position - currentPosition) / (float)delta;

    }


}
