using System.Drawing;
using System.Windows.Forms;

namespace blackjack_nik
{
    partial class SettingsForm
    {
        private System.ComponentModel.IContainer components = null;
        private NumericUpDown nudStarting;
        private NumericUpDown nudMin;
        private NumericUpDown nudMax;
        private Button btnSave;
        private Button btnCancel;

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
            this.nudStarting = new System.Windows.Forms.NumericUpDown();
            this.nudMin = new System.Windows.Forms.NumericUpDown();
            this.nudMax = new System.Windows.Forms.NumericUpDown();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.nudStarting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMax)).BeginInit();

            var layout = new TableLayoutPanel();
            layout.Dock = DockStyle.Fill;
            layout.ColumnCount = 2;
            layout.RowCount = 5;
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.Padding = new Padding(20);

            AddSettingRow(layout, "Starting Bankroll:", nudStarting, 0);
            AddSettingRow(layout, "Minimum Bet:", nudMin, 1);
            AddSettingRow(layout, "Maximum Bet:", nudMax, 2);

            btnSave.Text = "Save";
            btnSave.Dock = DockStyle.Fill;
            btnSave.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnSave.Click += new System.EventHandler(this.btnSave_Click);

            btnCancel.Text = "Cancel";
            btnCancel.Dock = DockStyle.Fill;
            btnCancel.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            layout.Controls.Add(btnSave, 0, 3);
            layout.Controls.Add(btnCancel, 1, 3);

            ConfigureNumeric(nudStarting, 100, 100000, 100);
            ConfigureNumeric(nudMin, 5, 10000, 5);
            ConfigureNumeric(nudMax, 50, 100000, 25);

            this.Controls.Add(layout);
            this.Text = "Settings";
            this.StartPosition = FormStartPosition.CenterParent;
            this.ClientSize = new Size(500, 350);

            ((System.ComponentModel.ISupportInitialize)(this.nudStarting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMax)).EndInit();
        }

        private void AddSettingRow(TableLayoutPanel layout, string labelText, Control editor, int rowIndex)
        {
            var label = new Label
            {
                Text = labelText,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold)
            };

            editor.Dock = DockStyle.Fill;
            editor.Margin = new Padding(10);

            layout.Controls.Add(label, 0, rowIndex);
            layout.Controls.Add(editor, 1, rowIndex);
        }

        private void ConfigureNumeric(NumericUpDown numeric, decimal min, decimal max, decimal increment)
        {
            numeric.Minimum = min;
            numeric.Maximum = max;
            numeric.Increment = increment;
            numeric.ThousandsSeparator = true;
            numeric.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
        }
    }
}

