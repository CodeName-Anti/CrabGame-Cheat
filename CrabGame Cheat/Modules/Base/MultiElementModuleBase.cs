using JNNJMods.UI;
using JNNJMods.UI.Elements;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace JNNJMods.CrabGameCheat.Modules
{
    public abstract class MultiElementModuleBase : ModuleBase
    {
        [JsonIgnore]
        public List<ElementInfo> Elements { get; protected set; }

        [JsonIgnore]
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
            set
            {
                int i = 0;
                foreach (ElementInfo info in Elements)
                {

                    if (info.KeyBindable)
                    {
                        info.KeyBind = value[i];
                        i++;
                    }
                }
            }
        }

        public override void Init(ClickGUI gui, bool json = false)
        {
            base.Init(gui, json);
            Elements = new List<ElementInfo>();
        }

        public MultiElementModuleBase(string name, ClickGUI gui, WindowIDs windowId) : base(name, gui, windowId)
        {

        }

    }
}
