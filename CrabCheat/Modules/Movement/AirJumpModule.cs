using ImGuiNET;
using JNNJMods.CrabCheat.Rendering;
using JNNJMods.CrabCheat.Translators;
using JNNJMods.CrabCheat.Util;
using UnityEngine;

namespace JNNJMods.CrabCheat.Modules.Movement;

[CheatModule]
public class AirJumpModule : Module
{
	public bool Enabled;

	public AirJumpModule() : base("AirJump", TabID.Movement)
	{

	}

	public override void RenderGUIElements()
	{
		ImGui.Checkbox(Name, ref Enabled);
	}

	public override void Update()
	{
		if (!InGame)
			return;

		if (!Utilities.GetKeyDown(SaveManager.Instance.state.jump))
			return;

		if (!Enabled)
			return;

		Vector3 velocity = Instances.PlayerMovement.GetRb().velocity;

		velocity.y = 20f;

		Instances.PlayerMovement.GetRb().velocity = velocity;

	}
}
