extends Node

const PORT = 9080
var tcp_server := TCPServer.new()
#var socket := WebSocketPeer.new()
var lobby_index_last = 10000;

func log_message(message):
	print(Time.get_time_string_from_system() + ": " + message);

# Called when the node enters the scene tree for the first time.
func _ready():
	if tcp_server.listen(PORT) != OK:
		log_message("Unable to start server.")
		set_process(false)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta):
	while tcp_server.is_connection_available():
		var conn: StreamPeerTCP = tcp_server.take_connection()
		assert(conn != null)
		var socket = WebSocketPeer.new()
		socket.accept_stream(conn)
		
		var client = load("res://Scenes/SignalServer/SignalServerClient.tscn").instantiate()
		client.socket = socket
		lobby_index_last+=1
		client.name =  str(lobby_index_last)
		client.lobby_name_placeholder =  str(lobby_index_last)
		add_child(client)


		
func _exit_tree():
	tcp_server.stop()


