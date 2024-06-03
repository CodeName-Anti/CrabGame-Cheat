using ImGuiNET;
using JNNJMods.CrabCheat.Rendering;
using JNNJMods.CrabCheat.Util;

namespace JNNJMods.CrabCheat.Modules.LobbyOwner;

[CheatModule]
public class ForceStartModule : Module
{
	public ForceStartModule() : base("ForceStart", TabID.LobbyOwner)
	{
	}

	public override void RenderGUIElements()
	{
		if (ImGui.Button(Name))
			UnityMainThreadDispatcher.Enqueue(ForceStartGame);
	}

	private void ForceStartGame()
	{
		if (!InGame)
			return;

		GameLoop.Instance.StartGames();
	}
}
