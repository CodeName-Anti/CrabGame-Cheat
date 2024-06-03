using ImGuiNET;
using JNNJMods.CrabCheat.Rendering;
using JNNJMods.CrabCheat.Util;
using SteamworksNative;
using System.Collections.Generic;

namespace JNNJMods.CrabCheat.Modules.Combat;

[CheatModule]
public class WeaponSpawnerModule : Module
{
	private bool init;

	private List<ItemData> items = [];
	private string[] itemNames = [];

	private int selected;

	public WeaponSpawnerModule() : base("Weapon Spawner", TabID.Combat) { }

	public override void RenderGUIElements()
	{
		if (!init)
		{
			ImGui.Text("Weapon Spawner not initialized, join a game!");
			return;
		}

		ImGui.Combo("Items", ref selected, itemNames, itemNames.Length);

		ImGui.SameLine();

		if (ImGui.Button("Spawn"))
		{
			ItemData currentItem = items[selected];

			UnityMainThreadDispatcher.Enqueue(() => ServerSend.ForceGiveItem(SteamUser.GetSteamID().m_SteamID, currentItem.itemID, currentItem.objectID));
		}
	}

	public override void Update()
	{
		if (init)
			return;

		if (!InGame)
			return;

		List<string> itemNamesList = [];

		foreach (ItemData item in ItemManager.idToItem.values)
		{
			items.Add(item);
			itemNamesList.Add(item.name);
		}

		itemNames = itemNamesList.ToArray();

		init = true;
	}

}
