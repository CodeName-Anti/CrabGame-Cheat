using ImGuiNET;
using JNNJMods.CrabCheat.Rendering;
using JNNJMods.CrabCheat.Util;
using SteamworksNative;

namespace JNNJMods.CrabCheat.Modules.Other;

[CheatModule]
public class GlassBreakerModule : Module
{

	public GlassBreakerModule() : base("GlassBreaker", TabID.Other)
	{
	}

	public override void RenderGUIElements()
	{
		if (!ImGui.Button(Name))
			return;

		UnityMainThreadDispatcher.Enqueue(BreakGlass);
	}

	private void BreakGlass()
	{
		if (!InGame)
			return;

		if (GameManager.Instance == null)
			return;

		if (GameManager.Instance.gameMode.freezeTimer.field_Private_Single_0 < 18)
			return;

		foreach (MonoBehaviour1PublicGasoglGaVefxUnique glass in MonoBehaviourPublicObpiInObUnique.Instance.pieces)
		{
			if (glass == null)
				continue;

			if (glass.gameObject.name.Contains("Solid"))
				continue;

			glass.LocalInteract();
			glass.AllInteract(SteamUser.GetSteamID().m_SteamID);
		}
	}
}
