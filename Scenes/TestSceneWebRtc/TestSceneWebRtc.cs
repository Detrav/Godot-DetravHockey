using Godot;
using System;

public partial class TestSceneWebRtc : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var p1 = new WebRtcPeerConnection();
		p1.CreateDataChannel("chat");
		p1.CreateOffer();
//# var p1 = WebRTCPeerConnection.new()
//# var p2 = WebRTCPeerConnection.new()
//# var ch1 = p1.create_data_channel("chat", {"id": 1, "negotiated": true})
//# var ch2 = p2.create_data_channel("chat", {"id": 1, "negotiated": true})
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
