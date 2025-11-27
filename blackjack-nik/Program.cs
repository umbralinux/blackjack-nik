using System;
using System.Windows.Forms;
using blackjack_nik;

static class Program
{
    [STAThread]
    static void Main()
    {
        
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new MainMenuForm());
    }
}
