using ImGuiNET;
using JNNJMods.CrabCheat.Patches;
using JNNJMods.CrabCheat.Rendering;

namespace JNNJMods.CrabCheat.Modules.Other;

[CheatModule]
public class NoCameraShakeModule : Module
{
	public bool Enabled;

	public NoCameraShakeModule() : base("No Camerashake", TabID.Other)
	{
	}

	public override void RenderGUIElements()
	{
		if (ImGui.Checkbox(Name, ref Enabled))
			CameraShakerPatch.NoCameraShake = Enabled;
	}
}
