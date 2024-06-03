using BepInEx;
using System.IO;

namespace JNNJMods.CrabCheat.Util.Config;

public class ConfigPaths
{
	public static readonly string ConfigDirectory = Paths.ConfigPath;

	public static readonly string ConfigurationFile = Path.Combine(ConfigDirectory, "CrabCheat.json");

}
