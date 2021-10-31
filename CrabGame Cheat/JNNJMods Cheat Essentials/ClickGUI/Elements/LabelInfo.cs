using UnityEngine;

namespace JNNJMods.UI.Elements
{
    public class LabelInfo : ElementInfo
    {
        /// <summary>
        /// Text of the Label.
        /// </summary>
        public string text;
        public LabelInfo(int windowId, string text) : base(windowId, false)
        {
            this.text = text;
        }

        protected override object RenderElement(Rect rect, GUIStyle style)
        {
            RunStyleCheck(GUI.skin.label);
            GUI.Label(rect, text, style);
            return null;
        }
    }
}
