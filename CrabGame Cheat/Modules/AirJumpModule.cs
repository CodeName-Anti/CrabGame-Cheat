using JNNJMods.UI.Elements;
using JNNJMods.UI;
using UnityEngine;
using JNNJMods.CrabGameCheat.Util;

namespace JNNJMods.CrabGameCheat.Modules
{
    [CheatModule]
    public class AirJumpModule : SingleElementModule<ToggleInfo>
    {

        public AirJumpModule(ClickGUI gui) : base("AirJump", gui, WindowIDs.MOVEMENT)
        {

        }

        public override ElementInfo CreateElement(int windowId)
        {
            return new ToggleInfo(windowId, Name, false, true);
        }

        public override void Update()
        {
            if (InGame && Input.GetKeyDown(KeyCode.Space) && Element.GetValue<bool>())
            {
                var velocity = PlayerMovement.Instance.GetRb().velocity;

                velocity.y = 20f;

                PlayerMovement.Instance.GetRb().velocity = velocity;
            }
        }
    }
}
