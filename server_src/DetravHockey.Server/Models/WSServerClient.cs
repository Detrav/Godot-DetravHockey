using DetravHockey.Server.Models.Packets;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace DetravHockey.Server.Models
{
    public class WSServerClient : IDisposable
    {
        public int lastId = 10000;
        public static ConcurrentDictionary<string, Lobby> Lobbies { get; } = new ConcurrentDictionary<string, Lobby>();

        private static JsonSerializerOptions options = new JsonSerializerOptions()
        {
            TypeInfoResolver = new MyCustomResolver()
        };
        private readonly WebSocket socket;

        public Lobby? Lobby { get; set; }
        public object Id { get; }

        public WSServerClient(WebSocket webSocket)
        {
            this.socket = webSocket;
            Id = lastId++;
        }

        public void ProcessPacket(ReadOnlySpan<byte> packet)
        {
            var result = JsonSerializer.Deserialize<PacketBase>(packet, options);
            if (result != null)
            {
                ProcessPacket(result);
            }
        }

        public void ProcessPacket(PacketBase packet)
        {
            switch (packet)
            {
                case PingPacket pingPacket:
                    SendPacket(new PongPacket() { Time = pingPacket.Time });
                    break;
                case CreateLobbyPacket:

                    Lobby = new Lobby();

                    Lobby.Name = (Lobby.LastId++).ToString();
                    Lobby.Players.Add(this);
                    Lobbies[Lobby.Name] = Lobby;
                    SendPacket(new SetLobbyPacket()
                    {
                        Name = Lobby.Name,
                    });
                    break;
                case GetLobiesListPacket:
                    SendPacket(new SetLobiesListPacket()
                    {
                        Names = Lobbies.Keys.ToArray()
                    });
                    break;

                case ConnectToLobbyPacket connectToLobbyPacket:
                    if (Lobbies.TryGetValue(connectToLobbyPacket.Name, out var lobby))
                    {
                        Lobby = lobby;
                        Lobby.Players.Add(this);

                        foreach (var player in Lobby.Players)
                        {
                            player.SendPacket(new StartGamePacket());
                        }
                    }
                    break;

                case BallPositionPacket:
                case PlayerPositionPacket:

                    if (Lobby != null)
                    {
                        foreach (var player in Lobby.Players)
                        {
                            if (player.Id != Id)
                            {
                                player.SendPacket(packet);
                            }
                        }
                    }

                    break;
            }
        }

        public void SendPacket(PacketBase packet)
        {
            var json = JsonSerializer.Serialize(packet, options);
            var bytes = Encoding.UTF8.GetBytes(json);
            _ = socket.SendAsync(bytes, WebSocketMessageType.Binary, true, CancellationToken.None);

        }

        public void Dispose()
        {
            if (Lobby != null)
            {
                Lobbies.Remove(Lobby.Name, out _);
                Lobby.Players.Clear();
                Lobby = null;
            }
        }
    }
}
