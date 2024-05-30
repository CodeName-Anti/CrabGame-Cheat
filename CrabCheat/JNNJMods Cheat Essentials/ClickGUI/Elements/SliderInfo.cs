using UnityEngine;

namespace JNNJMods.UI.Elements
{
    public class SliderInfo : ElementInfo
    {
        public float
            minValue,
            maxValue;

        public SliderInfo(int windowId, float minValue, float maxValue) : base(windowId, false)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;

            SetValue(minValue);
        }

        public SliderInfo(int windowId) : this(windowId, 0, 100) { }

        protected override object RenderElement(Rect rect, GUIStyle style)
        {
            RunStyleCheck(GUI.skin.horizontalSlider);
            return GUI.HorizontalSlider(rect, GetValue<float>(), minValue, maxValue);
        }
    }
}
