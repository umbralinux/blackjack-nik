using System.Drawing;
using System.Windows.Forms;

namespace blackjack_nik
{
    partial class MainMenuForm
    {
        private System.ComponentModel.IContainer components = null;
        private Button btnPractice;
        private Button btnMultiplayer;
        private Button btnSettings;
        private Button btnExit;
        private Label lblTitle;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnPractice = new System.Windows.Forms.Button();
            this.btnMultiplayer = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.layout = new System.Windows.Forms.TableLayoutPanel();
            this.layout.SuspendLayout();
            this.SuspendLayout();

            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 32F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.Gold;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Text = "Blackjack";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // Configure Buttons
            // 
            ConfigureButton(this.btnPractice, "Practice Mode", this.btnPractice_Click);
            ConfigureButton(this.btnMultiplayer, "Multiplayer", this.btnMultiplayer_Click);
            ConfigureButton(this.btnSettings, "Settings", this.btnSettings_Click);
            ConfigureButton(this.btnExit, "Exit", this.btnExit_Click);

            // 
            // layout
            // 
            this.layout.BackColor = System.Drawing.Color.DarkGreen;
            this.layout.ColumnCount = 1;
            this.layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layout.Controls.Add(this.lblTitle, 0, 0);
            this.layout.Controls.Add(this.btnPractice, 0, 1);
            this.layout.Controls.Add(this.btnMultiplayer, 0, 2);
            this.layout.Controls.Add(this.btnSettings, 0, 3);
            this.layout.Controls.Add(this.btnExit, 0, 4);
            this.layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layout.Location = new System.Drawing.Point(0, 0);
            this.layout.Name = "layout";
            this.layout.Padding = new System.Windows.Forms.Padding(100, 40, 100, 40);
            this.layout.RowCount = 5;
            this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.layout.Size = new System.Drawing.Size(800, 600);
            this.layout.TabIndex = 0;

            // 
            // MainMenuForm
            // 
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.layout);
            this.Name = "MainMenuForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Blackjack Menu";
            this.layout.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void ConfigureButton(Button button, string text, System.EventHandler onClick)
        {
            button.Dock = DockStyle.Fill;
            button.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            button.Text = text;
            button.Margin = new Padding(0, 10, 0, 10);
            button.Click += onClick;
        }

        private TableLayoutPanel layout;
    }
}

