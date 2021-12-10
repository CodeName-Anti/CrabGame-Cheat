using JNNJMods.CrabGameCheat.Util;
using JNNJMods.UI;
using JNNJMods.UI.Elements;

namespace JNNJMods.CrabGameCheat.Modules
{
    [CheatModule]
    public class NoFreezeModule : SingleElementModule<ToggleInfo>
    {

        public NoFreezeModule(ClickGUI gui) : base("NoFreeze", gui, WindowIDs.Movement)
        {

        }

        public override ElementInfo CreateElement(int windowId)
        {
            return new ToggleInfo(windowId, Name, false, true);
        }

        public override void Update()
        {
            if (InGame && Element.GetValue<bool>() && PersistentPlayerData.frozen)
            {
                PersistentPlayerData.frozen = false;
            }
        }

    }
}
