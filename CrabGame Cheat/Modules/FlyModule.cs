using CodeStage.AntiCheat.ObscuredTypes;
using JNNJMods.CrabGameCheat.Translators;
using JNNJMods.CrabGameCheat.Util;
using JNNJMods.UI;
using JNNJMods.UI.Elements;
using System;
using UnityEngine;

namespace JNNJMods.CrabGameCheat.Modules
{
    [CheatModule]
    public class FlyModule : SingleElementModule<ToggleInfo>
    {

        public FlyModule(ClickGUI gui) : base("Fly", gui, WindowIDs.MOVEMENT)
        {

        }

        public override ElementInfo CreateElement(int windowId)
        {
            return new ToggleInfo(windowId, "Fly", false, true);
        }

		private static bool GetKey(int key)
        {
			return Input.GetKey((KeyCode)key);
        }

        public override void FixedUpdate()
        {
			if(InGame && Element.GetValue<bool>())
            {
				PlayerSave save = SaveManager.Instance.state;
				if (!GetKey(save.jump) ||
					!GetKey(save.forward) ||
					!GetKey(save.backwards) ||
					!GetKey(save.left) ||
					!GetKey(save.right))
				{
					Rigidbody rb1 = Instances.PlayerMovement.GetRb();
					rb1.AddForce(Vector3.up * 65);
				}
			}
        }

        public override void Update()
        {
            
            if (InGame && Element.GetValue<bool>())
            {
				PlayerSave save = SaveManager.Instance.state;
				Instances.PlayerMovement.GetRb().velocity = new Vector3(0f, 0f, 0f);
				float speed = Input.GetKey(KeyCode.LeftControl) ? 0.5f : (GetKey(save.sprint) ? 1f : 0.5f);

				if (GetKey(save.jump))
				{
					PlayerStatus.Instance.transform.position = new Vector3(PlayerStatus.Instance.transform.position.x, PlayerStatus.Instance.transform.position.y + speed, PlayerStatus.Instance.transform.position.z);
				}

				Vector3 playerTransformPosVec = PlayerStatus.Instance.transform.position;
				if (GetKey(save.forward))
				{
					PlayerStatus.Instance.transform.position = new Vector3(playerTransformPosVec.x + Camera.main.transform.forward.x * Camera.main.transform.up.y * speed, playerTransformPosVec.y + Camera.main.transform.forward.y * speed, playerTransformPosVec.z + Camera.main.transform.forward.z * Camera.main.transform.up.y * speed);
				}
				if (GetKey(save.backwards))
				{
					PlayerStatus.Instance.transform.position = new Vector3(playerTransformPosVec.x - Camera.main.transform.forward.x * Camera.main.transform.up.y * speed, playerTransformPosVec.y - Camera.main.transform.forward.y * speed, playerTransformPosVec.z - Camera.main.transform.forward.z * Camera.main.transform.up.y * speed);
				}
				if (GetKey(save.right))
				{
					PlayerStatus.Instance.transform.position = new Vector3(playerTransformPosVec.x + Camera.main.transform.right.x * speed, playerTransformPosVec.y, playerTransformPosVec.z + Camera.main.transform.right.z * speed);
				}
				if (GetKey(save.left))
				{
					PlayerStatus.Instance.transform.position = new Vector3(playerTransformPosVec.x - Camera.main.transform.right.x * speed, playerTransformPosVec.y, playerTransformPosVec.z - Camera.main.transform.right.z * speed);
				}
			}
        }
    }
}
