using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetravHockey.Server.Packets
{
    public class SetLobbyPacket : PacketBase
    {
        public string Name { get; set; }
    }
}
