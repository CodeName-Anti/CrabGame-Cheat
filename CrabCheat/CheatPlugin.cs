using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using JNNJMods.CrabCheat.Util;
using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using UnityEngine;

namespace JNNJMods.CrabCheat;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class CheatPlugin : BasePlugin
{
	public static CheatPlugin Instance { get; private set; }

	public Harmony HarmonyInstance { get; private set; }

	private static void DownloadJsonLibrary()
	{
		string path = Utilities.GetAssemblyLocation();
		string dllLoadPath = Path.Combine(path, "Newtonsoft.Json.dll");
		if (!File.Exists(dllLoadPath))
		{
			// Define paths
			string tempPath = Path.Combine(Path.GetTempPath(), "CrabCheat " + Guid.NewGuid().ToString());
			string zipPath = Path.Combine(tempPath, "Unzipped");

			string zip = Path.Combine(tempPath, "Json.zip");

			Directory.CreateDirectory(zipPath);

			Utilities.DownloadFile(zip,
				"https://github.com/JamesNK/Newtonsoft.Json/releases/download/13.0.2/Json130r2.zip");

			// Extract to a temp directory
			ZipFile.ExtractToDirectory(zip, zipPath);

			string dllFile = Path.Combine(zipPath, "Bin", "net6.0", "Newtonsoft.Json.dll");

			// Copy to actual directory
			File.Copy(dllFile, dllLoadPath);

			Assembly.LoadFrom(dllLoadPath);
		}
	}


	public override void Load()
	{
		Instance = this;

		// Initialize Harmony
		try
		{
			HarmonyInstance = new Harmony(MyPluginInfo.PLUGIN_GUID);

			TryPatchAll();
		}
		catch (Exception ex)
		{
			CheatLog.Error("Harmony patching error: " + ex.ToString());
		}

		DownloadJsonLibrary();

		// Register Cheat in IL2CPP
		ClassInjector.RegisterTypeInIl2Cpp<Cheat>();
		ClassInjector.RegisterTypeInIl2Cpp<UnityMainThreadDispatcher>();

		// Create GameObject
		GameObject obj = new("JNNJs CrabCheat");
		UnityEngine.Object.DontDestroyOnLoad(obj);
		obj.hideFlags |= HideFlags.HideAndDontSave;


		// Add CheatObject Component
		obj.AddComponent<Cheat>();
		obj.AddComponent<UnityMainThreadDispatcher>();
	}

	private void TryPatchAll()
	{
		// Get all types from current assembly.
		AccessTools.GetTypesFromAssembly(Assembly.GetExecutingAssembly()).Do(type =>
		{
			try
			{
				// Patch type
				new PatchClassProcessor(HarmonyInstance, type).Patch();
			}
			catch (Exception ex)
			{
				CheatLog.Error($"Failed to patch \"{type.FullName}\": {ex}!");
			}
		});
	}

}