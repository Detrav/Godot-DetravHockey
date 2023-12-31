using Godot;
using System;

public partial class MainMenu : Control
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
       
        
        DisplayServer.WindowSetTitle("Session " + (new Random().Next(int.MaxValue)) );
        

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void _on_button_single_pressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/DebugScene/DebugLevel.tscn");
    }

    public void _on_button_exit_pressed()
    {
        GetTree().Quit();
    }

    public void _on_button_start_server_pressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/MainMenu/StartLobbyMenu.tscn");
    }

    public void _on_button_start_signal_server_pressed()
    {
        GetTree().ChangeSceneToFile("res://Server/WSServer.tscn");
    }

    public void _on_button_connect_server_pressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/MainMenu/JoinLobbyMenu.tscn");
    }
}
