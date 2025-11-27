using System;
using System.Collections.Generic;
using System.Linq;

namespace blackjack_nik
{
    public enum GameResult { InProgress, PlayerBust, DealerBust, PlayerWin, DealerWin, Push }

    public class BlackjackGame
    {
        private readonly GameOptions options;

        public Deck Deck { get; private set; }
        public Hand DealerHand { get; private set; }
        public List<PlayerHandState> PlayerHands { get; } = new List<PlayerHandState>();
        public int ActiveHandIndex { get; private set; }
        public decimal Bankroll { get; private set; }
        public bool RoundInProgress { get; private set; }
        public bool DealerTurnComplete { get; private set; }

        public decimal MinBet => options.MinBet;
        public decimal MaxBet => options.MaxBet;

        private readonly Deck[] deckRotation;
        private int deckPointer;

        public BlackjackGame(GameOptions options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            deckRotation = Enumerable.Range(0, 6).Select(_ => new Deck()).ToArray();
            deckPointer = 0;
            Bankroll = options.StartingBankroll;
        }

        public PlayerHandState ActiveHand =>
            RoundInProgress && ActiveHandIndex >= 0 && ActiveHandIndex < PlayerHands.Count
                ? PlayerHands[ActiveHandIndex]
                : null;

        public bool IsPlayerTurnActive => RoundInProgress && PlayerHands.Any(h => !h.PlayerTurnComplete);
        public bool ShouldHideDealerHoleCard => RoundInProgress && !DealerTurnComplete && IsPlayerTurnActive;

        public bool CanHitActiveHand =>
            ActiveHand != null && !ActiveHand.PlayerTurnComplete && ActiveHand.Result == GameResult.InProgress;

        public bool CanStandActiveHand => CanHitActiveHand;

        public bool CanDoubleDownActiveHand =>
            CanHitActiveHand &&
            ActiveHand.Hand.Cards.Count == 2 &&
            Bankroll >= ActiveHand.Bet;

        public bool CanSplitActiveHand =>
            CanHitActiveHand &&
            ActiveHand.Hand.Cards.Count == 2 &&
            ActiveHand.Hand.Cards[0].Rank == ActiveHand.Hand.Cards[1].Rank &&
            Bankroll >= ActiveHand.Bet;

        public void ResetBankroll()
        {
            Bankroll = options.StartingBankroll;
        }

        public bool CanPlaceBet(decimal bet)
        {
            if (RoundInProgress) return false;
            if (bet < MinBet || bet > MaxBet) return false;
            return bet <= Bankroll;
        }

        public void StartRound(decimal bet)
        {
            if (!CanPlaceBet(bet))
                throw new InvalidOperationException("Cannot start round with the requested bet.");

            PlayerHands.Clear();
            DealerHand = new Hand();
            PrepareDeckForRound();
            ActiveHandIndex = 0;
            DealerTurnComplete = false;
            RoundInProgress = true;

            Bankroll -= bet;

            var handState = new PlayerHandState(bet);
            PlayerHands.Add(handState);

            DealInitialCards(handState);
            PostInitialChecks();
        }

        public void PlayerHit()
        {
            if (!CanHitActiveHand) return;

            ActiveHand.Hand.Add(Deck.Deal());
            if (ActiveHand.Hand.IsBust())
            {
                ActiveHand.Result = GameResult.PlayerBust;
                ActiveHand.PlayerTurnComplete = true;
                AdvanceHandOrFinish();
            }
        }

        public void PlayerStand()
        {
            if (!CanStandActiveHand) return;

            ActiveHand.PlayerTurnComplete = true;
            AdvanceHandOrFinish();
        }

        public void PlayerDoubleDown()
        {
            if (!CanDoubleDownActiveHand) return;

            Bankroll -= ActiveHand.Bet;
            ActiveHand.Bet *= 2;
            ActiveHand.IsDoubleDown = true;

            ActiveHand.Hand.Add(Deck.Deal());
            if (ActiveHand.Hand.IsBust())
            {
                ActiveHand.Result = GameResult.PlayerBust;
            }

            ActiveHand.PlayerTurnComplete = true;
            AdvanceHandOrFinish();
        }

