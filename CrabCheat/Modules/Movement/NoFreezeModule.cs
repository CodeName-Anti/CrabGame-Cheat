using ImGuiNET;
using JNNJMods.CrabCheat.Rendering;

namespace JNNJMods.CrabCheat.Modules.Movement;

[CheatModule]
public class NoFreezeModule : Module
{
	public bool Enabled;

	public NoFreezeModule() : base("NoFreeze", TabID.Movement)
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

		if (!Enabled)
			return;

		PersistentPlayerData.frozen = false;
	}

}
