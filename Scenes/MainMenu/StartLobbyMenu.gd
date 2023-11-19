extends Control

var ws = WebSocketPeer.new()
var process_internal = true;
var global_state = 0;

# Called when the node enters the scene tree for the first time.
func _ready():
	ws.connect_to_url("ws://20.203.167.235:9080")
	#ws.send("test".to_utf8_buffer());
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if process_internal :
		ws.poll()
		var state = ws.get_ready_state()
		if state == WebSocketPeer.STATE_CONNECTING:
			$CenterContainer/VBoxContainer/Label2.text = "Connecting"
		elif state == WebSocketPeer.STATE_OPEN:
			if global_state == 0:
				$CenterContainer/VBoxContainer/Label2.text = "Ready"
				global_state = 1;
				#send_packet({"id" : "create_lobby" })
				var webrtc = get_tree().root.get_node("/root/WebRtcSingleton")
				webrtc.create_server(session_description_created, ice_candidate_created)
				send_packet({"id" : "create_lobby" })
			while ws.get_available_packet_count():
				process_packet(JSON.parse_string(ws.get_packet().get_string_from_utf8()))
		elif state == WebSocketPeer.STATE_CLOSING:
			$CenterContainer/VBoxContainer/Label2.text = "Closing"
			pass
		elif state == WebSocketPeer.STATE_CLOSED:
			
			var code = ws.get_close_code()
			var reason = ws.get_close_reason()
			$CenterContainer/VBoxContainer/Label2.text = "WebSocket closed with code: %d, reason %s. Clean: %s" % [code, reason, code != -1]
			process_internal = false
	if global_state == 1 :
		var webrtc = get_tree().root.get_node("/root/WebRtcSingleton")
		if is_instance_valid(webrtc.peer) :
			var rtc = webrtc.peer as WebRTCPeerConnection
			var ch = webrtc.channel as WebRTCDataChannel
			$CenterContainer/VBoxContainer/LabelDebug.text = "%d : %d : %d : %d" % [rtc.get_connection_state(), rtc.get_gathering_state(), rtc.get_signaling_state(), ch.get_ready_state() if is_instance_valid(ch) else -1]

func session_description_created(type: String, sdp: String):
	send_packet({
		"id" : "set_session_description",
		"type" : type,
		"sdp" : sdp
	})
	pass
	
func ice_candidate_created(media: String, index: int, name: String):
	send_packet({
		"id" : "add_ice_candidate",
		"media" : media,
		"index" : index,
		"name" : name
	})
	pass

func _on_button_back_pressed():
	get_tree().change_scene_to_file("res://Scenes/MainMenu/MainMenu.tscn")
	
func send_packet(packet : Variant):
	ws.send( JSON.stringify(packet).to_utf8_buffer())

func process_packet(packet : Variant):
	if packet.id == "set_lobby_name":
		global_state = 1
		$CenterContainer/VBoxContainer/Label2.text = packet.value
	elif packet.id == "create_offer":
		var webrtc = get_tree().root.get_node("/root/WebRtcSingleton")
		webrtc.create_offer()
	elif packet.id == "set_session_description":
		var webrtc = get_tree().root.get_node("/root/WebRtcSingleton")
		webrtc.set_remote_session_description(packet.type,packet.sdp)
	

