using CodeStage.AntiCheat.ObscuredTypes;
using ImGuiNET;
using JNNJMods.CrabCheat.Patches;
using JNNJMods.CrabCheat.Rendering;
using JNNJMods.CrabCheat.Util;

namespace JNNJMods.CrabCheat.Modules.Player;

[CheatModule]
public class GodModeModule : Module
{
	public bool Enabled;

	public GodModeModule() : base("GodMode", TabID.Player)
	{
	}

	public override void RenderGUIElements()
	{
		if (!ImGui.Checkbox(Name, ref Enabled))
			return;

		if (InGame)
			UnityMainThreadDispatcher.Enqueue(() => PlayerStatus.Instance.currentHp = new ObscuredInt(100));

		PlayerStatusPatch.GodMode = Enabled;
	}
}
