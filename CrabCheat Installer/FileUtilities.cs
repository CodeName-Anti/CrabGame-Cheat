using Ookii.Dialogs.WinForms;
using System.IO;
using System.Windows.Forms;

namespace CrabGame_Cheat_Installer
{
    public static class FileUtilities
    {

        private static string BrowseGame()
        {
            // Create BrowseFolder Dialog
            VistaFolderBrowserDialog dialog = new()
            {
                Description = "Select Crab Game folder",
                ShowNewFolderButton = false
            };

            // Show dialog and return Path
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return dialog.SelectedPath;
            }

            return null;
        }

        public static string FindGameLocation()
        {
            // Try to get the Game Folder through steam
            string location = SteamUtils.GetAppLocation(1782210, "Crab Game");

            // Fallback to browse the folder
            if (string.IsNullOrEmpty(location) || !File.Exists(Path.Combine(location, "Crab Game.exe")))
            {
                location = BrowseGame();
            }

            return location;
        }

        public static void CopyDir(string sourceFolder, string destFolder)
        {
            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);

            // Get Files & Copy
            string[] files = Directory.GetFiles(sourceFolder);
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);

                // ADD Unique File Name Check to Below!!!!
                string dest = Path.Combine(destFolder, name);
                File.Copy(file, dest, true);
            }

            // Get dirs recursively and copy files
            string[] folders = Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders)
            {
                string name = Path.GetFileName(folder);
                string dest = Path.Combine(destFolder, name);
                CopyDir(folder, dest);
            }
        }

    }
}
