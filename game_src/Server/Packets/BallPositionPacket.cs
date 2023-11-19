using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetravHockey.Server.Packets
{
    public class BallPositionPacket : PacketBase
    {
        public float X { get;  set; }
        public float Y { get;  set; }
        public float DX { get;  set; }
        public float DY { get;  set; }
        public float S { get;  set; }
    }
}
