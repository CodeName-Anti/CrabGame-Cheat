using Gameloop.Vdf;
using Gameloop.Vdf.Linq;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net;

namespace CrabGame_Cheat_Installer.Utils
{
    internal static class SteamUtils
    {

        public static string GetAppName(ulong appId)
        {
            using (var client = new WebClient())
            {
                string json = client.DownloadString("http://api.steampowered.com/ISteamApps/GetAppList/v0002/");

                JObject obj = JObject.Parse(json);

                JArray apps = obj.Value<JObject>("applist").Value<JArray>("apps");

                string name = null;

                foreach (var app in apps.Values<JObject>())
                {
                    if (app.Value<ulong>("appid") == appId)
                    {
                        name = app.Value<string>("name");
                        break;
                    }
                }

                return name;
            }
        }

        public static string GetAppLocation(ulong appId, string appName = null)
        {
            string steamInstall = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Valve\Steam", "InstallPath", null) as string;

            VProperty prop = VdfConvert.Deserialize(File.ReadAllText(Path.Combine(steamInstall, "steamapps", "libraryfolders.vdf")));

            string installPath = null;

            for (int i = 0; i < prop.Value.Children().Count(); i++)
            {
                try
                {
                    VToken t = prop.Value[$"{i}"];

                    if (t == null)
                    {
                        break;
                    }

                    VToken apps = t["apps"];

                    if (apps[$"{appId}"] == null)
                        continue;

                    installPath = t.Value<string>("path");

                }
                catch (Exception)
                {
                    break;
                }
            }

            if (installPath != null)
            {
                return Path.Combine(installPath, "steamapps", "common", appName ?? GetAppLocation(appId));
            }

            return null;
        }

    }
}
