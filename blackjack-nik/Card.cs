namespace blackjack_nik
{
    public enum Suit { Clubs, Diamonds, Hearts, Spades }
    public enum Rank { Two = 2, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack = 11, Queen = 12, King = 13, Ace = 1 }

    public class Card
    {
        public Suit Suit { get; }
        public Rank Rank { get; }

        public Card(Rank rank, Suit suit)
        {
            Rank = rank;
            Suit = suit;
        }

        public int Value
        {
            get
            {
                if (Rank >= Rank.Jack && Rank <= Rank.King) return 10;
                if (Rank == Rank.Ace) return 1; // Ace handled specially in Hand
                return (int)Rank;
            }
        }

        public override string ToString() => $"{Rank} of {Suit}";
    }
}
