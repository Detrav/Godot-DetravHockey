using Godot;
using System;

public partial class MousePlayer : CharacterBody2D
{
    public float Speed = 500;

    Ball ball;
    //Vector2 currentSpeed;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {

    }

    public override void _PhysicsProcess(double delta)
    {
        //GlobalPosition = GetGlobalMousePosition();

        if (!IsInstanceValid(ball))
        {
            ball = GetParent().GetNode<Ball>("Ball");
        }
        else
        {


            //if (Position.DistanceTo(ball.Position))
            //{

            //}
            //GD.Print(Position.DistanceTo(ball.Position));
        }

        var mouse = GetGlobalMousePosition();
        var direction = mouse - GlobalPosition;

        Velocity = direction / (float)delta;



        MoveAndSlide();

        //for (int i = 0; i < GetSlideCollisionCount(); i++)
        //{
        //    var c = GetSlideCollision(i);
        //    if (c.GetCollider() is RigidBody2D rb)
        //    {
        //        rb.ApplyImpulse(-c.GetNormal() * 200);
        //    }
        //}


        //for (int i = 0; i < 2; i++)
        //{
        //    var lastPosition = GlobalPosition;

        //    var hit = MoveAndCollide(direction);

        //    var targetSpeed = (GlobalPosition - lastPosition) / (float)delta;

        //    currentSpeed = (currentSpeed + targetSpeed) / 2;

        //    if (hit == null)
        //    {
        //        break;
        //    }
        //    if (hit.GetCollider() is Ball ball)
        //    {

        //        //ball.DirectionSpeed +=  (ball.GlobalPosition - GlobalPosition).Normalized() * 1000;
        //        break;
        //    }
        //    var reminder = mouse - GlobalPosition;
        //    direction = reminder.Slide(hit.GetNormal());

        //}
    }
}
