using System;
using System.Collections.Generic;

namespace blackjack_nik
{
    public class Deck
    {
        private static readonly Random rng = new Random();

        private readonly List<Card> cards = new List<Card>();
        private readonly int deckCopies;
        private bool shuffleRequired = true;

        public Deck(int deckCopies = 1)
        {
            this.deckCopies = Math.Max(1, deckCopies);
            BuildFreshShoe();
        }

        public int RemainingCards => cards.Count;
        public int TotalCards => deckCopies * 52;
        public int CutCardThreshold => Math.Max(26, TotalCards / 5);

        public void MarkForShuffle()
        {
            shuffleRequired = true;
        }

        public void Rebuild()
        {
            BuildFreshShoe();
        }

        public Card Deal()
        {
            EnsureReady();

            if (cards.Count == 0)
                throw new InvalidOperationException("Deck empty");

            int lastIndex = cards.Count - 1;
            var card = cards[lastIndex];
            cards.RemoveAt(lastIndex);

            if (cards.Count == 0)
                shuffleRequired = true;

            return card;
        }

        private void EnsureReady()
        {
            if (cards.Count == 0)
            {
                BuildFreshShoe();
            }

            if (shuffleRequired)
            {
                LazyShuffle();
            }
        }

        private void BuildFreshShoe()
        {
            cards.Clear();
            for (int copy = 0; copy < deckCopies; copy++)
            {
                foreach (Suit suit in Enum.GetValues(typeof(Suit)))
                {
                    foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                    {
                        cards.Add(new Card(rank, suit));
                    }
                }
            }

            shuffleRequired = true;
        }

        private void LazyShuffle()
        {
            for (int i = cards.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                (cards[i], cards[j]) = (cards[j], cards[i]);
            }

            shuffleRequired = false;
        }
    }
}
