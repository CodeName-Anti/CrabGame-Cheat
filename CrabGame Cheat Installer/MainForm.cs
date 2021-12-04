using CrabGame_Cheat_Installer.Utils;
using Newtonsoft.Json.Linq;
using Ookii.Dialogs.WinForms;
using System;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows.Forms;

namespace CrabGame_Cheat_Installer
{
    public partial class MainForm : Form
    {
        #region Variables
        public const string
            Repository = "CrabGame-Cheat",
            RepoOwner = "CodeName-Anti",
            DownloadURL = $"https://github.com/{RepoOwner}/{Repository}/releases/latest/download/CrabCheat_BepInEx.dll",
            ReleasesAPI = $"https://api.github.com/repos/{RepoOwner}/{Repository}/releases",
            BepInExURL = "https://builds.bepis.io/projects/bepinex_be/521/BepInEx_UnityIL2CPP_x64_8e9f1f3_6.0.0-be.521.zip";

        private string
            _path,
            _pluginFile;

        private string path
        {
            get
            {
                if(string.IsNullOrEmpty(_path))
                {
                    _path = FindGameLocation();
                }

                return _path;
            }
        }

        private string pluginFile
        {
            get
            {

                if(string.IsNullOrEmpty(_pluginFile))
                {
                    string folder = Path.Combine(path, "BepInEx", "plugins");

                    try
                    {
                        _pluginFile = Directory.
                            GetFiles(folder).
                            Where(f => AssemblyName.GetAssemblyName(f).Equals("CrabCheat_BepInEx")).
                            First();
                    }
                    catch (Exception)
                    {
                        Directory.CreateDirectory(folder);

                        _pluginFile = Path.Combine(folder, "CrabCheat_BepInEx.dll");
                    }

                }

                return _pluginFile;
            }
        }
        #endregion

        public MainForm()
        {
            InitializeComponent();

            ChangeButton();

        }

        private void ChangeButton()
        {

            if (UpdateAvailable())
            {
                InstallButton.Text = "Update";
                return;
            }

            InstallButton.Enabled = !IsInstalled();

            if (!InstallButton.Enabled)
            {
                InstallButton.Text = "Already Installed!";
            }
            else
                InstallButton.Text = "Install";
        }

        private static string FindGameLocation()
        {
            string location = SteamUtils.GetAppLocation(1782210, "Crab Game");

            if(string.IsNullOrEmpty(location))
            {
                VistaFolderBrowserDialog dialog = new()
                {
                    Description = "Select Crab Game folder",
                    ShowNewFolderButton = false
                };

                if(dialog.ShowDialog() == DialogResult.OK)
                {
                    location = dialog.SelectedPath;
                }

            }

            return location;
        }

        private bool UpdateAvailable()
        {
            try
            {
                string plugin = pluginFile;

                using var client = new WebClient();
                // Some random user agent because with others it responds with 403
                client.Headers.Add("User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:93.0) Gecko/20100101 Firefox/93.0");
                string json = client.DownloadString(ReleasesAPI);

                JArray jArr = JArray.Parse(json);

                string stringVersion = jArr[0].ToObject<JObject>().GetValue("tag_name").ToObject<string>();

                // Compare GitHub and Local Version
                Version git = new(stringVersion);

                Version current = AssemblyName.GetAssemblyName(plugin).Version;

                int result = current.CompareTo(git);

                return result < 0;
            } catch(Exception)
            {
                return false;
            }
        }

        private void DownloadFile(string url, string file)
        {
            using var client = new WebClient();

            client.DownloadFile(url, file);
        }

        private void DownloadCheat() => DownloadFile(DownloadURL, pluginFile);

        private void InstallBepInEx()
        {
            string zipFile = Path.GetTempFileName();

            DownloadFile(BepInExURL, zipFile);

            ZipFile.ExtractToDirectory(zipFile, path);
        }

        private bool IsInstalled()
        {
            return File.Exists(pluginFile);
        }

        private void InstallButton_Click(object sender, EventArgs e)
        {
            if (!IsInstalled())
            {
                InstallBepInEx();
            }

            DownloadCheat();

            ChangeButton();
        }
    }
}
