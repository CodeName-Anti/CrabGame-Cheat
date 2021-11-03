using JNNJMods.UI.Elements;
using JNNJMods.UI;
using JNNJMods.CrabGameCheat.Util;
using JNNJMods.CrabGameCheat.Patches;

namespace JNNJMods.CrabGameCheat.Modules
{
    [CheatModule]
    public class NoFreezeModule : SingleElementModule<ToggleInfo>
    {

        public NoFreezeModule(ClickGUI gui) : base("NoFreeze", gui, WindowIDs.MOVEMENT)
        {

        }

        public override ElementInfo CreateElement(int windowId)
        {
            Element = new ToggleInfo(windowId, Name, true, true);

            Element.ToggleChanged += Element_ToggleChanged;

            return Element;
        }

        void Element_ToggleChanged(bool toggled)
        {
            ClientHandlePatch.NoFreeze = toggled;
        }

    }
}
