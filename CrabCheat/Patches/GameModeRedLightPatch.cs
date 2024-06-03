using HarmonyLib;
using SteamworksNative;

namespace JNNJMods.CrabCheat.Patches;

[HarmonyPatch(typeof(GameModeRedLight))]
public static class GameModeRedLightPatch
{
	public static bool RLGLGodMode;

	[HarmonyPatch(nameof(GameModeRedLight.Method_Private_Boolean_UInt64_0))]
	[HarmonyPrefix]
	public static bool CanSeePlayer([HarmonyArgument("param_1")] ulong playerId, ref bool __result)
	{
		if (RLGLGodMode && playerId == SteamUser.GetSteamID().m_SteamID)
		{
			__result = false;
			return false;
		}

		return true;
	}

}
