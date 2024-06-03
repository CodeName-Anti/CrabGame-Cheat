using ImGuiNET;
using JNNJMods.CrabCheat.Patches;
using JNNJMods.CrabCheat.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace JNNJMods.CrabCheat.Modules.LobbyOwner;

[CheatModule]
public class NextMapModule : Module
{
	private bool init;

	public bool AutomaticReset;

	private Dictionary<string, int> allMaps = [];
	private string[] mapNames;
	private int selectedIndex;

	public NextMapModule() : base("Next map", TabID.LobbyOwner) { }

	public override void RenderGUIElements()
	{
		if (!init)
			return;

		if (ImGui.Combo("Map selection", ref selectedIndex, mapNames, mapNames.Length))
		{
			if (selectedIndex == 0)
				ServerSendPatch.nextMapId = -1;
			else
				// Get id from name
				ServerSendPatch.nextMapId = allMaps[mapNames[selectedIndex]];
		}

		ImGui.SameLine();

		ImGui.Checkbox("Reset Automatically", ref AutomaticReset);

		ImGui.SetItemTooltip("Sets the map selection back to Random after the selected map has been loaded.");
	}

	public override void Update()
	{
		if (init)
			return;

		if (MapManager.Instance == null)
			return;

		allMaps = MapManager.Instance.maps.ToDictionary(m => m.mapName, m => m.id);
		mapNames = ["Random", .. MapManager.Instance.maps.Select(m => m.mapName)];

		init = true;
	}

	public void ResetSelectedMap()
	{
		if (!AutomaticReset)
			return;

		// Set selected map to random
		selectedIndex = 0;
	}

}
