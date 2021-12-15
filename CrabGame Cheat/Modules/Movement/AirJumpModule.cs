using JNNJMods.CrabGameCheat.Translators;
using JNNJMods.CrabGameCheat.Util;
using JNNJMods.UI;
using JNNJMods.UI.Elements;

namespace JNNJMods.CrabGameCheat.Modules
{
    [CheatModule]
    public class AirJumpModule : SingleElementModule<ToggleInfo>
    {

        public AirJumpModule(ClickGUI gui) : base("AirJump", gui, WindowIDs.Movement)
        {

        }

        public override ElementInfo CreateElement(int windowId)
        {
            return new ToggleInfo(windowId, Name, false, true);
        }

        public override void Update()
        {
            if (InGame && Utilities.GetKeyDown(SaveManager.Instance.state.jump) && Element.GetValue<bool>())
            {
                var velocity = Instances.PlayerMovement.GetRb().velocity;

                velocity.y = 20f;

                Instances.PlayerMovement.GetRb().velocity = velocity;
            }
        }
    }
}
