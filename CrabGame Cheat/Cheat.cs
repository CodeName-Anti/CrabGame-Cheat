using JNNJMods.UI;
using UnityEngine;
using MelonLoader;
using JNNJMods.Render;
using JNNJMods.CrabGameCheat.Util;
using JNNJMods.CrabGameCheat.Modules;

namespace JNNJMods.CrabGameCheat
{
    public class Cheat : MelonMod
    {
        public static Cheat Instance { get; private set; }
        public Config config;
        private ClickGUI gui;
        private RainbowColor rainbow;

        public override void OnApplicationStart()
        {
            Instance = this;

            AntiCheat.StopAntiCheat();

            //Create ClickGUI
            gui = new ClickGUI(10, 40, 10)
            {
                BlackOut = true
            };

            //Add Main Window
            gui.AddWindow((int)WindowIDs.MAIN, "JNNJ's CrabGame Cheat", 100, 100, 320, 400); //250

            //Read Config
            config = Config.FromFile(ConfigPaths.ConfigFile, gui);

            if(config == null)
            {
                config = new Config(gui);
            }

            //Create RainbowColor for watermark
            rainbow = new RainbowColor();
        }

        public override void OnApplicationLateStart()
        {
            WelcomeScreen.draw = true;
        }

        public override void OnUpdate()
        {
            //Run Update on every module
            config.ExecuteForModules((ModuleBase m) =>
            {
                m.Update();
            });

            //Hook Update for ClickGUI
            gui.Update();

            //Hide and Show ClickGUI
            if (Input.GetKeyDown(config.ClickGuiKeyBind))
            {
                gui.Shown = !gui.Shown;
            }
        }

        public override void OnGUI()
        {
            //Run OnGUI on every Module
            config.ExecuteForModules((ModuleBase m) =>
            {
                m.OnGUI();
            });

            //Hook OnGUI for WelcomeScreen
            WelcomeScreen.OnGUI();

            //Draw ClickGUI
            gui.DrawWindows();

            //Draw WaterMark
            if(!gui.Shown)
            {

                string text = "CrabGame Cheat by JNNJ";

#if DEBUG
                text += " (DEBUG VERSION, DO NOT SHARE)";
#endif

                int fontSize = 17;

                Rect centeredRect = DrawingUtil.CenteredTextRect(text, 17);

                DrawingUtil.DrawText(text, centeredRect.x, 10, fontSize, rainbow.GetColor());
            }
        }
    }
}
