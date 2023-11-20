using Godot;
using System;

public partial class Ball : CharacterBody2D
{

    public Vector2 Direction { get; set; }
    private float speed;
    private float friction = 0.75f;
    int frameSkip = 0;

    public event EventHandler<GodotObject> OnBounce;

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

        var travel = Direction * Speed * (float)delta;

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

                Direction = (target + targetVelocity) / 2;


                Speed = Direction.Length() / friction;

                if (Speed < 500) Speed = 500;

                Direction = Direction.Normalized();

                travel = Direction * Speed * (float)delta;

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
                Direction = Direction.Bounce(normal);
            }

            OnBounce?.Invoke(this, hit.GetCollider());
        }

        if (i >= 4)
        {
            frameSkip = 30;
            Speed = 4000;
            CollisionMask &= ~2u;
        }


    }

}
