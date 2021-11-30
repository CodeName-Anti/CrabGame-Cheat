using CodeStage.AntiCheat.ObscuredTypes;
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

        public override void Init(ClickGUI gui, bool json = false)
        {
            base.Init(gui, json);

        }

        public override ElementInfo CreateElement(int windowId)
        {
            Element = new ToggleInfo(windowId, "GodMode", false, true);

            Element.ToggleChanged += Element_ToggleChanged;

            return Element;
        }

        private void Element_ToggleChanged(bool toggled)
        {
            PlayerStatus.Instance.currentHp = new ObscuredInt(100);
            PlayerStatusPatch.GodMode = toggled;
        }
    }
}
