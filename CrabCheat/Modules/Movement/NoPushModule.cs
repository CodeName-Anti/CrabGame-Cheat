using ImGuiNET;
using JNNJMods.CrabCheat.Patches;
using JNNJMods.CrabCheat.Rendering;

namespace JNNJMods.CrabCheat.Modules.Movement;

[CheatModule]
public class NoPushModule : Module
{
	public bool Enabled;

	public NoPushModule() : base("NoPush", TabID.Movement)
	{
	}

	public override void RenderGUIElements()
	{
		if (ImGui.Checkbox(Name, ref Enabled))
			GameManagerPatch.NoPush = Enabled;
	}
}
