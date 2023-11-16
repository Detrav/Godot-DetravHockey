using Godot;
using System;

public partial class MousePlayer : PlayerBase
{
    protected override void ProcessVelocity(double delta)
    {
        base.ProcessVelocity(delta);

        var mouse = GetGlobalMousePosition();
        var direction = mouse - GlobalPosition;

        Velocity = direction / (float)delta;
    }

   
}
