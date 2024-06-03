using ImGuiNET;
using JNNJMods.CrabCheat.Patches;
using JNNJMods.CrabCheat.Rendering;

namespace JNNJMods.CrabCheat.Modules.Player;

[CheatModule]
public class NoFallModule : Module
{
	public bool Enabled;

	public NoFallModule() : base("NoFall", TabID.Player)
	{

	}

	public override void RenderGUIElements()
	{
		if (ImGui.Checkbox(Name, ref Enabled))
			PlayerStatusPatch.NoFall = Enabled;
	}
}
