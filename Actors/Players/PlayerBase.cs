using Godot;
using System;

public partial class PlayerBase : CharacterBody2D
{

    protected float Speed = 500;
    protected Ball ball;

    public Vector2 CurrentSpeed { get; set; }

    public override void _Ready()
    {
    }


    public override void _Process(double delta)
    {
    }


    public override void _PhysicsProcess(double delta)
    {
        if (!IsInstanceValid(ball))
        {
            ball = GetParent().GetNode<Ball>("Ball");
        }
        else
        {
            ProcessVelocity(delta);

            var currentPosition = Position;
            MoveAndSlide();
            CurrentSpeed = (Position - currentPosition) / (float)delta;
        }
    }

    protected virtual void ProcessVelocity(double delta)
    {

    }
}
