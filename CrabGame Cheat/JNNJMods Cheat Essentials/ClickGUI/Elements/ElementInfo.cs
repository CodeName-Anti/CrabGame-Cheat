using JNNJMods.Utils;
using UnityEngine;

namespace JNNJMods.UI.Elements
{
    public class ElementInfo
    {
        #region Variables
        /// <summary>
        /// Defines if this Element can be bound to a Key
        /// </summary>
        public bool KeyBindable;

        /// <summary>
        /// Current KeyBind of this Element
        /// </summary>
        public KeyCode KeyBind;

        /// <summary>
        /// Current value
        /// </summary>
        protected object value;

        /// <summary>
        /// <see cref="ElementManager"/> instance of this.
        /// </summary>
        public ElementManager manager;

        /// <summary>
        /// <see cref="GUIStyle"/> of the Element
        public GUIStyle style;

        public int WindowId { get; private set; }

        /// <summary>
        /// Executed when the Value Changes.
        /// </summary>
        public event ValueChangedCallback ValueChanged = delegate { };
        private int index = -1;
        private Rect rect;

        #endregion

        public virtual void Activate() { }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="val"></param>
        /// <returns>The set Element</returns>
        public object SetValue(object val)
        {
            return value = val;
        }

        /// <summary>
        /// Gets the value cast to T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetValue<T>()
        {
            if (value.GetType().IsAssignableFrom(typeof(T)))
            {
                return (T)value;
            }
            return default;
        }

        public ElementInfo(int windowId, bool keyBindable = false)
        {
            WindowId = windowId;
            KeyBindable = keyBindable;
        }
        
        /// <summary>
        /// Renders the Element
        /// </summary>
        /// <returns></returns>
        protected virtual object RenderElement(Rect rect, GUIStyle style)
        {
            return null;
        }

        protected void RunStyleCheck(GUIStyle fallback)
        {
            if(style == null)
            {
                style = fallback;
            }
        }

        /// <summary>
        /// Renders the Element and sets the Value
        /// </summary>

        public void Render()
        {
            if (index == -1)
            {
                index = manager.Elements.IndexOf(this);
            }

            rect = manager.NextControlRect(index);

            if(KeyBindable)
            {
                float offset = rect.width / 5f;

                Rect keyBindRect = rect;

                rect.width -= offset + manager.controlDist;
                keyBindRect.x += rect.width + manager.controlDist;
                keyBindRect.width = offset;

                string btnText = KeyBind == KeyCode.None ? "Bind" : KeyCodeFormatter.KeyNames[KeyBind];

                if (GUI.Button(keyBindRect, btnText))
                {
                    ClickGUI gui = ClickGUI.Instance;
                    gui.Hide(false);
                    gui.keyBindSelection.Shown = true;
                    gui.keyBindSelection.KeySelected += KeyBindSelection_KeySelected;
                }
            }

            object oldValue = value;
            object newValue = RenderElement(rect, style);

            if(oldValue != newValue)
            {
                ValueChanged(oldValue, newValue);
            }

            value = newValue;
        }

        private void KeyBindSelection_KeySelected(KeyCode key)
        {
            KeyBind = key;
        }

        public delegate void ValueChangedCallback(object oldValue, object newValue);

    }
}
