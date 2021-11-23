using HarmonyLib;
using JNNJMods.CrabGameCheat.Modules;
using JNNJMods.CrabGameCheat.Util;
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

        //private MetricsCommunication metrics;

        public void OnApplicationStart(Harmony harmony)
        {
            Instance = this;

            ClassInjector.RegisterTypeInIl2Cpp<Outline>();

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
            gui.AddWindow((int)WindowIDs.OTHER, "Other", 70, 90, 320, 400);
            gui.AddWindow((int)WindowIDs.COMBAT, "Combat", 400, 90, 320, 400);
            gui.AddWindow((int)WindowIDs.MOVEMENT, "Movement", 730, 90, 320, 500);
            gui.AddWindow((int)WindowIDs.PLAYER, "Player", 1060, 90, 320, 400);
            gui.AddWindow((int)WindowIDs.RENDER, "Render", 70, 525, 320, 400);


            //Read Config
            try
            {
                config = Config.FromFile(ConfigPaths.ConfigFile, gui);
            }
            catch (Exception) { }

            if (config == null)
            {
                config = new Config(gui);
            }

            //Create RainbowColor for watermark
            rainbow = new RainbowColor(.2f);
            SceneManager.sceneLoaded = (UnityEngine.Events.UnityAction<Scene, LoadSceneMode>)OnSceneLoaded;
        }

        private async void ConfigSaver()
        {
            while(saveConfig)
            {
                await Task.Delay(5 * 60 * 1000);
                try
                {
                    config.WriteToFile(ConfigPaths.ConfigFile);
                } catch(Exception)
                {
                }
            }
        }

        public void OnApplicationQuit()
        {
            saveConfig = false;
            config.WriteToFile(ConfigPaths.ConfigFile);
            //metrics.Stop();
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

            ConfigSaver();

            /* Metrics disable due to the website being taken down
             metrics = new MetricsCommunication();
            try
            {
                metrics.Start();
            }
            catch (Exception) { }*/
        }

        public void OnUpdate()
        {
            //Hook UIChanger
            UIChanger.OnUpdate();

            //Run Update on every module
            config.ExecuteForModules((ModuleBase m) =>
            {
                m.Update();
            });

            //Hook Update for ClickGUI
            gui.Update();

            //Hide and Show ClickGUI
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                gui.Shown = false;
            }

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
            if (WelcomeScreen.draw)
                WelcomeScreen.OnGUI();

            //Draw ClickGUI
            gui.DrawWindows();

            //Draw WaterMark
            int fontSize = 17;

            DrawingUtil.DrawText(waterMarkText, DrawingUtil.CenteredTextRect(waterMarkText, fontSize).x, 10, fontSize, rainbow.GetColor());

        }
    }
}
