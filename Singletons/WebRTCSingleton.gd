extends Node

var peer : WebRTCPeerConnection;
var channel : WebRTCDataChannel;
var ice_candidate_created3 : Callable
var play_state = 0
var b_is_server = false

signal package_received(package:String)

# Called when the node enters the scene tree for the first time.
func _ready():
	print("Start Server")
	pass # Replace with function body.


func create_server( session_description_created : Callable, ice_candidate_created : Callable):
	b_is_server = true
	play_state = 0
	peer = WebRTCPeerConnection.new()
	channel = peer.create_data_channel("lobby_name", {"id": 1, "negotiated": true})
	peer.session_description_created.connect(session_description_created)
	peer.session_description_created.connect(peer_set_local_description)
	ice_candidate_created3 = ice_candidate_created #peer.ice_candidate_created.connect(ice_candidate_created)
	peer.ice_candidate_created.connect(ice_candidate_created2)
	
func create_offer():
	peer.create_offer()
	pass
	
func join_server(session_description_created : Callable):
	b_is_server = false
	play_state = 0
	peer = WebRTCPeerConnection.new()
	channel = peer.create_data_channel("lobby_name", {"id": 1, "negotiated": true})
	
	#for ice in ices:
		#peer.add_ice_candidate(ice.media,ice.index,ice.name)
	#peer.set_remote_description(type, sdp)
	peer.session_description_created.connect(session_description_created)
	peer.session_description_created.connect(peer_set_local_description)
	
func set_remote_session_description(type: String, sdp: String):
	peer.set_remote_description(type,sdp)

func peer_set_local_description(type: String, sdp: String):
	peer.set_local_description(type,sdp)
	
func add_ice_candidate(media: String, index: int, name: String):
	peer.add_ice_candidate(media,index,name)
	
func ice_candidate_created2(media: String, index: int, name: String):
	# fuck
	ice_candidate_created3.call(media,index,name)
	pass

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if is_instance_valid(peer) :
		peer.poll()
		
		if play_state == 0 && channel.get_ready_state() == 1:
			play_state = 1
			get_tree().change_scene_to_file("res://Scenes/NetworkLevel/NetworkLevel.tscn")
		elif play_state == 1:
			while channel.get_available_packet_count() > 0:
				package_received.emit(channel.get_packet().get_string_from_utf8())
		
		#var c_state = peer.get_connection_state()
		#var g_state = peer.get_gathering_state()
		#var s_state = peer.get_signaling_state()
		#var ch_state = channel.get_ready_state()
		pass

func send_package(package:String):
	channel.put_packet(package.to_utf8_buffer())
