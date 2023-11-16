using Godot;
using System;

public partial class Ball : CharacterBody2D
{

    private Vector2 direction;
    private float speed;
    private float friction = 0.75f;
    int frameSkip = 0;

    public float Speed
    {
        get => speed;
        set
        {
            speed = Math.Clamp(value, 0, 4000);
        }
    }


    public override void _PhysicsProcess(double delta)
    {

        base._PhysicsProcess(delta);

        if (frameSkip <= 0)
        {
            CollisionMask |= 2u;
        }
        else
        {
            frameSkip--;
        }

        var travel = direction * Speed * (float)delta;

        int i = 0;

        for (; i < 4; i++)
        {



            var hit = MoveAndCollide(travel);



            if (hit == null)
                break;

            if (i == 0)
            {
                var player = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
                if (player.GetPlaybackPosition() > 0.05 || !player.Playing)
                    player.Play();
            }

            var normal = hit.GetNormal();

            if (hit.GetCollider() is PlayerBase characterBody && characterBody.CurrentSpeed.Length() > speed / 2)
            {



                var target = (Position - characterBody.Position).Normalized() * speed;
                var targetVelocity = characterBody.CurrentSpeed * 2;

                direction = (target + targetVelocity) / 2;


                Speed = direction.Length() / friction;

                if (Speed < 500) Speed = 500;

                direction = direction.Normalized();

                travel = direction * Speed * (float)delta;

                //if ((characterBody.Position - Position).Length() < 110)
                //{
                //    frameSkip = 30;
                //    Speed = 4000;
                //}
                ////GD.Print((characterBody.Position - Position).Length());
                //else
                {
                    frameSkip = 2;
                }
                CollisionMask &= ~2u;


            }
            else
            {
                speed = speed * friction;

                travel -= hit.GetTravel();


                travel = travel.Bounce(normal);
                direction = direction.Bounce(normal);
            }
        }

        if (i >= 4)
        {
            frameSkip = 30;
            Speed = 4000;
            CollisionMask &= ~2u;
        }


    }

}
