using JNNJMods.CrabGameCheat.Translators;
using JNNJMods.CrabGameCheat.Util;
using JNNJMods.UI;
using JNNJMods.UI.Elements;
using UnityEngine;

namespace JNNJMods.CrabGameCheat.Modules
{
    [CheatModule]
    public class AntiBoundKillsModule : SingleElementModule<ToggleInfo>
    {

        public float killHeight = -69420187;

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

                if(killHeight == -69420187)
                {
                    var killBounds = Object.FindObjectOfType<KillPlayerOutOfBounds>();

                    if (killBounds != null)
                    {
                        killHeight = killBounds.killHeight;
                    }
                    else
                        return;
                }

                var pos = Instances.PlayerMovement.GetRb().position;

                if (pos.y < (killHeight + 2))
                {
                    //Makes you slide to the sides
                    Instances.PlayerMovement.GetRb().velocity = Vector3.Exclude(Vector3.up, Instances.PlayerMovement.GetRb().velocity);

                    pos.y = killHeight + 2;

                    //Float above KillBounds
                    Instances.PlayerMovement.GetRb().position = pos;
                }
            }
        }

    }
}
