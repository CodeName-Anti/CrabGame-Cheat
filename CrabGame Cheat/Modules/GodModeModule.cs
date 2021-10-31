using JNNJMods.UI;
using JNNJMods.UI.Elements;

namespace JNNJMods.CrabGameCheat.Modules
{
    public class GodModeModule : SingleElementModule<ToggleInfo>
    {

        public GodModeModule(ClickGUI gui) : base("GodMode", gui, WindowIDs.MAIN)
        {
        }

        public override ElementInfo CreateElement(int windowId)
        {
            return new ToggleInfo(windowId, "GodMode", false, true);
        }

        public override void Update()
        {
            if(InGame & Element.GetValue<bool>())
            {
                PlayerStatus.Instance.currentHp = 100;
            }
        }

    }
}
