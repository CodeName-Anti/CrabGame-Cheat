using ImGuiNET;
using JNNJMods.CrabCheat.Patches;
using JNNJMods.CrabCheat.Rendering;

namespace JNNJMods.CrabCheat.Modules.Other;

[CheatModule]
public class RLGLGodMode : Module
{
	public bool Enabled;

	public RLGLGodMode() : base("RedLight GreenLight GodMode", TabID.Other)
	{
	}

	public override void RenderGUIElements()
	{
		if (ImGui.Checkbox(Name, ref Enabled))
			GameModeRedLightPatch.RLGLGodMode = Enabled;
	}
}
