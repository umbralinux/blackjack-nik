namespace blackjack_nik
{
    public class ServerInfo
    {
        public string Name { get; set; }
        public string Region { get; set; }
        public string Status { get; set; }
        public string Players { get; set; }
        public string Description { get; set; }
        public string MaxPlayers { get; set; } // Helper for parsing
    }
}
