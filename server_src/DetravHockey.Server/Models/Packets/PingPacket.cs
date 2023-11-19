using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetravHockey.Server.Models.Packets
{
    public class PingPacket : PacketBase
    {
        public DateTime Time { get; set; }
    }
}
