using JNNJMods.Utils;
using System;
using System.Threading.Tasks;
using UnityEngine;
using static JNNJMods.Render.DrawingUtil;

namespace JNNJMods.UI
{
    public class KeyBindSelection
    {
        #region Variables
        public Color
            SelectionColor,
            SuccessfulColor,
            TextColor;

        public int FontSize;

        public bool Shown;

        private bool displayReset;

        private KeyCode key = KeyCode.None;

        public event KeySelectedCallback KeySelected = delegate { };

        #endregion

        public KeyBindSelection(Color selectionColor, Color successfulColor, Color textColor, int fontSize)
        {
            SelectionColor = selectionColor;
            SuccessfulColor = successfulColor;
            TextColor = textColor;
            FontSize = fontSize;
        }

        public KeyBindSelection() : this(Color.red, Color.green, Color.black, 40)
        {

        }

        private async void DelayedReset()
        {
            await Task.Delay(5000);

            Shown = false;
            key = KeyCode.None;
            ClickGUI.Instance.Show(false);

            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

        public void DrawSelection()
        {
            if (Shown)
            {
                #region Key Detection
                Event e = Event.current;

                if (e != null && e.isKey && key == KeyCode.None)
                {
                    if(Input.GetKeyDown(e.keyCode))
                    {

                        KeyCode key = e.keyCode;

                        switch(key)
                        {
                            case KeyCode.Escape:
                                key = KeyCode.None;
                                displayReset = true;
                                break;
                        }

                        KeySelected(this.key = key);

                        KeySelected = delegate { };

                        DelayedReset();
                    }
                }
                #endregion

                DrawFullScreenColor(TransparentBlack);

                Resolution res = Screen.currentResolution;
                float x = (res.width - res.width / 2.5f) / 2f;
                float y = (res.height - res.height / 2.5f) / 2f;
                Rect rect = new Rect(x, y, res.width / 2.5f, res.height / 2.5f);

                Color color = key != KeyCode.None ? SuccessfulColor : SelectionColor;

                if(displayReset)
                {
                    color = SuccessfulColor;
                }

                DrawColor(MakeColorTransparent(color), rect);

                string text = "Press Any Key to Bind..." + (key != KeyCode.None || displayReset ? "\n" + KeyCodeFormatter.KeyNames[key] : "");

                DrawCenteredText(text , FontSize, TextColor);
            }
        }

        public delegate void KeySelectedCallback(KeyCode key);
    }
}
