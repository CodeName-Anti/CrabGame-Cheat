using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace JNNJMods.CrabGameCheat.Util
{
    public class UIChanger
    {
        private static bool init,
            versionUIInit,
            aboutUIInit;

        public static void OnUpdate()
        {
            if (init)
                return;

            if(!versionUIInit)
            {
                try
                {
                    string gameVersion = VersionUI.Instance.versionText.text;
                    VersionUI.Instance.versionText.text = "CrabGame Cheat " + Cheat.FormattedVersion + " by JNNJ Game Version " + gameVersion;
                    versionUIInit = true;
                    CheatLog.Msg("Version Changed");
                }
                catch (Exception) { }
            }

            if(!aboutUIInit)
            {
                try
                {
                    MenuUI ui = MenuUI.Instance;

                    GameObject creditsWindow = ui.gameObject.GetChildren().Where(obj => obj.name.Contains("Credits")).First();

                    GameObject tab0 = creditsWindow.gameObject.GetChildren().Where(obj => obj.name.Contains("Tab")).First();

                    GameObject content = tab0.GetChildren().First().gameObject.GetChildren().Where(obj => obj.name.Contains("Content")).First().gameObject;

                    TextMeshProUGUI textMesh = content.GetChildren().Where(obj => obj.name.Contains("Text")).First().GetComponent<TextMeshProUGUI>();

                    textMesh.text = "<size=150%>Crab Game <size=100%>is a game made by me (Dani lol), " +
                        "definitely not based on any cool and popular korean " +
                        "shows haha. anyway imagine you subscribed to me on " +
                        "youtube thatd be crazy haha unless? :flushed: <br>" +
                        "<br>" +
                        "<size=150%>CrabGame Cheat </size>is a Cheat made by JNNJ.";

                    CheatLog.Msg("Credits Changed");

                    aboutUIInit = true;
                }
                catch (Exception) { }
            }

            if(versionUIInit && aboutUIInit)
            {
                init = true;
                CheatLog.Msg("UI Updated!");
            }
        }
    }
}
