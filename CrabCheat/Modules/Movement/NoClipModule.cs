using ImGuiNET;
using JNNJMods.CrabCheat.Rendering;
using JNNJMods.CrabCheat.Translators;
using JNNJMods.CrabCheat.Util;
using UnityEngine;

namespace JNNJMods.CrabCheat.Modules.Movement;

[CheatModule]
public class NoClipModule : Module
{
	public bool Enabled;

	public NoClipModule() : base("NoClip", TabID.Movement)
	{

	}

	public override void RenderGUIElements()
	{
		ImGui.Checkbox(Name, ref Enabled);
	}

	public override void FixedUpdate()
	{
		if (!InGame)
			return;

		if (!Enabled)
			return;

		PlayerSave save = SaveManager.Instance.state;

		if (Utilities.GetKey(save.jump))
			return;

		if (Utilities.GetKey(save.forward))
			return;

		if (Utilities.GetKey(save.backwards))
			return;

		if (Utilities.GetKey(save.left))
			return;

		if (Utilities.GetKey(save.right))
			return;

		Rigidbody rb = Instances.PlayerMovement.GetRb();
		rb.AddForce(Vector3.up * 65);
	}

	public override void Update()
	{
		if (!InGame)
			return;

		if (!Enabled)
			return;

		PlayerSave save = SaveManager.Instance.state;
		Instances.PlayerMovement.GetRb().velocity = new Vector3(0f, 0f, 0f);
		float speed = Input.GetKey(KeyCode.LeftControl) ? 0.5f : Utilities.GetKey(save.sprint) ? 1f : 0.5f;

		if (Utilities.GetKey(save.jump))
		{
			PlayerStatus.Instance.transform.position = new Vector3(PlayerStatus.Instance.transform.position.x, PlayerStatus.Instance.transform.position.y + speed, PlayerStatus.Instance.transform.position.z);
		}

		Vector3 playerTransformPosVec = PlayerStatus.Instance.transform.position;
		if (Utilities.GetKey(save.forward))
		{
			PlayerStatus.Instance.transform.position = new Vector3(playerTransformPosVec.x + (Camera.main.transform.forward.x * Camera.main.transform.up.y * speed), playerTransformPosVec.y + (Camera.main.transform.forward.y * speed), playerTransformPosVec.z + (Camera.main.transform.forward.z * Camera.main.transform.up.y * speed));
		}
		if (Utilities.GetKey(save.backwards))
		{
			PlayerStatus.Instance.transform.position = new Vector3(playerTransformPosVec.x - (Camera.main.transform.forward.x * Camera.main.transform.up.y * speed), playerTransformPosVec.y - (Camera.main.transform.forward.y * speed), playerTransformPosVec.z - (Camera.main.transform.forward.z * Camera.main.transform.up.y * speed));
		}
		if (Utilities.GetKey(save.right))
		{
			PlayerStatus.Instance.transform.position = new Vector3(playerTransformPosVec.x + (Camera.main.transform.right.x * speed), playerTransformPosVec.y, playerTransformPosVec.z + (Camera.main.transform.right.z * speed));
		}
		if (Utilities.GetKey(save.left))
		{
			PlayerStatus.Instance.transform.position = new Vector3(playerTransformPosVec.x - (Camera.main.transform.right.x * speed), playerTransformPosVec.y, playerTransformPosVec.z - (Camera.main.transform.right.z * speed));
		}

	}
}
