using HarmonyLib;
using SteamworksNative;

namespace JNNJMods.CrabCheat.Patches;

[HarmonyPatch]
public static class AntiTagPatches
{
	public static bool AntiTag;

	[HarmonyPatch(typeof(GameModeTag), nameof(GameModeTag.TagPlayer))]
	[HarmonyPatch(typeof(GameModeBombTag), nameof(GameModeBombTag.TryTagPlayer))]
	[HarmonyPrefix]
	public static bool TagPlayer([HarmonyArgument("param_1")] ulong tagger, [HarmonyArgument("param_2")] ulong tagged)
	{
		return !AntiTag || tagged != SteamUser.GetSteamID().m_SteamID;
	}

	[HarmonyPatch(typeof(GameModeHat), nameof(GameModeHat.StealHat))]
	public static bool StealHat([HarmonyArgument("param_1")] ulong stealer, [HarmonyArgument("param_2")] ulong stolenFrom)
	{
		return !AntiTag || stolenFrom != SteamUser.GetSteamID().m_SteamID;
	}

}
