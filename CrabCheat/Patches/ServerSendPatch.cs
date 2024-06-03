using HarmonyLib;
using JNNJMods.CrabCheat.Modules.LobbyOwner;
using System;

namespace JNNJMods.CrabCheat.Patches;

[HarmonyPatch(typeof(ServerSend))]
public static class ServerSendPatch
{
	public static int nextMapId = -1;

	[HarmonyPatch(nameof(ServerSend.LoadMap), typeof(int), typeof(int))]
	[HarmonyPrefix]
	public static bool LoadMap([HarmonyArgument("param_0")] ref int mapId, [HarmonyArgument("param_1")] ref int gamemodeId)
	{
		if (nextMapId != -1)
		{
			mapId = nextMapId;

			try
			{
				Cheat.Instance.ModuleManager.GetModule<NextMapModule>().ResetSelectedMap();
			}
			catch (Exception) { }
		}

		return true;
	}

}
