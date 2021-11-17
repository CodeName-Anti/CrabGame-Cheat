using JNNJMods.UI;
using JNNJMods.UI.Elements;
using UnityEngine;

namespace JNNJMods.CrabGameCheat.Modules
{
    public abstract class SingleElementModuleBase : ModuleBase
    {
        public ElementInfo Element { get; protected set; }

        public void SetKeyBind()
        {
            Element.KeyBind = KeyBind;
        }

        public KeyCode KeyBind
        {
            get
            {
                return Element.KeyBind;
            }
            set
            {
                if (Element.KeyBindable)
                    Element.KeyBind = value;
            }
        }

        public SingleElementModuleBase(string name, ClickGUI gui, WindowIDs windowId) : base(name, gui, windowId)
        {

        }

        public override void Init(ClickGUI gui, bool json = false)
        {
            base.Init(gui, json);
            gui.AddElement(Element = CreateElement(ID));
            SetKeyBind();
        }

        /// <summary>
        /// Called after Module initializes.
        /// </summary>
        /// <returns></returns>
        public abstract ElementInfo CreateElement(int windowId);
    }
}
