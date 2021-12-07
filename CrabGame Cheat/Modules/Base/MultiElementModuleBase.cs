using JNNJMods.CrabGameCheat.Util.KeyBinds;
using JNNJMods.UI;
using JNNJMods.UI.Elements;
using System.Collections.Generic;
using System.Linq;

namespace JNNJMods.CrabGameCheat.Modules
{
    public abstract class MultiElementModuleBase : ModuleBase
    {
        public List<ElementInfo> Elements { get; protected set; }

        public override void SetKeyBinds(KeyBind keybind)
        {
            if(Elements == null || Elements.Count == 0)
                return;

            int keybindIndex = 0;
            int toggleIndex = 0;
            foreach(ElementInfo info in Elements)
            {
                if(typeof(ToggleInfo).IsAssignableFrom(info.GetType()))
                {
                    (info as ToggleInfo).SetToggled(keybind.Toggled[toggleIndex]);

                    toggleIndex++;
                }

                if (Elements[keybindIndex].KeyBindable)
                {
                    info.KeyBind = keybind.Keys[keybindIndex];
                    keybindIndex++;
                }
            }
        }

        public override KeyBind GetKeyBinds()
        {
            var bindable = Elements.Where(e => e.KeyBindable);
            return new KeyBind()
            {
                Keys = bindable.Select(e => e.KeyBind).ToArray(),
                Toggled =
                    Elements.
                    // Get all toggles
                    Where(e => typeof(ToggleInfo).IsAssignableFrom(e.GetType())).
                    // Get Toggled Value
                    Select(e => ((ToggleInfo)e).GetValue<bool>()).
                    // To Array
                    ToArray()
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
