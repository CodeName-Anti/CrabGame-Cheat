using HarmonyLib;

namespace JNNJMods.CrabCheat.Patches;

[HarmonyPatch(typeof(CameraShaker))]
public static class CameraShakerPatch
{
	public static bool NoCameraShake;

	[HarmonyPatch(nameof(CameraShaker.GunShake))]
	[HarmonyPatch(nameof(CameraShaker.PushShake))]
	[HarmonyPatch(nameof(CameraShaker.DamageShake))]
	[HarmonyPrefix]
	public static bool DisableCamShake()
	{
		return !NoCameraShake;
	}

}
