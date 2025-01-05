using ImGuiNET;
using JNNJMods.CrabCheat.Rendering;
using JNNJMods.CrabCheat.Translators;
using JNNJMods.CrabCheat.Util;
using UnityEngine;

namespace JNNJMods.CrabCheat.Modules.Combat;

[CheatModule]
public class InfinityPunchModule : Module
{

	public bool Enabled;

	public InfinityPunchModule() : base("Infinity Punch", TabID.Combat)
	{
	}

	public override void RenderGUIElements()
	{
		ImGui.Text("Infinity Punch should be used with");
		ImGui.SameLine();
		ImGui.TextColored(Color.red.ToSysVec(), "No Camerashake");

		ImGui.Checkbox(Name, ref Enabled);
	}

	public override void FixedUpdate()
	{
		if (!InGame)
			return;

		if (!Enabled)
			return;

		MonoBehaviourPublicObsfBoLawhSiUnique punch = Instances.PlayerMovement.punchPlayers;

		punch.field_Private_Boolean_0 = true;
		punch.field_Private_Single_0 = 3.1f;
	}

}
