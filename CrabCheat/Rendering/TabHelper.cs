using System;
using System.Collections.Generic;

namespace JNNJMods.CrabCheat.Rendering;

internal static class TabHelper
{
	private static readonly Dictionary<TabID, string> tabNameOverrides = new()
	{
		{ TabID.LobbyOwner, "Owner related" },
		{ TabID.PlayerList, "Player list" }
	};

	public static string GetTabName(TabID id)
	{
		return tabNameOverrides.TryGetValue(id, out string tabName) ? tabName : Enum.GetName(id);
	}
}
