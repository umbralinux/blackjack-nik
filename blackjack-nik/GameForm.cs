using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace blackjack_nik
{
    public partial class GameForm : Form
    {
        private const float baseWidth = 1280f;
        private const float baseHeight = 720f;

        private readonly GameOptions options;
        private BlackjackGame game;

        public GameForm() : this(GameOptions.FromSettings())
        {
        }

        public GameForm(GameOptions options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.Sizable;
            MaximizeBox = true;
            Load += GameForm_Load;
            Resize += GameForm_Resize;
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            game = new BlackjackGame(options);
            nudBet.Minimum = options.MinBet;
            nudBet.Maximum = options.MaxBet;
            nudBet.Value = ClampBet(game.Bankroll);
            UpdateUI(true);
            float scale = Math.Min(Width / baseWidth, Height / baseHeight);
            ScaleCardImages(scale);
        }

        private decimal ClampBet(decimal bankroll)
        {
            if (bankroll <= 0) return options.MinBet;
            return Math.Min(options.MaxBet, Math.Max(options.MinBet, bankroll));
        }

        private void btnDeal_Click(object sender, EventArgs e)
        {
            if (game.RoundInProgress) return;

            decimal bet = nudBet.Value;
            try
            {
                game.StartRound(bet);
                UpdateUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Unable to deal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnHit_Click(object sender, EventArgs e)
        {
            if (!game.CanHitActiveHand) return;

            game.PlayerHit();
            UpdateUI();
        }

        private void btnStand_Click(object sender, EventArgs e)
        {
            if (!game.CanStandActiveHand) return;

            game.PlayerStand();
            UpdateUI();
        }

        private void btnDoubleDown_Click(object sender, EventArgs e)
        {
            if (!game.CanDoubleDownActiveHand) return;

            game.PlayerDoubleDown();
            UpdateUI();
        }

        private void btnSplit_Click(object sender, EventArgs e)
        {
            if (!game.CanSplitActiveHand) return;

            game.PlayerSplit();
            UpdateUI();
        }

        private void btnResetBankroll_Click(object sender, EventArgs e)
        {
            if (game.RoundInProgress)
            {
                MessageBox.Show("Finish the current round before resetting your bankroll.", "Round in progress",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            game.ResetBankroll();
            nudBet.Value = ClampBet(game.Bankroll);
            UpdateUI(true);
        }

        private void UpdateUI(bool initial = false)
        {
            lblDealer.Text = game.DealerHand != null && game.DealerHand.Cards.Any()
                ? $"Dealer ({(game.ShouldHideDealerHoleCard ? "??" : game.DealerHand.BestValue().ToString(CultureInfo.InvariantCulture))})"
                : "Dealer";

            lblPrimaryHandTitle.Text = "Player - First Hand";

            lblBalance.Text = $"Balance: {game.Bankroll.ToString("C0", CultureInfo.CurrentCulture)}";
            lblActiveBet.Text = game.RoundInProgress && game.ActiveHand != null
                ? $"Bet: {game.ActiveHand.Bet.ToString("C0", CultureInfo.CurrentCulture)}"
                : "Bet: --";

            lblActiveHand.Text = game.RoundInProgress && game.ActiveHand != null
                ? $"Hand {game.ActiveHandIndex + 1} of {game.PlayerHands.Count}"
                : "No active hand";
            lblPlayerScore.Text = BuildPlayerScoreSummary();

            RenderDealerCards();
            RenderPlayerHands();

            nudBet.Enabled = !game.RoundInProgress;
            btnDeal.Enabled = !game.RoundInProgress && game.Bankroll >= game.MinBet;
            btnHit.Enabled = game.CanHitActiveHand;
            btnStand.Enabled = game.CanStandActiveHand;
            btnDouble.Enabled = game.CanDoubleDownActiveHand;
            btnSplit.Enabled = game.CanSplitActiveHand;
            btnResetBankroll.Enabled = !game.RoundInProgress;

            if (!game.RoundInProgress && !game.PlayerHands.Any())
            {
                lblMessage.Text = "Set your bet and press Deal to begin.";
            }
            else if (game.RoundInProgress && game.ActiveHand != null)
            {
                lblMessage.Text = $"Hand {game.ActiveHandIndex + 1}: choose your move.";
            }
            else if (!game.RoundInProgress && game.PlayerHands.Any())
            {
                lblMessage.Text = BuildRoundSummary();
            }

            float scale = Math.Min(Width / baseWidth, Height / baseHeight);
            ScaleCardImages(scale);
        }

        private string BuildRoundSummary()
        {
            var parts = game.PlayerHands
                .Select((hand, idx) => $"Hand {idx + 1}: {DescribeResult(hand)}")
                .ToArray();
            return string.Join(" | ", parts);
        }

        private string DescribeResult(PlayerHandState hand)
        {
            switch (hand.Result)
            {
                case GameResult.PlayerBust:
                    return "Busted";
                case GameResult.PlayerWin:
                    return hand.IsNaturalBlackjack && !hand.IsSplitHand ? "Blackjack!" : "Win";
                case GameResult.DealerWin:
                    return "Dealer Wins";
                case GameResult.Push:
                    return "Push";
                default:
                    return "Pending";
            }
        }

        private void RenderDealerCards()
        {
            pnlDealer.Controls.Clear();
            if (game.DealerHand == null) return;

            for (int i = 0; i < game.DealerHand.Cards.Count; i++)
            {
                var pic = CreateCardPictureBox();
                bool hide = game.ShouldHideDealerHoleCard && i == 1;
                pic.Image = hide ? GetCardBack() : GetCardImage(game.DealerHand.Cards[i]);
                pnlDealer.Controls.Add(pic);
            }
        }

        private void RenderPlayerHands()
        {
            pnlPrimaryHand.Controls.Clear();
            pnlSplitHands.Controls.Clear();
            splitSection.Visible = false;

            if (!game.PlayerHands.Any()) return;

            for (int i = 0; i < game.PlayerHands.Count; i++)
            {
                var handState = game.PlayerHands[i];
                var handView = CreateHandView(i, handState);

                if (i == 0)
                {
                    pnlPrimaryHand.Controls.Add(handView);
                }
                else
                {
                    pnlSplitHands.Controls.Add(handView);
                    splitSection.Visible = true;
                }
            }

            if (splitSection.Visible)
            {
                int splitCount = Math.Max(0, game.PlayerHands.Count - 1);
                lblSplitHandTitle.Text = $"Split Hands ({splitCount})";
            }
            else
            {
                lblSplitHandTitle.Text = "Split Hands";
            }
        }

        private string BuildHandTitle(int index, PlayerHandState handState)
        {
            var title = $"Hand {index + 1} - {handState.Bet.ToString("C0", CultureInfo.CurrentCulture)}";
            if (game.RoundInProgress && index == game.ActiveHandIndex && game.ActiveHand == handState)
            {
                title += " (Active)";
            }

            if (!game.RoundInProgress && handState.Result != GameResult.InProgress)
            {
                title += $" [{DescribeResult(handState)}]";
            }

            return title;
        }

        private string BuildPlayerScoreSummary()
        {
            if (game == null || !game.PlayerHands.Any())
                return "Score: --";

            if (game.PlayerHands.Count == 1)
                return $"Score: {game.PlayerHands[0].Hand.BestValue()}";

            var parts = game.PlayerHands
                .Select((hand, idx) => $"H{idx + 1}:{hand.Hand.BestValue()}")
                .ToArray();

            return $"Score: {string.Join(" | ", parts)}";
        }

        private Control CreateHandView(int index, PlayerHandState handState)
        {
            bool isActive = game.RoundInProgress && index == game.ActiveHandIndex && game.ActiveHand == handState;

            var container = new Panel
            {
                BackColor = isActive ? Color.FromArgb(80, Color.DarkGoldenrod) : Color.FromArgb(20, Color.Black),
                Margin = new Padding(10),
                Padding = new Padding(10),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };

            var title = new Label
            {
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                Text = BuildHandTitle(index, handState),
                Dock = DockStyle.Top,
                AutoSize = true
            };

            var subtitle = new Label
            {
                ForeColor = Color.LightGray,
                Font = new Font("Segoe UI", 10f, FontStyle.Italic),
                Text = $"Value: {handState.Hand.BestValue()}",
                Dock = DockStyle.Top,
                AutoSize = true
            };

            var actionsLabel = new Label
            {
                ForeColor = isActive ? Color.Yellow : Color.Silver,
                Font = new Font("Segoe UI", 9f, FontStyle.Regular),
                Text = isActive ? "Use Hit / Stand / Double / Split." : "Waiting for turn...",
                Dock = DockStyle.Top,
                AutoSize = true
            };

            var cardsPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Dock = DockStyle.Top,
                Margin = new Padding(0, 10, 0, 0)
            };

            // Render player hand cards using shared rendering logic
            RenderHandCards(cardsPanel, handState);

            container.Controls.Add(cardsPanel);
            container.Controls.Add(actionsLabel);
            container.Controls.Add(subtitle);
            container.Controls.Add(title);

            return container;
        }

        private PictureBox CreateCardPictureBox()
        {
            return new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                Width = 120,
                Height = 170,
                Margin = new Padding(5)
            };
        }

        private Image GetCardImage(Card card)
        {
            Bitmap spriteSheet = Properties.Resources.cards;
            int cardWidth = 48;
            int cardHeight = 64;

            int suitIndex = 0;
            switch (card.Suit)
            {
                case Suit.Hearts: suitIndex = 0; break;
                case Suit.Diamonds: suitIndex = 1; break;
                case Suit.Spades: suitIndex = 2; break;
                case Suit.Clubs: suitIndex = 3; break;
            }

            int rankIndex = 0;
            switch (card.Rank)
            {
                case Rank.Ace: rankIndex = 0; break;
                case Rank.Two: rankIndex = 1; break;
                case Rank.Three: rankIndex = 2; break;
                case Rank.Four: rankIndex = 3; break;
                case Rank.Five: rankIndex = 4; break;
                case Rank.Six: rankIndex = 5; break;
                case Rank.Seven: rankIndex = 6; break;
                case Rank.Eight: rankIndex = 7; break;
                case Rank.Nine: rankIndex = 8; break;
                case Rank.Ten: rankIndex = 9; break;
                case Rank.Jack: rankIndex = 10; break;
                case Rank.Queen: rankIndex = 11; break;
                case Rank.King: rankIndex = 12; break;
            }

            Rectangle srcRect = new Rectangle(rankIndex * cardWidth, suitIndex * cardHeight, cardWidth, cardHeight);
            Bitmap cardImage = new Bitmap(cardWidth, cardHeight);

            using (Graphics g = Graphics.FromImage(cardImage))
                g.DrawImage(spriteSheet, new Rectangle(0, 0, cardWidth, cardHeight), srcRect, GraphicsUnit.Pixel);

            return cardImage;
        }

        private Image GetCardBack()
        {
            Bitmap spriteSheet = Properties.Resources.cards;
            int cardWidth = 48;
            int cardHeight = 64;

            Rectangle srcRect = new Rectangle(0, 4 * cardHeight, cardWidth, cardHeight);
            Bitmap back = new Bitmap(cardWidth, cardHeight);

            using (Graphics g = Graphics.FromImage(back))
                g.DrawImage(spriteSheet, new Rectangle(0, 0, cardWidth, cardHeight), srcRect, GraphicsUnit.Pixel);

            return back;
        }

        private void RenderHandCards(FlowLayoutPanel panel, PlayerHandState handState, bool hideSecondCard = false)
        {
            panel.Controls.Clear();
            for (int i = 0; i < handState.Hand.Cards.Count; i++)
            {
                var pic = CreateCardPictureBox();
                bool hide = hideSecondCard && i == 1;
                pic.Image = hide ? GetCardBack() : GetCardImage(handState.Hand.Cards[i]);
                panel.Controls.Add(pic);
            }
        }

        private void GameForm_Resize(object sender, EventArgs e)
        {
            float scale = Math.Min(Width / baseWidth, Height / baseHeight);
            foreach (Control c in Controls)
                ScaleFontsRecursive(c, scale);

            ScaleCardImages(scale);
        }

        private void ScaleFontsRecursive(Control c, float scale)
        {
            c.Font = new Font(c.Font.FontFamily, Math.Max(8f, 14f * scale), c.Font.Style);
            foreach (Control child in c.Controls)
                ScaleFontsRecursive(child, scale);
        }

        private void ScaleCardImages(float scale)
        {
            foreach (Control c in Controls)
                ScaleCardPicturesRecursive(c, scale);
        }

        private void ScaleCardPicturesRecursive(Control c, float scale)
        {
            if (c is PictureBox pb)
            {
                pb.Width = (int)(100 * scale);
                pb.Height = (int)(150 * scale);
            }

            foreach (Control child in c.Controls)
                ScaleCardPicturesRecursive(child, scale);
        }
    }
}
