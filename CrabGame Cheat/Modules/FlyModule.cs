using JNNJMods.UI.Elements;
using JNNJMods.UI;
using JNNJMods.CrabGameCheat.Util;
using JNNJMods.CrabGameCheat.Translators;

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
            if(InGame)
            {
                if(Element.GetValue<bool>())
                {
                    Instances.PlayerMovement.underWater = true;
                    Instances.PlayerMovement.SetSwimSpeed(4666f);
                }
                else
                    Instances.PlayerMovement.underWater = false;
            }
        }
    }
}
