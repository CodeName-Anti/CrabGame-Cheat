using JNNJMods.CrabGameCheat.Util.KeyBinds;
using JNNJMods.UI;
using JNNJMods.UI.Elements;
using Newtonsoft.Json;

namespace JNNJMods.CrabGameCheat.Modules
{
    public abstract class SingleElementModuleBase : ModuleBase
    {
        [JsonIgnore]
        public ElementInfo Element { get; protected set; }


        public SingleElementModuleBase(string name, ClickGUI gui, WindowIDs windowId) : base(name, gui, windowId)
        {

        }

        public override void Init(ClickGUI gui, bool json = false)
        {
            base.Init(gui, json);
            gui.AddElement(Element = CreateElement(ID));
        }

        public override void SetKeyBinds(KeyBind keybind)
        {
            if (keybind.Keys.Length > 0)
                Element.KeyBind = keybind.Keys[0];
        }

        public override KeyBind GetKeyBinds()
        {
            return new KeyBind()
            {
                Keys = new[] { Element.KeyBind }
            };
        }

        /// <summary>
        /// Called after Module initializes.
        /// </summary>
        /// <returns></returns>
        public abstract ElementInfo CreateElement(int windowId);
    }
}
