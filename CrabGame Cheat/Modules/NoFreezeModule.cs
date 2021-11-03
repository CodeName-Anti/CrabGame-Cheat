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
            return new ToggleInfo(windowId, Name, true, true);
        }

        public override void Update()
        {
            if(InGame && Element.GetValue<bool>() && PersistentPlayerData.frozen)
            {
                PersistentPlayerData.frozen = false;
            }
        }

    }
}
