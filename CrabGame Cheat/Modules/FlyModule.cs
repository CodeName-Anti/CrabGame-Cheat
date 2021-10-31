using JNNJMods.UI.Elements;
using JNNJMods.UI;

namespace JNNJMods.CrabGameCheat.Modules
{
    public class FlyModule : SingleElementModule<ToggleInfo>
    {

        public FlyModule(ClickGUI gui) : base("Fly", gui, WindowIDs.MAIN)
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
                    PlayerMovement.Instance.underWater = true;
                    PlayerMovement.Instance.swimSpeed = 4666f;
                }
                else
                    PlayerMovement.Instance.underWater = false;
            }
        }
    }
}
