using System;
using System.Windows.Forms;

namespace blackjack_nik
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            LoadValues();
        }

        private void LoadValues()
        {
            var settings = Properties.Settings.Default;
            nudStarting.Value = settings.StartingBankroll;
            nudMin.Value = settings.MinBet;
            nudMax.Value = settings.MaxBet;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (nudMin.Value > nudMax.Value)
            {
                MessageBox.Show("Minimum bet must be less than or equal to the maximum bet.", "Invalid range",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var settings = Properties.Settings.Default;
            settings.StartingBankroll = nudStarting.Value;
            settings.MinBet = nudMin.Value;
            settings.MaxBet = nudMax.Value;
            settings.Save();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}

