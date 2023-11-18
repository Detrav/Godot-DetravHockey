extends Control

var ws = WebSocketPeer.new()
var process_internal = true
var global_state = 0

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
			if global_state == 0 :
				global_state = 1
				_on_button_refresh_pressed()
			$CenterContainer/VBoxContainer/Label2.text = "Ready"
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

func _on_button_back_pressed():
	get_tree().change_scene_to_file("res://Scenes/MainMenu/MainMenu.tscn")

func process_packet(packet : Variant):
	if packet.id == "set_lobby_list":
		$CenterContainer/VBoxContainer/ItemList.clear()
		for name in packet.list:
			$CenterContainer/VBoxContainer/ItemList.add_item(name)
	elif packet.id == "set_session_description":
		var webrtc = get_tree().root.get_node("/root/WebRtcSingleton")
		webrtc.set_remote_session_description(packet.type,packet.sdp)
	elif packet.id == "add_ice_candidate":
		var webrtc = get_tree().root.get_node("/root/WebRtcSingleton")
		webrtc.add_ice_candidate(packet.media,packet.index,packet.name)
		
func send_packet(packet : Variant):
	ws.send( JSON.stringify(packet).to_utf8_buffer())

func _on_button_connect_pressed():
	var items = $CenterContainer/VBoxContainer/ItemList.get_selected_items()
	if items.size() > 0 :
		var item = $CenterContainer/VBoxContainer/ItemList.get_item_text(items[0])
		send_packet({"id" : "connect_to_lobby", "name" : item})
		var webrtc = get_tree().root.get_node("/root/WebRtcSingleton")
		webrtc.join_server(session_description_created)
	pass # Replace with function body.


func _on_button_refresh_pressed():
	send_packet({"id" : "get_lobby_list"})
	pass # Replace with function body.
	
	
func session_description_created(type: String, sdp: String):
	send_packet({
		"id" : "set_session_description",
		"type" : type,
		"sdp" : sdp
	})
	pass
	
