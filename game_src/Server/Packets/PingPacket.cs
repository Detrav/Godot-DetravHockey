using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetravHockey.Server.Packets
{
    public class PingPacket : PacketBase
    {
        public DateTime Time { get; set; }
    }
}
