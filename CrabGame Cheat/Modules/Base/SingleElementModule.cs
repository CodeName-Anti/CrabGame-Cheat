using JNNJMods.UI;
using JNNJMods.UI.Elements;
using Newtonsoft.Json;

namespace JNNJMods.CrabGameCheat.Modules
{
    public abstract class SingleElementModule<T> : SingleElementModuleBase where T : ElementInfo
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

        public SingleElementModule(string name, ClickGUI gui, WindowIDs windowId) : base(name, gui, windowId)
        {

        }
    }
}
