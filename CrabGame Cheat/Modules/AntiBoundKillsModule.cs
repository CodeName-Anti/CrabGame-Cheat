using JNNJMods.CrabGameCheat.Util;
using JNNJMods.UI;
using JNNJMods.UI.Elements;
using UnityEngine;

namespace JNNJMods.CrabGameCheat.Modules
{
    [CheatModule]
    public class AntiBoundKillsModule : SingleElementModule<ToggleInfo>
    {
        public AntiBoundKillsModule(ClickGUI gui) : base("AntiBound Kills", gui, WindowIDs.PLAYER)
        {

        }

        public override ElementInfo CreateElement(int windowId)
        {
            return new ToggleInfo(windowId, Name, false, true);
        }

        public override void Update()
        {
            if(InGame && Element.GetValue<bool>())
            {
                var pos = PlayerMovement.Instance.GetRb().position;

                if (pos.y < -90)
                {
                    //Makes you slide to the sides
                    PlayerMovement.Instance.GetRb().velocity = Vector3.Exclude(Vector3.up, PlayerMovement.Instance.GetRb().velocity);

                    pos.y = -90;

                    PlayerMovement.Instance.GetRb().position = pos;
                }
            }
        }

    }
}
