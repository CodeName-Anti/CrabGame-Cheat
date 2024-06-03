using Il2CppInterop.Runtime.Injection;
using JNNJMods.CrabCheat.Modules;
using JNNJMods.CrabCheat.Modules.Player;
using JNNJMods.CrabCheat.Rendering;
using JNNJMods.CrabCheat.Rendering.Outline;
using JNNJMods.CrabCheat.Util;
using JNNJMods.CrabCheat.Util.Config;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace JNNJMods.CrabCheat;

public class Cheat(IntPtr handle) : MonoBehaviour(handle)
{
	public static EventSystem EventSys { get; private set; }

	public static Cheat Instance { get; private set; }

	public bool Shown => renderer != null && renderer.RenderGUI;

	public ModuleManager ModuleManager;
	public static Configuration Config => Configuration.Instance;
	public static string FormattedVersion;

	public GUIRenderer renderer;

	private RainbowColor rainbow;
	private string waterMarkText;

	private void Awake()
	{
		Instance = this;

		// Load Outline for ESP
		ClassInjector.RegisterTypeInIl2Cpp<Outline>();

		//Preformat Version, for performance improvements
		FormattedVersion = Utilities.FormatAssemblyVersion(Assembly.GetExecutingAssembly(), true);
		waterMarkText = "CrabCheat " + FormattedVersion + " by JNNJ";

		// Initialize HarmonyFind
		HarmonyFindAttribute.InitPatches();

		// Print out all Methods patched by harmony
		foreach (MethodBase method in CheatPlugin.Instance.HarmonyInstance.GetPatchedMethods())
		{
			CheatLog.Msg($"Patched: {method.DeclaringType.FullName}.{method.Name}");
		}

		AntiCheat.StopAntiCheat();

		Configuration.ReadConfiguration();

		ModuleManager = new ModuleManager();

		try
		{
			// Create RainbowColor for watermark
			rainbow = new RainbowColor(.2f);

			// Add SceneLoaded Listener
			SceneManager.sceneLoaded += (UnityEngine.Events.UnityAction<Scene, LoadSceneMode>)OnSceneLoaded;
		}
		catch (Exception)
		{
			// Ignored, not very high priority
		}

		LogLoadedMessage();
	}

	private static void LogLoadedMessage()
	{
		string loaded = $"Loaded CrabCheat {FormattedVersion} by JNNJ!";
		string githubURL = $"GitHub URL: https://github.com/{Constants.Owner}/{Constants.Repository}";

		int lenght = (loaded.Length > githubURL.Length ? loaded.Length : githubURL.Length) + 1;

		string placeHolder = "";

		for (int i = 0; i < lenght; i++)
			placeHolder += '=';

		CheatLog.Error(placeHolder);

		CheatLog.Msg(loaded);
		CheatLog.Msg(githubURL);

		CheatLog.Error(placeHolder);
	}

	public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		// Reset KillBounds
		ModuleManager.GetModule<AntiBoundKillsModule>().killHeight = -69420187;

		// Redo UIChanger
		if (scene.name.Equals("Menu"))
		{
			UIChanger.Init = false;
		}
	}

	private void Start()
	{
		Config.WasFirstInitThisRun = Config.FirstInit;
		Config.FirstInit = false;
		Config.SaveConfig();

		// Destroy AntiCheat GameObject
		AntiCheat.LateStopAntiCheat();

		WelcomeScreen.Draw = true;
		WelcomeScreen.OnClose += () =>
		{
			renderer = new();
			renderer.Initialize();
		};
	}

	private void OnApplicationQuit()
	{
		renderer.Shutdown();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Update()
	{
		// Change UI
		UIChanger.OnUpdate();

		// Run Update on every module
		ModuleManager.ExecuteForModules(m => m.Update());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void FixedUpdate()
	{
		// Run FixedUpdate on every module
		ModuleManager.ExecuteForModules(m => m.FixedUpdate());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void OnGUI()
	{
		if (renderer != null && renderer.RenderGUI)
		{
			DrawingUtil.DrawFullScreenColor(new Color(0, 0, 0, 0.7f));
		}

		// Run OnGUI on every Module
		ModuleManager.ExecuteForModules(m => m.OnGUI());

		// Run OnGUI for WelcomeScreen
		if (WelcomeScreen.Draw)
			WelcomeScreen.OnGUI();

		// Draw WaterMark
		int fontSize = 17;

		DrawingUtil.DrawText(waterMarkText,
			// X
			DrawingUtil.CenteredTextRect(waterMarkText, fontSize).x,
			// Y
			10,
			// FontSize
			fontSize,
			// Color
			rainbow != null ? rainbow.GetColor() : Color.red
		);
	}
}
