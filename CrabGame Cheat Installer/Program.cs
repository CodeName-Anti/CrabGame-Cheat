using System;
using System.Windows.Forms;

namespace CrabGame_Cheat_Installer
{
    internal static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            new MainForm().Show();

            Application.Run();
        }
    }
}
