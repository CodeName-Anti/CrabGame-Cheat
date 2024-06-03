using ImGuiNET;
using JNNJMods.CrabCheat.Rendering;
using JNNJMods.CrabCheat.Util;

namespace JNNJMods.CrabCheat.Modules.LobbyOwner;

[CheatModule]
public class SkipRoundModule : Module
{
	public SkipRoundModule() : base("Skip Round", TabID.LobbyOwner) { }

	public override void RenderGUIElements()
	{
		if (ImGui.Button("Skip Round"))
		{
			UnityMainThreadDispatcher.Enqueue(GameLoop.Instance.NextGame);
		}
	}
}
