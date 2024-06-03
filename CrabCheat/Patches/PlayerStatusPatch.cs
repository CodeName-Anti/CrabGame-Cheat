using HarmonyLib;
using UnityEngine;

namespace JNNJMods.CrabCheat.Patches;

[HarmonyPatch(typeof(PlayerStatus))]
public static class PlayerStatusPatch
{

	public static bool
		GodMode,
		NoFall;

	[HarmonyPrefix]
	[HarmonyPatch(nameof(PlayerStatus.DamagePlayer))]
	public static bool DamagePlayer(int param_1, Vector3 param_2, ulong param_3, int param_4)
	{
		//itemId -2 is Fall damage

		return (!NoFall || param_1 != -2) && !GodMode;
	}

}
