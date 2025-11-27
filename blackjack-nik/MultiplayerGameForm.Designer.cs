using System;
using System.Drawing;
using System.Windows.Forms;

namespace blackjack_nik
{
    partial class MultiplayerGameForm
    {
        
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblDealer;
        private System.Windows.Forms.FlowLayoutPanel pnlDealer;
        private System.Windows.Forms.Label lblPrimaryHandTitle;
        private System.Windows.Forms.FlowLayoutPanel pnlPrimaryHand;
        private System.Windows.Forms.Label lblSplitHandTitle;
        private System.Windows.Forms.FlowLayoutPanel pnlSplitHands;
        private System.Windows.Forms.TableLayoutPanel splitSection;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button btnHit;
        private System.Windows.Forms.Button btnStand;
        private System.Windows.Forms.Button btnDouble;
        private System.Windows.Forms.Button btnSplit;
        private System.Windows.Forms.Label lblBalance;
        private System.Windows.Forms.Label lblActiveBet;
        private System.Windows.Forms.Label lblActiveHand;
        private System.Windows.Forms.Label lblPlayerScore;
        private System.Windows.Forms.NumericUpDown nudBet;
        private System.Windows.Forms.Button btnDeal;
        private System.Windows.Forms.Button btnResetBankroll;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblBalance = new System.Windows.Forms.Label();
            this.lblActiveBet = new System.Windows.Forms.Label();
            this.lblActiveHand = new System.Windows.Forms.Label();
            this.lblPlayerScore = new System.Windows.Forms.Label();
            this.nudBet = new System.Windows.Forms.NumericUpDown();
            this.btnDeal = new System.Windows.Forms.Button();
            this.lblDealer = new System.Windows.Forms.Label();
            this.pnlDealer = new System.Windows.Forms.FlowLayoutPanel();
            this.lblPrimaryHandTitle = new System.Windows.Forms.Label();
            this.pnlPrimaryHand = new System.Windows.Forms.FlowLayoutPanel();
            this.lblSplitHandTitle = new System.Windows.Forms.Label();
            this.pnlSplitHands = new System.Windows.Forms.FlowLayoutPanel();
            this.lblMessage = new System.Windows.Forms.Label();
            this.btnHit = new System.Windows.Forms.Button();
            this.btnStand = new System.Windows.Forms.Button();
            this.btnDouble = new System.Windows.Forms.Button();
            this.btnSplit = new System.Windows.Forms.Button();
            this.btnResetBankroll = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudBet)).BeginInit();

            // --- Root Layout ---
            var root = new TableLayoutPanel();
            root.Dock = DockStyle.Fill;
            root.RowCount = 7;
            root.ColumnCount = 1;
            root.BackColor = Color.DarkGreen;
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Info row
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Bet row
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 25f)); // Dealer
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 25f)); // Split player
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 35f)); // Primary player
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Message area
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Buttons row

            // --- Info row ---
            var infoPanel = new TableLayoutPanel();
            infoPanel.Dock = DockStyle.Fill;
            infoPanel.ColumnCount = 4;
            infoPanel.RowCount = 1;
            infoPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            infoPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            infoPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            infoPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));

            lblBalance.Dock = DockStyle.Fill;
            lblBalance.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblBalance.ForeColor = Color.White;
            lblBalance.TextAlign = ContentAlignment.MiddleLeft;

            lblActiveBet.Dock = DockStyle.Fill;
            lblActiveBet.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblActiveBet.ForeColor = Color.White;
            lblActiveBet.TextAlign = ContentAlignment.MiddleCenter;

            lblActiveHand.Dock = DockStyle.Fill;
            lblActiveHand.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblActiveHand.ForeColor = Color.White;
            lblActiveHand.TextAlign = ContentAlignment.MiddleRight;

            lblPlayerScore = new System.Windows.Forms.Label();
            lblPlayerScore.Dock = DockStyle.Fill;
            lblPlayerScore.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblPlayerScore.ForeColor = Color.White;
            lblPlayerScore.TextAlign = ContentAlignment.MiddleRight;

            infoPanel.Controls.Add(lblBalance, 0, 0);
            infoPanel.Controls.Add(lblActiveBet, 1, 0);
            infoPanel.Controls.Add(lblActiveHand, 2, 0);
            infoPanel.Controls.Add(lblPlayerScore, 3, 0);

            // --- Bet row ---
            var betPanel = new TableLayoutPanel();
            betPanel.Dock = DockStyle.Fill;
            betPanel.ColumnCount = 4;
            betPanel.RowCount = 1;
            betPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            betPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40f));
            betPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30f));
            betPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30f));

            var lblBetPrompt = new Label();
            lblBetPrompt.Text = "Bet:";
            lblBetPrompt.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblBetPrompt.ForeColor = Color.White;
            lblBetPrompt.Dock = DockStyle.Fill;
            lblBetPrompt.TextAlign = ContentAlignment.MiddleLeft;

            nudBet.Dock = DockStyle.Fill;
            nudBet.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            nudBet.Minimum = 10;
            nudBet.Maximum = 5000;
            nudBet.Increment = 5;
            nudBet.BorderStyle = BorderStyle.FixedSingle;
            nudBet.ThousandsSeparator = true;

            btnDeal.Dock = DockStyle.Fill;
            btnDeal.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnDeal.Text = "Deal";
            btnDeal.Click += new System.EventHandler(this.btnDeal_Click);

            btnResetBankroll.Dock = DockStyle.Fill;
            btnResetBankroll.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnResetBankroll.Text = "Reset Bankroll";
            btnResetBankroll.Click += new System.EventHandler(this.btnResetBankroll_Click);

            betPanel.Controls.Add(lblBetPrompt, 0, 0);
            betPanel.Controls.Add(nudBet, 1, 0);
            betPanel.Controls.Add(btnDeal, 2, 0);
            betPanel.Controls.Add(btnResetBankroll, 3, 0);

            // --- Dealer section ---
            var dealerSection = new TableLayoutPanel();
            dealerSection.Dock = DockStyle.Fill;
            dealerSection.RowCount = 2;
            dealerSection.ColumnCount = 1;
            dealerSection.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            dealerSection.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            lblDealer.Dock = DockStyle.Top;
            lblDealer.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblDealer.ForeColor = Color.White;
            lblDealer.Text = "Dealer:";
            lblDealer.Height = 30;


            pnlDealer.Dock = DockStyle.Fill;
            pnlDealer.AutoScroll = true;
            pnlDealer.BorderStyle = BorderStyle.FixedSingle;
            pnlDealer.WrapContents = false;
            pnlDealer.FlowDirection = FlowDirection.LeftToRight;
            pnlDealer.Padding = new Padding(10);
            

            dealerSection.Controls.Add(lblDealer, 0, 0);
            dealerSection.Controls.Add(pnlDealer, 0, 1);

            // --- Primary player section ---
            var primarySection = new TableLayoutPanel();
            primarySection.Dock = DockStyle.Fill;
            primarySection.RowCount = 2;
            primarySection.ColumnCount = 1;
            primarySection.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            primarySection.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            lblPrimaryHandTitle.Dock = DockStyle.Top;
            lblPrimaryHandTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblPrimaryHandTitle.ForeColor = Color.White;
            lblPrimaryHandTitle.Text = "Player - First Hand";
            lblPrimaryHandTitle.Height = 30;

            pnlPrimaryHand.Dock = DockStyle.Fill;
            pnlPrimaryHand.AutoScroll = true;
            pnlPrimaryHand.BorderStyle = BorderStyle.FixedSingle;
            pnlPrimaryHand.WrapContents = true;
            pnlPrimaryHand.FlowDirection = FlowDirection.LeftToRight;
            pnlPrimaryHand.Padding = new Padding(10);

            primarySection.Controls.Add(lblPrimaryHandTitle, 0, 0);
            primarySection.Controls.Add(pnlPrimaryHand, 0, 1);

            // --- Split player section ---
            var splitSection = new TableLayoutPanel();
            splitSection.Dock = DockStyle.Fill;
            splitSection.RowCount = 2;
            splitSection.ColumnCount = 1;
            splitSection.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            splitSection.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            lblSplitHandTitle.Dock = DockStyle.Top;
            lblSplitHandTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblSplitHandTitle.ForeColor = Color.White;
            lblSplitHandTitle.Text = "Split Hands";
            lblSplitHandTitle.Height = 30;

            pnlSplitHands.Dock = DockStyle.Fill;
            pnlSplitHands.AutoScroll = true;
            pnlSplitHands.BorderStyle = BorderStyle.FixedSingle;
            pnlSplitHands.WrapContents = true;
            pnlSplitHands.FlowDirection = FlowDirection.LeftToRight;
            pnlSplitHands.Padding = new Padding(10);

            splitSection.Controls.Add(lblSplitHandTitle, 0, 0);
            splitSection.Controls.Add(pnlSplitHands, 0, 1);
            splitSection.Visible = false;

            // --- Message label ---
            lblMessage.Dock = DockStyle.Fill;
            lblMessage.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblMessage.ForeColor = Color.Yellow;
            lblMessage.TextAlign = ContentAlignment.MiddleCenter;

            // --- Buttons ---
            var buttonPanel = new TableLayoutPanel();
            buttonPanel.Dock = DockStyle.Fill;
            buttonPanel.ColumnCount = 4;
            buttonPanel.RowCount = 1;
            buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));

            // Hit
            btnHit.Dock = DockStyle.Fill;
            btnHit.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnHit.Text = "Hit";
            btnHit.Click += new System.EventHandler(this.btnHit_Click);

            // Stand
            btnStand.Dock = DockStyle.Fill;
            btnStand.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnStand.Text = "Stand";
            btnStand.Click += new System.EventHandler(this.btnStand_Click);

            // Double
            btnDouble.Dock = DockStyle.Fill;
            btnDouble.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnDouble.Text = "Double";
            btnDouble.Click += new System.EventHandler(this.btnDoubleDown_Click);

            // Split
            btnSplit.Dock = DockStyle.Fill;
            btnSplit.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnSplit.Text = "Split";
            btnSplit.Click += new System.EventHandler(this.btnSplit_Click);

            buttonPanel.Controls.Add(btnHit, 0, 0);
            buttonPanel.Controls.Add(btnStand, 1, 0);
            buttonPanel.Controls.Add(btnDouble, 2, 0);
            buttonPanel.Controls.Add(btnSplit, 3, 0);

            // --- Add everything ---
            root.Controls.Add(infoPanel, 0, 0);
            root.Controls.Add(betPanel, 0, 1);
            root.Controls.Add(dealerSection, 0, 2);
            root.Controls.Add(splitSection, 0, 3);
            root.Controls.Add(primarySection, 0, 4);
            root.Controls.Add(lblMessage, 0, 5);
            root.Controls.Add(buttonPanel, 0, 6);

            this.splitSection = splitSection;

            // --- Form setup ---
            this.Controls.Add(root);
            this.Text = "Multiplayer Blackjack";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.DarkGreen;
            this.ClientSize = new Size(1280, 720);
            this.AutoScaleMode = AutoScaleMode.Dpi;
            ((System.ComponentModel.ISupportInitialize)(this.nudBet)).EndInit();
        }
        #endregion
    }
}
