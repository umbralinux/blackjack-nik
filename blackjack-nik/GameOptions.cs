namespace blackjack_nik
{
    public class GameOptions
    {
        public decimal StartingBankroll { get; set; } = 1000m;
        public decimal MinBet { get; set; } = 10m;
        public decimal MaxBet { get; set; } = 500m;

        public static GameOptions FromSettings()
        {
            var settings = Properties.Settings.Default;
            var options = new GameOptions
            {
                StartingBankroll = settings.StartingBankroll,
                MinBet = settings.MinBet,
                MaxBet = settings.MaxBet
            };
            if (options.MinBet > options.MaxBet)
            {
                options.MaxBet = options.MinBet;
            }

            return options;
        }
    }
}

