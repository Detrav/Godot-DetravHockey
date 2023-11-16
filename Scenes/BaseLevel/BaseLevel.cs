using Godot;
using System;

public partial class BaseLevel : Node2D
{
    public ShowText ShowTextNode { get; private set; }

    private int blue = 0;
    private int red = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ShowTextNode = GetNode<ShowText>("CanvasLayer/ShowText");
        ShowTextNode.ShowTextWithAnim("Start Game");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void _on_top_border_body_entered(Node2D body)
    {
        if (body is Ball ball)
        {
            ball.Velocity = Vector2.Zero;
            ball.Position = Vector2.Zero;
            ball.Speed = 0;
            blue++;
            ShowTextNode.ShowTextWithAnim("Blue goal!\n" + blue + " - " + red);
        }
    }

    public void _on_bottom_border_body_entered(Node2D body)
    {
        if (body is Ball ball)
        {
            ball.Velocity = Vector2.Zero;
            ball.Position = Vector2.Zero;
            ball.Speed = 0;
            red++;
            ShowTextNode.ShowTextWithAnim("Red goal!\n" + blue + " - " + red);
        }

    }


}
