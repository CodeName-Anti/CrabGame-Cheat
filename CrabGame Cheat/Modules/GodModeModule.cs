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
            Element = new ToggleInfo(windowId, "GodMode", false, true);

            Element.ToggleChanged += Element_ToggleChanged;

            return Element;
        }

        private void Element_ToggleChanged(bool toggled)
        {
            if(toggled)
            {
                PlayerStatus.Instance.currentHp = 6942000;
                PlayerStatus.Instance.maxHp = 6942000;
            } else
            {
                PlayerStatus.Instance.currentHp = 100;
                PlayerStatus.Instance.maxHp = 100;
            }
        }

        public override void Update()
        {
            if(InGame & Element.GetValue<bool>())
            {
                Element_ToggleChanged(true);
            }
        }

    }
}
