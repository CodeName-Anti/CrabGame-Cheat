using HarmonyLib;
using System.Reflection;

namespace JNNJMods.CrabCheat.Patches;

[HarmonyPatch]
public static class BepInExDetectionPatch
{

	[HarmonyPatch(typeof(MonoBehaviourPublicGataInefObInUnique), "Method_Private_Void_GameObject_Boolean_Vector3_Quaternion_0")]
	[HarmonyPatch(typeof(MonoBehaviourPublicCSDi2UIInstObUIloDiUnique), "Method_Private_Void_0")]
	[HarmonyPatch(typeof(MonoBehaviourPublicVesnUnique), "Method_Private_Void_0")]
	[HarmonyPatch(typeof(MonoBehaviourPublicObjomaOblogaTMObseprUnique), "Method_Public_Void_PDM_2")]
	[HarmonyPatch(typeof(MonoBehaviourPublicTeplUnique), "Method_Private_Void_PDM_32")]
	[HarmonyPrefix]
	public static bool Detect(MethodBase __originalMethod)
	{
		return false;
	}

}
