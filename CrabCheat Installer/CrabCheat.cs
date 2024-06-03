using Newtonsoft.Json.Linq;
using ShellProgressBar;
using System.Reflection;

namespace CrabGame_Cheat_Installer;

public class CrabCheat
{
	public const string Owner = "CodeName-Anti";
	public const string Repo = "CrabGame-Cheat";

	public const string DllFileName = "CrabCheat.dll";

	public const string ZipFileName = "CrabCheat.zip";

	public static string ApiUrl => GetApiUrl(Owner, Repo);
	public static string DownloadUrl => $"https://github.com/{Owner}/{Repo}/releases/latest/download/{ZipFileName}";

	private static Version cachedVersion = null;

	private static string GetApiUrl(string owner, string repo)
	{
		return $"https://api.github.com/repos/{owner}/{repo}/releases";
	}

	private static async Task InstallBepInExAsync(string path, ProgressBarBase parent)
	{
		// Find newest BepInEx download
		string bepinexDownload = "https://builds.bepinex.dev/projects/bepinex_be/691/BepInEx-Unity.IL2CPP-win-x64-6.0.0-be.691%2B3ba398f.zip";

		await Utils.DownloadAndUnzip(bepinexDownload, path, "BepInEx", parent);
	}

	public static async Task<string> Install(string path)
	{
		ProgressBar progressBar = new(1, "Installing CrabCheat", Utils.BaseProgressOptions);

		if (!File.Exists(Path.Combine(path, "BepInEx", "core", "BepInEx.Core.dll")))
		{
			progressBar.MaxTicks += 1;
			await InstallBepInExAsync(path, progressBar);
		}

		bool ignoreVersionCheck = false;
		bool updateAvailable = await CheckForUpdateAsync(path);

		if (GetInstalledVersion(path) == null)
			ignoreVersionCheck = true;

		if (!updateAvailable && !ignoreVersionCheck)
			return null;

		string crabCheatPath = Path.Combine(path, "BepInEx", "plugins", "CrabCheat");

		Directory.CreateDirectory(crabCheatPath);

		await Utils.DownloadAndUnzip(DownloadUrl, crabCheatPath, "CrabCheat", progressBar);

		progressBar.Dispose();
		Console.WriteLine();

		return updateAvailable ? "updated" : "installed";
	}

	public static void Uninstall(string path)
	{
		if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
			return;

		Utils.DeleteDirectory(path, "BepInEx", true);
		Utils.DeleteDirectory(path, "dotnet", true);

		Utils.DeleteFile(path, "changelog.txt");
		Utils.DeleteFile(path, "winhttp.dll");
		Utils.DeleteFile(path, "doorstop_config.ini");
		Utils.DeleteFile(path, ".doorstop_version");
	}

	public static async Task<Version> GetLatestVersion()
	{
		if (cachedVersion != null)
			return cachedVersion;

		try
		{
			string json = await (await Utils.SendGetAsync(ApiUrl)).Content.ReadAsStringAsync();

			JArray jArr = JArray.Parse(json);

			// Get version tag
			string stringVersion = jArr[0].ToObject<JObject>().GetValue("tag_name").ToObject<string>();

			return cachedVersion = new(stringVersion);
		}
		catch (Exception)
		{
			return null;
		}
	}

	public static Version GetInstalledVersion(string path)
	{
		string filePath = Path.Combine(path, "BepInEx", "plugins", "CrabCheat", DllFileName);

		if (string.IsNullOrEmpty(path) || !File.Exists(filePath))
			return null;

		var ver = AssemblyName.GetAssemblyName(filePath).Version;

		return ver;
	}

	public static async Task<bool> CheckForUpdateAsync(string path)
	{
		Version git = await GetLatestVersion();

		if (git == null)
			return false;

		Version current = GetInstalledVersion(path);

		if (current == null)
			return false;

		int result = current.CompareTo(git);

		return result < 0;
	}
}
