using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetravHockey.Server.Models.Packets
{
    public class PlayerPositionPacket : PacketBase
    {
        public float X { get;  set; }
        public float Y { get;  set; }
        public float DX { get;  set; }
        public float DY { get;  set; }
    }
}
