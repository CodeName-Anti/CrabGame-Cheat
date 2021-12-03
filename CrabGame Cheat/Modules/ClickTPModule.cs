using JNNJMods.CrabGameCheat.Translators;
using JNNJMods.CrabGameCheat.Util;
using JNNJMods.UI;
using JNNJMods.UI.Elements;
using UnityEngine;

namespace JNNJMods.CrabGameCheat.Modules
{
    [CheatModule]
    public class ClickTPModule : SingleElementModule<ToggleInfo>
    {
        public ClickTPModule(ClickGUI gui) : base("ClickTP", gui, WindowIDs.MOVEMENT)
        {


        }

        public override void Init(ClickGUI gui, bool json = false)
        {
            base.Init(gui, json);
        }

        public override ElementInfo CreateElement(int windowId)
        {
            return Element = new ToggleInfo(windowId, Name, false, true);
        }

        private static Vector3 FindTpPos()
        {
            Transform playerCam = Instances.PlayerMovement.playerCam;
            if (Physics.Raycast(playerCam.position, playerCam.forward, out RaycastHit raycastHit, 5000f, Instances.PlayerMovement.whatIsGround))
            {
                Vector3 b = Vector3.one;

                return raycastHit.point + b;
            }
            return Vector3.zero;
        }

        public override void Update()
        {
            if (!InGame) return;

            if (Gui.Shown) return;

            if (Input.GetKeyDown(KeyCode.Mouse1) && Element.GetValue<bool>())
            {
                Instances.PlayerMovement.GetRb().position = FindTpPos();
            }
        }

    }
}
