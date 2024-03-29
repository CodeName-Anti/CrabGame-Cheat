﻿using JNNJMods.Utils;
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

                // Check for Updates
                updateAvailable = AutoUpdate.UpdateAvailable;
            }

            Event e = Event.current;

            // Check if any key is pressed
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

        private static void DrawWelcome()
        {
            if (!draw) return;

            // Black out
            DrawFullScreenColor(TransparentBlack);

            // Position Calculation
            float divider = 2f;
            Resolution res = Screen.currentResolution;
            float x = (res.width - res.width / divider) / 2f;
            float y = (res.height - res.height / divider) / 2f;
            Rect rect = new(x, y, res.width / divider, res.height / divider);

            // Draw middle rect
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
                (updateAvailable ? "\n<color=red><b>There's an update available!</b></color>" : "") + "\n" +
                "To open the ClickGUI press \"" + KeyCodeFormatter.KeyNames[Cheat.Instance.config.ClickGuiKeyBind] + "\"!", 40, Color.white);
        }

    }
}
