using JNNJMods.CrabGameCheat.Translators;
using JNNJMods.CrabGameCheat.Util;
using JNNJMods.UI;
using JNNJMods.UI.Elements;

namespace JNNJMods.CrabGameCheat.Modules
{
    [CheatModule]
    public class FlyModule : SingleElementModule<ToggleInfo>
    {

        public FlyModule(ClickGUI gui) : base("Fly", gui, WindowIDs.MOVEMENT)
        {

        }

        public override ElementInfo CreateElement(int windowId)
        {
            return new ToggleInfo(windowId, "Fly", false, true);
        }

        public override void Update()
        {
            if (InGame)
            {
                if (Element.GetValue<bool>())
                {
                    Instances.PlayerMovement.isUnderwater = true;
                    Instances.PlayerMovement.SetSwimSpeed(4666f);
                }
                else
                    Instances.PlayerMovement.isUnderwater = false;
            }
        }
    }
}
