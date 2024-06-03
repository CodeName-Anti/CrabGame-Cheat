using ImGuiNET;
using JNNJMods.CrabCheat.Patches;
using JNNJMods.CrabCheat.Rendering;

namespace JNNJMods.CrabCheat.Modules.Player;

[CheatModule]
public class AntiTagModule : Module
{
	public bool Enabled;

	public AntiTagModule() : base("Anti Tag", TabID.Player)
	{
	}

	public override void RenderGUIElements()
	{
		if (ImGui.Checkbox(Name, ref Enabled))
			AntiTagPatches.AntiTag = Enabled;
	}
}
