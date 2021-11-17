using JNNJMods.CrabGameCheat.Patches;
using JNNJMods.CrabGameCheat.Util;
using JNNJMods.UI;
using JNNJMods.UI.Elements;

namespace JNNJMods.CrabGameCheat.Modules
{
    [CheatModule]
    public class NoPushModule : SingleElementModule<ToggleInfo>
    {

        public NoPushModule(ClickGUI gui) : base("NoPush", gui, WindowIDs.MOVEMENT)
        {

        }

        public override ElementInfo CreateElement(int windowId)
        {
            Element = new ToggleInfo(windowId, Name, false, true);

            Element.ToggleChanged += Element_ToggledChanged;

            return Element;
        }

        private void Element_ToggledChanged(bool toggled)
        {
            GameManagerPatch.NoPush = toggled;
        }
    }
}
