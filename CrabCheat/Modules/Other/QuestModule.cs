using ImGuiNET;
using JNNJMods.CrabCheat.Rendering;
using JNNJMods.CrabCheat.Util;

namespace JNNJMods.CrabCheat.Modules.Other;

[CheatModule]
public class QuestModule : Module
{
	private static PlayerSave Save => SaveManager.Instance.state;

	public QuestModule() : base("Quest", TabID.Other)
	{
	}

	public override void RenderGUIElements()
	{
		if (ImGui.Button("Complete Daily Quest"))
			UnityMainThreadDispatcher.Enqueue(CompleteDaily);

		if (ImGui.Button("Reset Daily Cooldown"))
			UnityMainThreadDispatcher.Enqueue(ResetDailyCooldown);
	}


	private void ResetDailyCooldown()
	{
		Save.nextQuestAvailableTime = Il2CppSystem.DateTime.Now;
		SaveManager.Instance.Save();
	}

	private void CompleteDaily()
	{
		QuestManager.Instance.CompleteQuest();

		SaveManager.Instance.state.AddQuestProgress(187);

		SaveManager.Instance.Save();
	}
}
