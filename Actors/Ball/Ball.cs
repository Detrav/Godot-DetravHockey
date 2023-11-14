using Godot;
using System;

public partial class Ball : RigidBody2D
{

    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        if (state.LinearVelocity.Length() > 2000)
        {
            state.LinearVelocity *= 4000 / state.LinearVelocity.Length();
        }
        base._IntegrateForces(state);
    }

    //private Vector2 directionSpeed;

    //public Vector2 DirectionSpeed
    //{
    //    get => directionSpeed;
    //    set
    //    {
    //        var len = value.Length();
    //        if (len > 2000)
    //        {
    //            directionSpeed = value.Normalized() * 2000;
    //        }
    //        else
    //        {
    //            directionSpeed = value;
    //        }
    //    }
    //}

    //// Called when the node enters the scene tree for the first time.
    //public override void _Ready()
    //{
    //}

    //// Called every frame. 'delta' is the elapsed time since the previous frame.
    //public override void _Process(double delta)
    //{
    //}

    //public override void _PhysicsProcess(double delta)
    //{


    //    var travel = DirectionSpeed * (float)delta;

    //    //var hit = MoveAndCollide(DirectionSpeed * (float)delta);

    //    //if (hit != null)
    //    //{
    //    //    DirectionSpeed = DirectionSpeed.Bounce(hit.GetNormal());
    //    //}

    //    for (int i = 0; i < 2; i++)
    //    {
    //        var hit = MoveAndCollide(travel);

    //        if (hit == null)
    //        {
    //            break;
    //        }



    //        var angle = travel.AngleTo(hit.GetNormal());
    //        GD.Print(angle);
    //        if (Math.Abs(angle) < 1.8)
    //        {
    //            travel = travel.Slide(hit.GetNormal());

    //            //if (travel.Length() > 0.01)
    //            //{
    //            //    DirectionSpeed = travel.Normalized() * DirectionSpeed.Length();
    //            //}
    //        }
    //        else
    //        {
    //            DirectionSpeed = DirectionSpeed * 0.9f;
    //            travel = travel.Bounce(hit.GetNormal());
    //            DirectionSpeed = DirectionSpeed.Bounce(hit.GetNormal());
    //        }


    //        //GD.Print(travel.AngleTo(hit.GetNormal()));

    //        //var reminder = mouse - GlobalPosition;
    //        //direction = reminder.Slide(hit.GetNormal());

    //    }

    //}
}
