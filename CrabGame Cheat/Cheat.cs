﻿using JNNJMods.UI;
using UnityEngine;
using JNNJMods.Render;
using JNNJMods.CrabGameCheat.Util;
using JNNJMods.CrabGameCheat.Modules;
using System.Reflection;

namespace JNNJMods.CrabGameCheat
{
    public class Cheat
    {
        public static Cheat Instance { get; private set; }
        public Config config;
        private ClickGUI gui;
        private RainbowColor rainbow;
        public static string FormattedVersion;

        public void OnApplicationStart(HarmonyLib.Harmony harmony)
        {
            Instance = this;

            FormattedVersion = Utilities.FormatAssemblyVersion(Assembly.GetExecutingAssembly(), true);

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
            gui.AddWindow((int)WindowIDs.PLAYER, "Player", 1100, 90, 320, 400);
            gui.AddWindow((int)WindowIDs.MOVEMENT, "Movement", 745, 90, 320, 400);
            gui.AddWindow((int)WindowIDs.COMBAT, "Combat", 400, 90, 320, 400);
            gui.AddWindow((int)WindowIDs.RENDER, "Render", 70, 525, 320, 400);
            gui.AddWindow((int)WindowIDs.OTHER, "Other", 70, 90, 320, 400);

            //Read Config
            config = Config.FromFile(ConfigPaths.ConfigFile, gui);

            if(config == null)
            {
                config = new Config(gui);
            }

            //Create RainbowColor for watermark
            rainbow = new RainbowColor();
        }

        public void OnApplicationLateStart()
        {
            WelcomeScreen.draw = true;
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
            WelcomeScreen.OnGUI();

            //Draw ClickGUI
            gui.DrawWindows();

            //Draw WaterMark
            if(!gui.Shown)
            {

                string text = "CrabGame Cheat " + FormattedVersion +  " by JNNJ";
                int fontSize = 17;

                Rect centeredRect = DrawingUtil.CenteredTextRect(text, 17);

                DrawingUtil.DrawText(text, centeredRect.x, 10, fontSize, rainbow.GetColor());
            }
        }
    }
}
