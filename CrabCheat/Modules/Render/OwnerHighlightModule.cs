using ImGuiNET;
using JNNJMods.CrabCheat.Rendering;
using JNNJMods.CrabCheat.Rendering.Outline;
using JNNJMods.CrabCheat.Util;
using System.Collections.Generic;
using UnityEngine;

namespace JNNJMods.CrabCheat.Modules.Render;

[CheatModule]
public class OwnerHighlightModule : Module
{
	public bool Enabled;

	private PlayerManager owner;

	public OwnerHighlightModule() : base("Owner Highlight", TabID.Render)
	{
	}

	public override void RenderGUIElements()
	{
		if (ImGui.Checkbox(Name, ref Enabled))
			UnityMainThreadDispatcher.Enqueue(ToggleChanged);
	}

	private void ToggleChanged()
	{
		if (!InGame)
			return;

		if (owner == null)
			owner = FindOwner();

		if (owner == null)
			return;

		foreach (GameObject obj in GetOutlines(owner))
		{
			if (obj == null)
				continue;

			if (Enabled)
				OutlineRenderer.Outline(obj, new Color(1F, 0.5333F, 0F), 7);
			else
				OutlineRenderer.UnOutline(obj);
		}
	}

	public void FixOutline()
	{
		// Remove outline
		Enabled = false;
		ToggleChanged();

		// Add new outline
		Enabled = true;
		ToggleChanged();
	}

	public override void FixedUpdate()
	{
		if (!InGame)
			return;

		if (!Enabled)
			return;

		if (owner == null)
			owner = FindOwner();

		if (owner != null && owner.GetComponent<Outline>() == null)
		{
			FixOutline();
		}
	}

	private List<GameObject> GetOutlines(PlayerManager manager)
	{
		List<GameObject> outlines = [];

		if (manager == null)
			return outlines;


		MonoBehaviourPublicGacrswGapaCoObGacrcoUnique customization = manager.playerCustomization;

		if (customization == null)
			return outlines;


		outlines.Add(customization.sweater);
		outlines.Add(customization.pants);

		return outlines;
	}

	private static PlayerManager FindOwner()
	{
		Il2CppSystem.Collections.Generic.Dictionary<ulong, PlayerManager> players = GameManager.Instance.activePlayers;

		foreach (PlayerManager player in players.Values)
		{
			if (player.steamProfile.m_SteamID == SteamManager.Instance.originalLobbyOwnerId.m_SteamID)
				return player;
		}

		return null;
	}

}
