using System.Collections.Generic;
using System.Linq;

namespace blackjack_nik
{
    public class Hand
    {
        public List<Card> Cards { get; } = new List<Card>();

        public void Add(Card c) => Cards.Add(c);

        public Card RemoveAt(int index)
        {
            var card = Cards[index];
            Cards.RemoveAt(index);
            return card;
        }

        // Best score <= 21, handling Aces as 1 or 11
        public int BestValue()
        {
            int baseSum = Cards.Sum(c => c.Value); // counts each Ace as 1
            int aces = Cards.Count(c => c.Rank == Rank.Ace);
            int best = baseSum;
            for (int i = 0; i < aces; i++)
            {
                // try to upgrade one Ace from 1 to 11 (+10)
                if (best + 10 <= 21) best += 10;
            }
            return best;
        }

        public bool IsBust() => BestValue() > 21;
        public bool IsBlackjack() => Cards.Count == 2 && BestValue() == 21;
    }
}
