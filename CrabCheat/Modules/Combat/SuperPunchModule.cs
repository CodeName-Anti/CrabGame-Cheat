using ImGuiNET;
using JNNJMods.CrabCheat.Patches;
using JNNJMods.CrabCheat.Rendering;

namespace JNNJMods.CrabCheat.Modules.Combat;

[CheatModule]
public class SuperPunchModule : Module
{
	public bool Enabled;
	public float Multiplier = 0f;

	public SuperPunchModule() : base("Super Punch", TabID.Combat)
	{
	}

	public override void RenderGUIElements()
	{
		if (ImGui.Checkbox(Name, ref Enabled))
		{
			GameManagerPatch.SuperPunch = Enabled;
		}

		ImGui.SameLine();

		if (ImGui.SliderFloat("Multiplier", ref Multiplier, 0, 10))
		{
			GameManagerPatch.SuperPunchMultiplier = Multiplier;
		}
	}
}
