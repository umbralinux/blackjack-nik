using System.Drawing;
using System.Windows.Forms;

namespace blackjack_nik
{
    partial class MultiplayerForm
    {
        private System.ComponentModel.IContainer components = null;
        private ListView lvServers;
        private ColumnHeader colName;
        private ColumnHeader colRegion;
        private ColumnHeader colStatus;
        private ColumnHeader colPlayers;
        private Button btnConnect;
        private Button btnRefresh;
        private Button btnClose;
        private Button btnHost;
        private Label lblDetails;

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
            this.lvServers = new System.Windows.Forms.ListView();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colRegion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPlayers = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblDetails = new System.Windows.Forms.Label();
            this.btnHost = new System.Windows.Forms.Button();
            this.SuspendLayout();

            this.lvServers.Dock = DockStyle.Top;
            this.lvServers.View = View.Details;
            this.lvServers.FullRowSelect = true;
            this.lvServers.HideSelection = false;
            this.lvServers.MultiSelect = false;
            this.lvServers.Columns.AddRange(new ColumnHeader[] { this.colName, this.colRegion, this.colStatus, this.colPlayers });
            this.lvServers.Height = 300;
            this.lvServers.SelectedIndexChanged += new System.EventHandler(this.lvServers_SelectedIndexChanged);

            this.colName.Text = "Server";
            this.colName.Width = 220;

            this.colRegion.Text = "Region";
            this.colRegion.Width = 140;

            this.colStatus.Text = "Status";
            this.colStatus.Width = 140;

            this.colPlayers.Text = "Players";
            this.colPlayers.Width = 100;

            this.lblDetails.Dock = DockStyle.Top;
            this.lblDetails.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            this.lblDetails.ForeColor = Color.White;
            this.lblDetails.BackColor = Color.FromArgb(20, 20, 20);
            this.lblDetails.Padding = new Padding(10);
            this.lblDetails.Height = 140;
            this.lblDetails.Text = "Select a server to view details.";

            var buttonsPanel = new TableLayoutPanel();
            buttonsPanel.Dock = DockStyle.Bottom;
            buttonsPanel.ColumnCount = 4;
            buttonsPanel.RowCount = 1;
            buttonsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            buttonsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            buttonsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            buttonsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            buttonsPanel.Height = 60;
            buttonsPanel.Padding = new Padding(0, 10, 0, 0);

            this.btnHost.Text = "Host Game";
            this.btnHost.Dock = DockStyle.Fill;
            this.btnHost.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnHost.Click += new System.EventHandler(this.btnHost_Click);

            this.btnConnect.Text = "Connect";
            this.btnConnect.Dock = DockStyle.Fill;
            this.btnConnect.Enabled = false;
            this.btnConnect.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);

            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Dock = DockStyle.Fill;
            this.btnRefresh.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            this.btnClose.Text = "Close";
            this.btnClose.Dock = DockStyle.Fill;
            this.btnClose.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            this.btnClose.Click += (s, e) => Close();

            buttonsPanel.Controls.Add(this.btnHost, 0, 0);
            buttonsPanel.Controls.Add(this.btnConnect, 1, 0);
            buttonsPanel.Controls.Add(this.btnRefresh, 2, 0);
            buttonsPanel.Controls.Add(this.btnClose, 3, 0);

            this.BackColor = Color.FromArgb(32, 32, 32);
            this.ClientSize = new Size(800, 520);
            this.Controls.Add(buttonsPanel);
            this.Controls.Add(this.lblDetails);
            this.Controls.Add(this.lvServers);
            this.Text = "Multiplayer Servers";
            this.StartPosition = FormStartPosition.CenterParent;
            this.ResumeLayout(false);
        }
    }
}

