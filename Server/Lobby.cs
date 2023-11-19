using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetravHockey.Server
{
    public class Lobby
    {
        public static int LastId { get; set; } = 10000;
        public string Name { get; set; }
        public List<WSServerClient> Players { get; } = new List<WSServerClient>();

        public Lobby() { }



    }
}
