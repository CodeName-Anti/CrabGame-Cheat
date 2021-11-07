using JNNJMods.UI;
using UnityEngine;
using JNNJMods.Render;
using JNNJMods.CrabGameCheat.Util;
using JNNJMods.CrabGameCheat.Modules;
using System.Reflection;
using UnityEngine.SceneManagement;
using System;

namespace JNNJMods.CrabGameCheat
{
    public class Cheat
    {
        public static Cheat Instance { get; private set; }

        public Config config;

        private ClickGUI gui;

        private RainbowColor rainbow;

        public static string FormattedVersion;

        private string waterMarkText;

        private MetricsCommunication metrics;

        public void OnApplicationStart(HarmonyLib.Harmony harmony)
        {
            Instance = this;

            FormattedVersion = Utilities.FormatAssemblyVersion(Assembly.GetExecutingAssembly(), true);

            waterMarkText = "CrabGame Cheat " + FormattedVersion + " by JNNJ";

            CheatLog.Msg("Loaded CrabGame Cheat " + FormattedVersion + " by JNNJ!");

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
            gui.AddWindow((int)WindowIDs.ITEM_SPAWNER, "Item Spawner", 1445, 90, 320, 700);
            gui.AddWindow((int)WindowIDs.PLAYER, "Player", 1100, 90, 320, 400);
            gui.AddWindow((int)WindowIDs.MOVEMENT, "Movement", 745, 90, 320, 500);
            gui.AddWindow((int)WindowIDs.COMBAT, "Combat", 400, 90, 320, 400);
            gui.AddWindow((int)WindowIDs.RENDER, "Render", 70, 525, 320, 400);
            gui.AddWindow((int)WindowIDs.OTHER, "Other", 70, 90, 320, 400);

            //Read Config
            try
            {
                config = Config.FromFile(ConfigPaths.ConfigFile, gui);
            } catch(Exception) { }

            if (config == null)
            {
                config = new Config(gui);
            }

            //Create RainbowColor for watermark
            rainbow = new RainbowColor(.2f);
            SceneManager.sceneLoaded = (UnityEngine.Events.UnityAction<Scene, LoadSceneMode>)OnSceneLoaded;
        }

        public void OnApplicationQuit()
        {
            metrics.Stop();
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mod)
        {
            var killBounds = UnityEngine.Object.FindObjectOfType<KillPlayerOutOfBounds>();

            if(killBounds != null)
            {
                config.GetModule<AntiBoundKillsModule>().killHeight = killBounds.killHeight;
            }

            if(scene.name.Equals("Menu"))
            {
                UIChanger.Init = false;
            }
        }

        public void OnApplicationLateStart()
        {
            WelcomeScreen.draw = true;

            metrics = new MetricsCommunication();
            metrics.Start();
        }

        public void OnUpdate()
        {
            UIChanger.OnUpdate();

            //Run Update on every module
            config.ExecuteForModules((ModuleBase m) =>
            {
                m.Update();
            });

            //Hook Update for ClickGUI
            gui.Update();
            
            /* Test Code for TileDrive GameMode
             * if you are the Server Owner, it makes the next GameMode TileDrive
            var tileDrive = GameModeManager.Instance.allGameModes.First(r => r.name == "Tile Drive");
            GameLoop.Instance.gameModesAvailable.Clear();
            GameLoop.Instance.gameModesAvailable.Add(tileDrive);

            GameLoop.Instance.smallModes.Clear();
            GameLoop.Instance.smallModes.Add(tileDrive);
            */

            //Hide and Show ClickGUI
            if (Input.GetKeyDown(config.ClickGuiKeyBind) && !gui.keyBindSelection.Shown)
            {
                gui.Shown = !gui.Shown;
            }
        }

        public void OnGUI()
        {
            //Run OnGUI on every Module
            config.ExecuteForModules((ModuleBase m) =>
            {
                m.OnGUI();
            });

            //Hook OnGUI for WelcomeScreen
            if(WelcomeScreen.draw)
                WelcomeScreen.OnGUI();

            //Draw ClickGUI
            gui.DrawWindows();

            //Draw WaterMark
            int fontSize = 17;
            
            DrawingUtil.DrawText(waterMarkText, DrawingUtil.CenteredTextRect(waterMarkText, fontSize).x, 10, fontSize, rainbow.GetColor());
            
        }
    }
}
