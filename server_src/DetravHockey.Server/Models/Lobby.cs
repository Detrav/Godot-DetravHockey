namespace DetravHockey.Server.Models
{
    public class Lobby
    {
        public static int LastId { get; set; } = 10000;
        public string Name { get; set; }
        public List<WSServerClient> Players { get; } = new List<WSServerClient>();

        public Lobby() { }



    }

}
