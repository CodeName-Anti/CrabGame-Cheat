using UnityEngine;

namespace JNNJMods.UI.Elements
{
    public class ButtonInfo : ElementInfo
    {
        /// <summary>
        /// Text on the Button.
        /// </summary>
        public string text;
        public event ButtonPressCallback ButtonPress = delegate { };

        public ButtonInfo(int windowId, string text, bool keyBindable = false) : base(windowId, keyBindable)
        {
            this.text = text;
        }

        public override void Activate()
        {
            ButtonPress();
        }

        protected override object RenderElement(Rect rect, GUIStyle style)
        {
            RunStyleCheck(GUI.skin.button);

            bool val = GUI.Button(rect, text, style);

            if (val) ButtonPress();
            return val;
        }

        public delegate void ButtonPressCallback();
    }
}
