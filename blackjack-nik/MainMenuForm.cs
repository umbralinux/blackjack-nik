using System;
using System.Windows.Forms;

namespace blackjack_nik
{
    public partial class MainMenuForm : Form
    {
        public MainMenuForm()
        {
            InitializeComponent();
        }

        private void btnPractice_Click(object sender, EventArgs e)
        {
            using (var gameForm = new GameForm(GameOptions.FromSettings()))
            {
                gameForm.ShowDialog(this);
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            using (var settingsForm = new SettingsForm())
            {
                settingsForm.ShowDialog(this);
            }
        }

        private void btnMultiplayer_Click(object sender, EventArgs e)
        {
            using (var multiplayerForm = new MultiplayerForm())
            {
                multiplayerForm.ShowDialog(this);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

