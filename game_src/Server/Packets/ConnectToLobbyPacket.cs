using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetravHockey.Server.Packets
{
    public class ConnectToLobbyPacket : PacketBase
    {
        public string Name { get; set; }
    }
}
