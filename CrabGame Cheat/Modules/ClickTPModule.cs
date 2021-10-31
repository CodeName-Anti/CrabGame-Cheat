using JNNJMods.UI;
using JNNJMods.UI.Elements;
using UnityEngine;

namespace JNNJMods.CrabGameCheat.Modules
{
    public class ClickTPModule : Module<ToggleInfo>
    {

        public ClickTPModule(ClickGUI gui) : base("ClickTP", gui, WindowIDs.MAIN)
        {
        }

        public override ElementInfo CreateElement(int windowId)
        {
            return Element = new ToggleInfo(windowId, Name);
        }

        private static Vector3 FindTpPos()
        {
            Transform playerCam = PlayerMovement.Instance.playerCam;
            if (Physics.Raycast(playerCam.position, playerCam.forward, out RaycastHit raycastHit, 5000f))
            {
                Vector3 b = Vector3.one;

                return raycastHit.point + b;
            }
            return Vector3.zero;
        }

        public override void Update()
        {
            if (!InGame) return;

            if (Input.GetKeyDown(KeyCode.Mouse0) && Element.GetValue<bool>())
            {
                Object.FindObjectOfType<PlayerMovement>().GetRb().position = FindTpPos();
            }
        }

    }
}
