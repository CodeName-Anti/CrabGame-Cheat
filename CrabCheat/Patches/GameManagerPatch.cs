using HarmonyLib;
using SteamworksNative;
using UnityEngine;

namespace JNNJMods.CrabCheat.Patches;

[HarmonyPatch(typeof(GameManager))]
public static class GameManagerPatch
{
	public static bool NoPush;

	public static bool SuperPunch;
	public static float SuperPunchMultiplier;

	[HarmonyPrefix]
	[HarmonyPatch(nameof(GameManager.PunchPlayer))]
	public static bool PunchPlayer([HarmonyArgument("param_1")] ref ulong fromClient, [HarmonyArgument("param_2")] ref ulong punchedPlayer, [HarmonyArgument("param_3")] ref Vector3 direction)
	{
		if (SuperPunch && fromClient == SteamUser.GetSteamID().m_SteamID)
		{
			direction *= SuperPunchMultiplier;
		}

		// Checks if pushed player is local player
		return !(NoPush && punchedPlayer == SteamUser.GetSteamID().m_SteamID);
	}

}
