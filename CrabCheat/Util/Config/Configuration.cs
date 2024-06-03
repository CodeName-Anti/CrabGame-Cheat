using Newtonsoft.Json;
using System;
using System.IO;

namespace JNNJMods.CrabCheat.Util.Config;

public class Configuration
{
	public static Configuration Instance { get; private set; }

	public bool FirstInit = true;
	[JsonIgnore]
	public bool WasFirstInitThisRun = false;

	public bool CanInitHook = false;
	public bool UseDXOverlay = false;

	private Configuration() { }

	private static Configuration CreateDefaultConfig()
	{
		Configuration config = new();

		config.SaveConfig();

		return config;
	}

	public void SaveConfig()
	{
		File.WriteAllText(ConfigPaths.ConfigurationFile, JsonConvert.SerializeObject(this, Formatting.Indented));
	}

	public static Configuration ReadConfiguration()
	{
		if (!File.Exists(ConfigPaths.ConfigurationFile))
			return Instance = CreateDefaultConfig();

		Configuration config;
		try
		{
			config = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(ConfigPaths.ConfigurationFile));
		}
		catch (Exception)
		{
			config = CreateDefaultConfig();
		}

		Instance = config;

		return Instance;
	}

}
