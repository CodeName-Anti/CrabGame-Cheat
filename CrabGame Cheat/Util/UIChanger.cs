using JNNJMods.CrabGameCheat.Translators;
using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace JNNJMods.CrabGameCheat.Util
{
    public class UIChanger
    {
        public static bool Init
        {
            get
            {
                return init;
            }
            set
            {
                init = value;
                versionUIInit = value;
                aboutUIInit = value;
            }
        }


        private static bool
            init,
            versionUIInit,
            aboutUIInit;

        public static void OnUpdate()
        {
            if (init)
                return;

            if (!versionUIInit)
            {
                try
                {
                    string gameVersion = Instances.VersionUI.versionText.text;
                    Instances.VersionUI.versionText.text = "CrabGame Cheat " + Cheat.FormattedVersion + " by JNNJ Game Version " + gameVersion;
                    versionUIInit = true;
                    CheatLog.Msg("Version Changed");
                }
                catch (Exception) { }
            }

            if (!aboutUIInit)
            {
                try
                {
                    var ui = Instances.MenuUI;

                    GameObject creditsWindow = ui.gameObject.GetChildren().Where(obj => obj.name.Contains("Credits")).First();

                    GameObject tab0 = creditsWindow.GetChildren().Where(obj => obj.name.Contains("Tab")).First();

                    GameObject content = tab0.GetChildren().First().GetChildren().Where(obj => obj.name.Contains("Content")).First();

                    TextMeshProUGUI textMesh = content.GetChildren().Where(obj => obj.name.Contains("Text")).First().GetComponent<TextMeshProUGUI>();

                    textMesh.text +=
                        "<br><br>" +
                        "<size=150%>CrabGame Cheat </size>is a Cheat made by JNNJ.";

                    CheatLog.Msg("Credits Changed");

                    aboutUIInit = true;
                }
                catch (Exception) { }
            }

            if (versionUIInit && aboutUIInit)
            {
                init = true;
                CheatLog.Msg("UI Updated!");
            }
        }

        private static void DiscordButton_onClick()
        {
            Application.OpenURL("https://discord.gg/UUhXmhPWfq");
        }

    }
}
