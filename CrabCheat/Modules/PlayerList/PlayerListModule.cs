using HarmonyLib;
using ImGuiNET;
using JNNJMods.CrabCheat.Rendering;
using JNNJMods.CrabCheat.Translators;
using JNNJMods.CrabCheat.Util;
using SteamworksNative;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JNNJMods.CrabCheat.Modules.PlayerList;

[CheatModule]
public class PlayerListModule : Module
{

	private List<PlayerContainer> players = [];

	public class PlayerContainer
	{
		public string Name;
		public float Distance;

		public PlayerManager Player;

		public bool Selected;

		public PlayerContainer(PlayerManager player)
		{
			Player = player;
			UpdateData();
		}

		public void UpdateData()
		{
			Name = Player.username;
			Distance = Vector3.Distance(Instances.PlayerMovement.transform.position, Player.transform.position);
		}

	}

	public PlayerListModule() : base("Player list", TabID.PlayerList) { }

	public override void RenderGUIElements()
	{

		if (ImGui.Button("Select all"))
		{
			players.ForEach(i => i.Selected = true);
		}

		ImGui.SameLine();

		if (ImGui.Button("Deselect all"))
		{
			players.ForEach(i => i.Selected = false);
		}

		if (ImGui.BeginTable("playerTable", 2, ImGuiTableFlags.Borders | ImGuiTableFlags.NoBordersInBody | ImGuiTableFlags.Sortable))
		{
			ImGui.TableSetupColumn("Name");
			ImGui.TableSetupColumn("Distance");

			ImGui.TableHeadersRow();

			SortTable();

			foreach (PlayerContainer item in players)
			{
				ImGui.TableNextRow();
				ImGui.TableNextColumn();

				ImGui.Selectable(item.Name, ref item.Selected, ImGuiSelectableFlags.SpanAllColumns);

				ImGui.TableNextColumn();

				ImGui.Text(item.Distance.ToString());
			}

			ImGui.EndTable();
		}

		ImGui.SeparatorText("Player actions");

		if (ImGui.Button("Kill"))
		{
			Action<PlayerManager> killAction;

			if (SteamManager.Instance.IsLobbyOwner())
				killAction = player => ServerSend.PlayerDied(player.steamProfile.m_SteamID, 0, new Vector3());
			else
				// might not work
				killAction = player => ClientSend.DamagePlayer(player.steamProfile.m_SteamID, int.MaxValue, new Vector3(), 0, 0);

			DoPlayerAction(killAction);
		}

		if (ImGui.Button("Teleport to player"))
		{
			DoPlayerAction(player => Instances.PlayerMovement.transform.position = player.transform.position);
		}

		if (ImGui.Button("Open profile"))
		{
			DoPlayerAction(player => SteamFriends.ActivateGameOverlayToUser("steamid", player.steamProfile));
		}

		// Possible feature: Send packets that players died to spawn a lot of ragdolls
	}

	public override void Update()
	{
		if (!InGame)
			return;

		if (Cheat.Instance.renderer.CurrentTabId != TabID.PlayerList)
			return;

		players.RemoveAll(container => !GameManager.Instance.activePlayers.ContainsValue(container.Player));

		foreach (PlayerManager player in GameManager.Instance.activePlayers.values)
		{
			IEnumerable<PlayerContainer> result = players.Where(container => container.Player.steamProfile.m_SteamID == player.steamProfile.m_SteamID);

			if (result.Any())
			{
				result.First().UpdateData();
			}
			else
			{
				players.Add(new PlayerContainer(player));
			}
		}
	}

	private void DoPlayerAction(Action<PlayerManager> action)
	{
		players
			.Where(container => container.Selected)
			.Do(container => UnityMainThreadDispatcher.Enqueue(() => action(container.Player)));
	}

	// Unsafe because the ImGui.NET api is stupid af
	private unsafe void SortTable()
	{
		ImGuiTableSortSpecsPtr sortSpecs = ImGui.TableGetSortSpecs();

		if (sortSpecs.SpecsDirty)
		{
			ImGuiTableSortSpecs* pSortSpecs = sortSpecs.NativePtr;

			for (int i = 0; i < pSortSpecs->SpecsCount; i++)
			{
				ImGuiTableColumnSortSpecs* columnSortSpec = &pSortSpecs->Specs[i];

				SortColumn(ref columnSortSpec);
			}

			sortSpecs.SpecsDirty = false;
		}
	}

	private unsafe void SortColumn(ref ImGuiTableColumnSortSpecs* columnSortSpecs)
	{
		ImGuiSortDirection sortDirection = columnSortSpecs->SortDirection;

		// No need to sort
		if (sortDirection == ImGuiSortDirection.None)
			return;

		switch (columnSortSpecs->ColumnIndex)
		{
			// Name column
			case 0:
				SortByName(sortDirection);
				break;

			// Distance column
			case 1:
				SortByDistance(sortDirection);
				break;

			// No other column to sort
			default:
				break;
		}
	}

	private void SortByName(ImGuiSortDirection sortDirection)
	{
		players = sortDirection == ImGuiSortDirection.Ascending
			? players.OrderBy(i => i.Name).ToList()
			: players.OrderByDescending(i => i.Name).ToList();
	}

	private void SortByDistance(ImGuiSortDirection sortDirection)
	{
		players = sortDirection == ImGuiSortDirection.Ascending
			? players.OrderBy(i => i.Distance).ToList()
			: players.OrderByDescending(i => i.Distance).ToList();
	}

}
