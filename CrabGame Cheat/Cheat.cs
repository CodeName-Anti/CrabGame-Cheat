using JNNJMods.UI;
using JNNJMods.UI.Elements;
using UnityEngine;
using MelonLoader;
using CodeStage.AntiCheat.Detectors;
using JNNJMods.Render;

namespace JNNJMods.CrabGameCheat
{
    public class Cheat : MelonMod
    {
        public static Cheat Instance { get; private set; }
        private ClickGUI gui;

        private ToggleInfo clickTp;

        public override void OnApplicationStart()
        {
            Instance = this;

            StopAntiCheat();

            gui = new ClickGUI(10, 40, 10)
            {
                BlackOut = true
            };

            gui.AddWindow((int)WindowIDs.MAIN, "JNNJ's CrabGame Cheat", 100, 100, 400, 500);

            clickTp = new ToggleInfo((int)WindowIDs.MAIN, "ClickTP")
            {
                KeyBindable = true
            };

            gui.AddElement(clickTp);
        }

        void StopAntiCheat()
        {
            TimeCheatingDetector.StopDetection();
            InjectionDetector.StopDetection();
            WallHackDetector.StopDetection();
            SpeedHackDetector.StopDetection();
            ObscuredCheatingDetector.StopDetection();

            TimeCheatingDetector.Dispose();
            InjectionDetector.Dispose();
            WallHackDetector.Dispose();
            SpeedHackDetector.Dispose();
            ObscuredCheatingDetector.Dispose();
        }

        public override void OnUpdate()
        {
            gui.Update();

            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                gui.Shown = !gui.Shown;
            }

            if (PlayerMovement.Instance == null)
                return;

            if (Input.GetKeyDown(KeyCode.Mouse0) && clickTp.GetValue<bool>())
            {
                Object.FindObjectOfType<PlayerMovement>().GetRb().position = FindTpPos();
            }
        }

        private static Vector3 FindTpPos()
        {
            Transform playerCam = PlayerMovement.Instance.playerCam;
            if (!Physics.Raycast(playerCam.position, playerCam.forward, out RaycastHit hitInfo, 1500f))
                return Vector3.zero;
            Vector3 vector3 = Vector3.zero;
            if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                vector3 = Vector3.one;
            return hitInfo.point + vector3;
        }

        public override void OnGUI()
        {
            gui.DrawWindows();

            if(!gui.Shown)
            {
                DrawingUtil.DrawText("CrabGame Cheat by JNNJ", new Vector2(0, 10), 17, Color.black);
            }
        }
    }
}
