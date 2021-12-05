using JNNJMods.CrabGameCheat.Util.KeyBinds;
using JNNJMods.UI;
using JNNJMods.UI.Elements;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JNNJMods.CrabGameCheat.Modules
{
    public abstract class MultiElementModuleBase : ModuleBase
    {
        public List<ElementInfo> Elements { get; protected set; }

        public override void SetKeyBinds(KeyBind keybind)
        {
            if(Elements == null || Elements.Count == 0)
                return;

            var keyBindable = Elements.Where(e => e.KeyBindable);

            if (keyBindable.Count() == 0)
                return;

            if (keyBindable.Count() != keybind.Keys.Length)
                return;

            int i = 0;
            foreach(ElementInfo info in keyBindable)
            {
                info.KeyBind = keybind.Keys[i];

                i++;
            }
        }

        public override KeyBind GetKeyBinds()
        {
            return new KeyBind()
            {
                Keys = Elements.Where(e => e.KeyBindable).Select(e => e.KeyBind).ToArray()
            };
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
