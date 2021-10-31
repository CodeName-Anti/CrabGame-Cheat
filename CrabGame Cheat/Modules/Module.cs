using JNNJMods.UI;
using JNNJMods.UI.Elements;
using Newtonsoft.Json;

namespace JNNJMods.CrabGameCheat.Modules
{
    public abstract class Module<T> : ModuleBase where T : ElementInfo
    {
        [JsonIgnore]
        protected new T Element
        {
            get
            {
                return base.Element as T;
            }
            set
            {
                base.Element = value;
            }
        }

        public Module(string name, ClickGUI gui, WindowIDs windowId, bool empty = false) : base(name, gui, windowId, empty)
        {

        }

    }
}
