using System.Text;
using UnityEngine;

namespace JNNJMods.UI.Elements
{
    public class ToggleInfo : ElementInfo
    {
        /// <summary>
        /// Executed when Toggle Changed
        /// </summary>
        public event ToggleChangedCallback ToggleChanged = delegate { };

        /// <summary>
        /// Button Text
        /// </summary>
        public string text;

        /// <summary>
        /// Enables Colors for text
        /// </summary>
        public bool color = true;

        private string MakeEnable(string text, bool state)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(text).Append(" (");

            if(color)
                builder.Append(state ? "<color=green>ON" : "<color=red>OFF").Append("</color>");
            else
                builder.Append(state ? "ON" : "OFF");

            builder.Append(")");

            return builder.ToString();
        }

        public ToggleInfo(int windowId, string text) : this(windowId, text, false)
        {
            this.text = text;
        }

        public ToggleInfo(int windowId, string text, bool startState, bool keyBindable = false) : base(windowId, keyBindable)
        {
            this.text = text;
            SetValue(startState);
        }

        public void SetToggled(bool toggled)
        {
            SetValue(toggled);
            ToggleChanged.Invoke((bool)value);
        }

        public bool Toggle()
        {
            Activate();

            return GetValue<bool>();
        }

        public override void Activate()
        {
            SetToggled(!GetValue<bool>());
        }

        protected override object RenderElement(Rect rect, GUIStyle style)
        {
            RunStyleCheck(GUI.skin.button);
            bool val = GUI.Button(rect, MakeEnable(text, GetValue<bool>()), style);

            bool toggled = GetValue<bool>();
            if (val)
            {
                toggled = !toggled;
                ToggleChanged(toggled);
            }

            return toggled;
        }

        public delegate void ToggleChangedCallback(bool toggled);
    }
}
