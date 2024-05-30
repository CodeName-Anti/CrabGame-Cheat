using UnityEngine;

namespace JNNJMods.UI.Elements
{
    public class CustomInfo : ElementInfo
    {
        /// <summary>
        /// Executes from OnGUI() to render a custom Element.
        /// </summary>
        public CustomRenderMethod CustomRender = delegate { return null; };

        public CustomInfo(int windowId, bool keyBindable = false) : base(windowId, keyBindable)
        {

        }

        protected override object RenderElement(Rect rect, GUIStyle style)
        {
            return CustomRender(rect, style);
        }

        public delegate object CustomRenderMethod(Rect rect, GUIStyle style);
    }
}
