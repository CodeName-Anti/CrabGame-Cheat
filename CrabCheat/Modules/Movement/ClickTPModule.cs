using ImGuiNET;
using JNNJMods.CrabCheat.Rendering;
using JNNJMods.CrabCheat.Translators;
using UnityEngine;

namespace JNNJMods.CrabCheat.Modules.Movement;

[CheatModule]
public class ClickTPModule : Module
{
	public bool Enabled;


	public ClickTPModule() : base("ClickTP", TabID.Movement)
	{
	}

	public override void RenderGUIElements()
	{
		ImGui.Checkbox(Name, ref Enabled);
	}

	private static Vector3 FindTpPos()
	{
		Transform playerCam = Instances.PlayerMovement.playerCam;

		bool rayHitStuff = Physics.Raycast(playerCam.position, playerCam.forward, out RaycastHit raycastHit, 5000f, GameManager.Instance.whatIsGround);
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
		if (!InGame)
			return;

		if (Cheat.Instance.Shown)
			return;

		if (!Enabled)
			return;

		if (!Input.GetKeyDown(KeyCode.Mouse1))
			return;

		Instances.PlayerMovement.GetRb().position = FindTpPos();
	}

}
