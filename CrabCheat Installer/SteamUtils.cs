using Gameloop.Vdf;
using Gameloop.Vdf.Linq;
using Microsoft.Win32;

namespace CrabGame_Cheat_Installer;

public static class SteamUtils
{

	public static string GetAppLocation(ulong appId, string appName)
	{
		// Find steam installation
		string steamInstall = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Valve\Steam", "InstallPath", null) as string;

		// Read steam libraries
		VProperty prop = VdfConvert.Deserialize(File.ReadAllText(Path.Combine(steamInstall, "steamapps", "libraryfolders.vdf")));

		string installPath = null;

		// Loop through all libraries
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

				// Test if app is in path
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
			string path = Path.Combine(installPath, "steamapps", "common", appName);

			if (Directory.Exists(path))
				return path;
		}

		return null;
	}

	public static void StartSteamApp(ulong appId)
	{

	}

}