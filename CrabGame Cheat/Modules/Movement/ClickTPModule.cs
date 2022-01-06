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
        public ClickTPModule(ClickGUI gui) : base("ClickTP", gui, WindowIDs.Movement)
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

            bool rayHitStuff = Physics.Raycast(playerCam.position, playerCam.forward, out var raycastHit, 5000f, GameManager.Instance.whatIsGround);
            Vector3 result;
            if (rayHitStuff)
            {
                Vector3 vector = Vector3.one;
                result = raycastHit.point + vector;
            }
            else
            {
                result = Vector3.zero;
            }
            return result;
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
