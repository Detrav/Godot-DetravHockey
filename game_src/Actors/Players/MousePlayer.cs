using Godot;
using System;

public partial class MousePlayer : PlayerBase
{

    public override void _Process(double delta)
    {
        base._Process(delta);
        TargetPosition = GetGlobalMousePosition();
    }

}
