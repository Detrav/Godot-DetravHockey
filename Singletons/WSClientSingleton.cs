using Godot;
using System;

public partial class WSClientSingleton : Node
{
    public static WSClientSingleton Instance { get; private set; }
    public WSServerClient Client { get; internal set; }
    public bool IsServer { get; internal set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		Instance = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
