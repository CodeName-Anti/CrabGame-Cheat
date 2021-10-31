using JNNJMods.UI.Elements;
using JNNJMods.UI;
using UnityEngine;

namespace JNNJMods.CrabGameCheat.Modules
{
    public class AirJumpModule : SingleElementModule<ToggleInfo>
    {

        public AirJumpModule(ClickGUI gui) : base("AirJump", gui, WindowIDs.MAIN)
        {

        }

        public override ElementInfo CreateElement(int windowId)
        {
            return new ToggleInfo(windowId, Name);
        }

        public override void Update()
        {
            if(InGame)
            {
                if (Input.GetKeyDown(KeyCode.Space) && Element.GetValue<bool>())
                {
                    PlayerMovement.Instance.PushPlayer(new Vector3(0.0f, 50f, 0.0f));
                }
            }
        }
    }
}
