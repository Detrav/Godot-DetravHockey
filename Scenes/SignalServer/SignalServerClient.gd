extends Node


var socket : WebSocketPeer
var lobby_name : String
var lobby_name_placeholder : String
var remote_lobby_name: String
var ws:  WebSocketPeer
var other



# Called when the node enters the scene tree for the first time.
func _ready():
	
	pass # Replace with function body.

func log_message(message):
	print(Time.get_time_string_from_system() + ": " + message);

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	socket.poll()

	if socket.get_ready_state() == WebSocketPeer.STATE_OPEN:
		while socket.get_available_packet_count():
			var json_text = socket.get_packet().get_string_from_utf8()
			print(json_text)
			process_packet(JSON.parse_string( json_text))
	elif socket.get_ready_state() == WebSocketPeer.STATE_CLOSED:
		queue_free()
		log_message("Close lobby " + lobby_name)

func send_packet(packet : Variant):
	var json_text = JSON.stringify(packet)
	print(json_text)
	socket.send( json_text.to_utf8_buffer())

func process_packet(packet : Variant):
	if packet.id == "get_lobby_list":
		var list = []
		for ch in get_parent().get_children():
			if !(ch.lobby_name == null || ch.lobby_name == ""):
				list.append(ch.lobby_name)
		send_packet({
			"id" : "set_lobby_list",
			"list" : list
		})
	elif packet.id == "create_lobby":
		lobby_name = lobby_name_placeholder
		send_packet({"id" : "set_lobby_name","value" : lobby_name})
		log_message("Start lobby " + lobby_name)
	elif packet.id == "set_session_description":
		other.send_packet(packet)
	elif packet.id == "add_ice_candidate":
		other.send_packet(packet)
	elif packet.id == "connect_to_lobby":
		remote_lobby_name = packet.name
		other = get_parent().get_node(remote_lobby_name)
		other.other = self
		other.send_packet({"id" : "create_offer"})
		#send_packet({"id": "connect_to_rtc", "type" : rtc_type, "sdp": rtc_sdp, "ices" : ices })


func _exit_tree():
	socket.close()
