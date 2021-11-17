using JNNJMods.UI;
using JNNJMods.UI.Elements;

namespace JNNJMods.CrabGameCheat.Modules
{
    public abstract class SingleElementModule<T> : SingleElementModuleBase where T : ElementInfo
    {
        public new T Element
        {
            get
            {
                return base.Element as T;
            }
            protected set
            {
                base.Element = value;
            }
        }

        public SingleElementModule(string name, ClickGUI gui, WindowIDs windowId) : base(name, gui, windowId)
        {

        }
    }
}
