using JNNJMods.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Reflection;
using UnityEngine;
using static JNNJMods.Render.DrawingUtil;

namespace JNNJMods.CrabGameCheat.Util
{
    public static class WelcomeScreen
    {
        private static bool
            init,
            updateAvailable;

        public static bool draw;

        public static void OnGUI()
        {
            if (!init)
            {
                init = true;

                try
                {
                    using var client = new WebClient();
                    //Some random user agent because with others it responds with 403
                    client.Headers.Add("User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:93.0) Gecko/20100101 Firefox/93.0");
                    string json = client.DownloadString("https://api.github.com/repos/CodeName-Anti/CrabGame-Cheat/releases");

                    JArray jArr = JArray.Parse(json);

                    string stringVersion = jArr[0].ToObject<JObject>().GetValue("tag_name").ToObject<string>();

                    //Compare GitHub and Local Version
                    Version git = new(stringVersion);
                    Version current = Assembly.GetExecutingAssembly().GetName().Version;

                    int result = current.CompareTo(git);

                    if (result < 0)
                    {
                        updateAvailable = true;
                    }
                }
                catch (Exception)
                {
                    CheatLog.Warning("Couldn't fetch Updates from GitHub!");
                }
            }

            Event e = Event.current;

            if (e != null && e.isKey)
            {
                if (Input.GetKeyDown(e.keyCode))
                {
                    draw = false;
                    return;
                }
            }

            DrawWelcome();
        }

        static void DrawWelcome()
        {
            if (!draw) return;

            //Black out
            DrawFullScreenColor(TransparentBlack);

            //Position Calculation
            float divider = 2f;
            Resolution res = Screen.currentResolution;
            float x = (res.width - res.width / divider) / 2f;
            float y = (res.height - res.height / divider) / 2f;
            Rect rect = new(x, y, res.width / divider, res.height / divider);

            //Draw middle rect
            DrawColor(new Color(0, 1, 0, 0.6f), rect);

            #region WaterMark

            string waterMark = "<b>JNNJ's CrabGame Cheat</b>";

            Rect waterMarkRect = CenteredTextRect(waterMark, 40);
            waterMarkRect.y -= (rect.y / 1.6f);

            GUI.Label(waterMarkRect, waterMark, GetTextStyle(40, Color.black));

            #endregion

            #region Press any Key to continue

            string continueText = "<i><b>Press any Key to continue...</b></i>";

            Rect continueRect = CenteredTextRect(continueText, 40);
            continueRect.y += (rect.y / 1.6f);

            GUI.Label(continueRect, continueText, GetTextStyle(40, Color.black));

            #endregion

            DrawCenteredText(
                "Welcome to JNNJ's CrabGame Cheat!" +
                (updateAvailable ? "\n<b>There's an update available!</b>" : "") + "\n" +
                "To open the ClickGUI press \"" + KeyCodeFormatter.KeyNames[Cheat.Instance.config.ClickGuiKeyBind] + "\"!", 40, Color.white);
        }

    }
}
