using HarmonyLib;
using JNNJMods.CrabGameCheat.Modules;
using JNNJMods.CrabGameCheat.Util;
using JNNJMods.CrabGameCheat.Util.KeyBinds;
using JNNJMods.Render;
using JNNJMods.UI;
using System;
using System.Reflection;
using System.Threading.Tasks;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JNNJMods.CrabGameCheat
{
    public class Cheat
    {
        public static Cheat Instance { get; private set; }

        public Config config;
        private bool saveConfig = true;

        private ClickGUI gui;

        private RainbowColor rainbow;

        public static string FormattedVersion;

        private string waterMarkText;

        public void OnApplicationStart(Harmony harmony)
        {
            Instance = this;

            //Load Outline for ESP
            ClassInjector.RegisterTypeInIl2Cpp<Outline>();

            //Preformat Version, for performance improvements
            FormattedVersion = Utilities.FormatAssemblyVersion(Assembly.GetExecutingAssembly(), true);

            waterMarkText = "CrabGame Cheat " + FormattedVersion + " by JNNJ";

            CheatLog.Msg("Loaded CrabGame Cheat " + FormattedVersion + " by JNNJ!");

            //Initialize HarmonyFind
            HarmonyFindAttribute.InitPatches();

            //Print out all Methods patched by harmony
            foreach (MethodBase b in harmony.GetPatchedMethods())
            {
                CheatLog.Msg("Class: " + b.DeclaringType.FullName + " Method: " + b.Name);
            }
            
            AntiCheat.StopAntiCheat();

            //Create ClickGUI
            gui = new ClickGUI(10, 40, 10)
            {
                BlackOut = true
            };

            //Add Windows
            gui.AddWindow((int)WindowIDs.Other, "Other", 70, 90, 320, 400);
            gui.AddWindow((int)WindowIDs.Combat, "Combat", 400, 90, 320, 400);
            gui.AddWindow((int)WindowIDs.Movement, "Movement", 730, 90, 320, 500);
            gui.AddWindow((int)WindowIDs.Player, "Player", 1060, 90, 320, 400);
            gui.AddWindow((int)WindowIDs.Render, "Render", 70, 525, 320, 400);
            gui.AddWindow((int)WindowIDs.LobbyOwner, "Owner related", 400, 525, 320, 400);

            ClickGUI.Instance.GetWindow((int)WindowIDs.LobbyOwner).Visible = false;

            config = new Config(gui);

            //Create RainbowColor for watermark
            rainbow = new RainbowColor(.2f);

            SceneManager.sceneLoaded = (UnityEngine.Events.UnityAction<Scene, LoadSceneMode>)OnSceneLoaded;
        }

        /// <summary>
        /// Saves Config every 2 minutes.
        /// </summary>
        private async void KeyBindSaver()
        {
            while(saveConfig)
            {
                await Task.Delay(2 * 60 * 1000);
                try
                {
                    KeyBindManager.Instance.WriteToFile(ConfigPaths.KeyBindFile);
                } catch(Exception)
                {
                }
            }
        }

        public void OnApplicationQuit()
        {
            saveConfig = false;
            KeyBindManager.Instance.WriteToFile(ConfigPaths.KeyBindFile);
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            //reset KillBounds
            config.GetModule<AntiBoundKillsModule>().killHeight = -69420187;

            //Redo UIChanger
            if (scene.name.Equals("Menu"))
            {
                UIChanger.Init = false;
            }
        }

        public void OnApplicationLateStart()
        {
            //Destroy AntiCheat GameObject
            AntiCheat.LateStopAntiCheat();
            WelcomeScreen.draw = true;

            KeyBindManager.ReadFromFile(ConfigPaths.KeyBindFile);

            KeyBindSaver();
        }

        public void OnUpdate()
        {
            //Hide and Show ClickGUI
            if (Input.GetKeyDown(KeyCode.Escape) && gui.Shown)
            {
                gui.Shown = false;
            }

            if (Input.GetKeyDown(config.ClickGuiKeyBind) && !gui.keyBindSelection.Shown)
            {
                gui.Shown = !gui.Shown;
            }

            //Hook UIChanger
            UIChanger.OnUpdate();

            //Run Update on every module
            config.ExecuteForModules(m => m.Update());

            //Hook Update for ClickGUI
            gui.Update();

        }

        public void FixedUpdate()
        {
            config.ExecuteForModules(m => m.FixedUpdate());
        }

        public void OnGUI()
        {
            //Draw ClickGUI
            gui.DrawWindows();

            //Run OnGUI on every Module
            config.ExecuteForModules(m => m.OnGUI());

            //Hook OnGUI for WelcomeScreen
            if (WelcomeScreen.draw)
                WelcomeScreen.OnGUI();

            int fontSize = 17;

            //Draw WaterMark
            DrawingUtil.DrawText(waterMarkText, DrawingUtil.CenteredTextRect(waterMarkText, fontSize).x, 10, fontSize, rainbow.GetColor());
        }
    }
}
