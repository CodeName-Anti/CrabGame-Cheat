using JNNJMods.CrabGameCheat.Patches;
using JNNJMods.CrabGameCheat.Util;
using JNNJMods.UI;
using JNNJMods.UI.Elements;

namespace JNNJMods.CrabGameCheat.Modules
{
    [CheatModule]
    public class GodModeModule : SingleElementModule<ToggleInfo>
    {

        public GodModeModule(ClickGUI gui) : base("GodMode", gui, WindowIDs.PLAYER)
        {
        }

        public override ElementInfo CreateElement(int windowId)
        {
            Element = new ToggleInfo(windowId, "GodMode", false, true);

            Element.ToggleChanged += Element_ToggleChanged;

            return Element;
        }

        private void Element_ToggleChanged(bool toggled)
        {
            PlayerStatus.Instance.currentHp = 100;
            PlayerStatusPatch.GodMode = toggled;
        }
    }
}
