using JNNJMods.UI;
using JNNJMods.UI.Elements;
using System.Collections.Generic;
using UnityEngine;

namespace JNNJMods.CrabGameCheat.Modules
{
    public abstract class MultiElementModuleBase : ModuleBase
    {
        public List<ElementInfo> Elements { get; protected set; }

        public void SetKeyBinds()
        {
            int i = 0;
            foreach (ElementInfo info in Elements)
            {

                if (info.KeyBindable)
                {
                    info.KeyBind = KeyBinds[i];
                    i++;
                }

            }
        }

        public List<KeyCode> KeyBinds
        {
            get
            {
                List<KeyCode> keys = new List<KeyCode>();

                foreach (ElementInfo info in Elements)
                {
                    if (info.KeyBindable)
                    {
                        keys.Add(info.KeyBind);
                    }
                }

                return keys;
            }
        }

        public override void Init(ClickGUI gui, bool json = false)
        {
            base.Init(gui, json);
            Elements = new List<ElementInfo>();
            SetKeyBinds();
        }

        public MultiElementModuleBase(string name, ClickGUI gui, WindowIDs windowId) : base(name, gui, windowId)
        {

        }

    }
}
