using Godot;
using System;
using System.ComponentModel.DataAnnotations;

public partial class ShowText : Control
{

    public void ShowTextWithAnim(string text)
    {
        var lbl = GetNode<Label>("Label");
        if (lbl != null) lbl.Text = text;
        var player = GetNode<AnimationPlayer>("AnimationPlayer");
        if (player != null) player.Play("FadeOut");
    }

    public void UnPause()
    {
        GetTree().Paused = false;
    }

    public void Pause()
    {
        GetTree().Paused = true;
    }

}
