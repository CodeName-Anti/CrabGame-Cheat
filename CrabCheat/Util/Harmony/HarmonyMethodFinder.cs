using HarmonyLib;
using JNNJMods.CrabCheat.Util;
using System;
using System.Linq;
using System.Reflection;

namespace JNNJMods.CrabCheat.Utils;

public static class HarmonyMethodFinder
{
	private static bool ParamsEqual(Type[] params1, Type[] params2)
	{
		// Check if lenght is equal
		if (params1.Length != params2.Length)
			return false;

		// Check if values are equal
		for (int i = 0; i < params1.Length; i++)
		{
			if (params1[i] != params2[i])
				return false;
		}

		return true;
	}

	private static Type[] GetParams(this MethodBase mBase)
	{
		// Get ParameterTypes from Method
		return mBase.GetParameters().Select(param => param.ParameterType).ToArray();
	}

	public static void RegisterPatch(Type t, Type[] parameters, HarmonyMethod customPatch = null, HarmonyPatchType patchType = HarmonyPatchType.Prefix)
	{
		BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic;
		Harmony harmony = CheatPlugin.Instance.HarmonyInstance;

		foreach (MethodInfo foundMethod in t.GetMethods(flags))
		{
			if (ParamsEqual(foundMethod.GetParams(), parameters))
			{
				HarmonyMethod patchMethod = customPatch ?? new HarmonyMethod(typeof(HarmonyMethodFinder).GetMethod(nameof(Patch), flags));

				harmony.Patch(foundMethod, patchMethod);
			}

		}
	}

	private static void Patch(MethodBase __originalMethod)
	{
		CheatLog.Error("Found method: " + __originalMethod.Name);
	}

}
