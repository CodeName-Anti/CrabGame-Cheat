using JNNJMods.CrabGameCheat.Loader;
using JNNJMods.CrabGameCheat.Modules;
using JNNJMods.CrabGameCheat.Util;
using JNNJMods.CrabGameCheat.Util.KeyBinds;
using JNNJMods.Render;
using JNNJMods.UI;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JNNJMods.CrabGameCheat
{
    public class Cheat : MonoBehaviour
    {
        public static Cheat Instance { get; private set; }

        public Config config;
        private bool saveConfig = true;

        private ClickGUI gui;

        private RainbowColor rainbow;

        public static string FormattedVersion;

        private string waterMarkText;

        public Cheat(IntPtr handle) : base(handle) { }

        public void OnLoad()
        {
            Instance = this;

            // Load Outline for ESP
            ClassInjector.RegisterTypeInIl2Cpp<Outline>();

            //Preformat Version, for performance improvements
            FormattedVersion = Utilities.FormatAssemblyVersion(Assembly.GetExecutingAssembly(), true);

            waterMarkText = "CrabGame Cheat " + FormattedVersion + " by JNNJ";

            CheatLog.Msg("Loaded CrabGame Cheat " + FormattedVersion + " by JNNJ!");

            // Initialize HarmonyFind
            HarmonyFindAttribute.InitPatches();

            // Print out all Methods patched by harmony
            foreach (var method in BepInExLoader.Instance.HarmonyInstance.GetPatchedMethods())
            {
                CheatLog.Msg($"Patched: {method.DeclaringType.FullName}.{method.Name}");
            }

            AntiCheat.StopAntiCheat();

            // Create ClickGUI
            gui = new ClickGUI(10, 40, 10)
            {
                BlackOut = true
            };

            // Add Windows
            gui.AddWindow((int)WindowIDs.Other, "Other", 70, 90, 320, 400);
            gui.AddWindow((int)WindowIDs.Combat, "Combat", 400, 90, 320, 400);
            gui.AddWindow((int)WindowIDs.Movement, "Movement", 730, 90, 320, 500);
            gui.AddWindow((int)WindowIDs.Player, "Player", 1060, 90, 320, 400);
            gui.AddWindow((int)WindowIDs.Render, "Render", 70, 525, 320, 400);
            gui.AddWindow((int)WindowIDs.LobbyOwner, "Owner related", 400, 525, 320, 400);

            ClickGUI.Instance.GetWindow((int)WindowIDs.LobbyOwner).Visible = false;

            config = new Config(gui);

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
            string loaded = $"Loaded CrabGame Cheat {FormattedVersion} by JNNJ!";
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

        /// <summary>
        /// Saves KeyBinds every 2 minutes.
        /// </summary>
        private async void KeyBindSaver()
        {
            while (saveConfig)
            {
                // Delay for 2 minutes
                await Task.Delay(120000);
                try
                {
                    // Save KeyBinds
                    KeyBindManager.Instance.WriteToFile(ConfigPaths.KeyBindFile);
                }
                catch (Exception)
                {
                }
            }
        }

        private void OnApplicationQuit()
        {
            // Disable KeyBindSave Loop
            saveConfig = false;
            // Save KeyBinds
            KeyBindManager.Instance.WriteToFile(ConfigPaths.KeyBindFile);
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Reset KillBounds
            config.GetModule<AntiBoundKillsModule>().killHeight = -69420187;

            // Redo UIChanger
            if (scene.name.Equals("Menu"))
            {
                UIChanger.Init = false;
            }
        }

        private void Start()
        {
            // Destroy AntiCheat GameObject
            AntiCheat.LateStopAntiCheat();
            WelcomeScreen.draw = true;

            KeyBindManager.ReadFromFile(ConfigPaths.KeyBindFile);

            KeyBindSaver();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Update()
        {
            // Hide and Show ClickGUI
            if (Input.GetKeyDown(KeyCode.Escape) && gui.Shown)
            {
                gui.Shown = false;
            }

            if (Input.GetKeyDown(config.ClickGuiKeyBind) && !gui.keyBindSelection.Shown)
            {
                gui.Shown = !gui.Shown;
            }

            // Change UI
            UIChanger.OnUpdate();

            // Run Update on every module
            config.ExecuteForModules(m => m.Update());

            // Run Update for ClickGUI
            gui.Update();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void FixedUpdate()
        {
            // Run FixedUpdate on every module
            config.ExecuteForModules(m => m.FixedUpdate());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnGUI()
        {
            // Draw ClickGUI
            gui.DrawWindows();

            // Run OnGUI on every Module
            config.ExecuteForModules(m => m.OnGUI());

            // Run OnGUI for WelcomeScreen
            if (WelcomeScreen.draw)
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
}