        public void PlayerSplit()
        {
            if (!CanSplitActiveHand) return;

            var original = ActiveHand;
            Bankroll -= original.Bet;

            var movedCard = original.Hand.RemoveAt(1);
            var splitHand = new PlayerHandState(original.Bet)
            {
                IsSplitHand = true
            };

            original.IsSplitHand = true;

            splitHand.Hand.Add(movedCard);

            original.Hand.Add(Deck.Deal());
            splitHand.Hand.Add(Deck.Deal());

            PlayerHands.Insert(ActiveHandIndex + 1, splitHand);
        }

        private void DealInitialCards(PlayerHandState firstHand)
        {
            firstHand.Hand.Add(Deck.Deal());
            DealerHand.Add(Deck.Deal());
            firstHand.Hand.Add(Deck.Deal());
            DealerHand.Add(Deck.Deal());

            if (firstHand.Hand.IsBlackjack())
            {
                firstHand.IsNaturalBlackjack = true;
                firstHand.PlayerTurnComplete = true;
            }
        }

        private void PostInitialChecks()
        {
            if (DealerHand.IsBlackjack())
            {
                DealerTurnComplete = true;
                foreach (var hand in PlayerHands)
                {
                    hand.PlayerTurnComplete = true;
                    if (hand.IsNaturalBlackjack)
                    {
                        hand.Result = GameResult.Push;
                        Bankroll += hand.Bet;
                    }
                    else
                    {
                        hand.Result = GameResult.DealerWin;
                    }
                }

                RoundInProgress = false;
                return;
            }

            if (PlayerHands.All(h => h.PlayerTurnComplete))
            {
                FinishRound();
            }
        }

        private void AdvanceHandOrFinish()
        {
            while (ActiveHandIndex < PlayerHands.Count && PlayerHands[ActiveHandIndex].PlayerTurnComplete)
                ActiveHandIndex++;

            if (ActiveHandIndex >= PlayerHands.Count)
            {
                FinishRound();
            }
        }

        private void FinishRound()
        {
            if (!DealerTurnComplete)
            {
                while (DealerHand.BestValue() < 17)
                {
                    DealerHand.Add(Deck.Deal());
                }

                DealerTurnComplete = true;
            }

            foreach (var hand in PlayerHands)
            {
                if (hand.Result == GameResult.PlayerBust)
                    continue;

                if (hand.IsNaturalBlackjack && !DealerHand.IsBlackjack() && !hand.IsSplitHand)
                {
                    AwardBlackjack(hand);
                    continue;
                }

                if (DealerHand.IsBust())
                {
                    AwardWin(hand);
                    continue;
                }

                int playerValue = hand.Hand.BestValue();
                int dealerValue = DealerHand.BestValue();

                if (playerValue > dealerValue)
                {
                    AwardWin(hand);
                }
                else if (playerValue < dealerValue)
                {
                    hand.Result = GameResult.DealerWin;
                }
                else
                {
                    hand.Result = GameResult.Push;
                    Bankroll += hand.Bet;
                }
            }

            RoundInProgress = false;
            AdvanceDeckRotation();
        }

        private void AwardWin(PlayerHandState hand)
        {
            hand.Result = GameResult.PlayerWin;
            Bankroll += hand.Bet * 2;
        }

        private void AwardBlackjack(PlayerHandState hand)
        {
            hand.Result = GameResult.PlayerWin;
            Bankroll += hand.Bet * 5m / 2m;
        }

        private void PrepareDeckForRound()
        {
            Deck = deckRotation[deckPointer];
            if (Deck.RemainingCards < Deck.CutCardThreshold)
            {
                Deck.Rebuild();
            }
            Deck.MarkForShuffle();
        }

        private void AdvanceDeckRotation()
        {
            deckPointer = (deckPointer + 1) % deckRotation.Length;
        }
    }

    public class PlayerHandState
    {
        public PlayerHandState(decimal bet)
        {
            Bet = bet;
            Hand = new Hand();
        }

        public Hand Hand { get; }
        public decimal Bet { get; set; }
        public bool PlayerTurnComplete { get; set; }
        public bool IsDoubleDown { get; set; }
        public bool IsSplitHand { get; set; }
        public bool IsNaturalBlackjack { get; set; }
        public GameResult Result { get; set; } = GameResult.InProgress;
    }
}
