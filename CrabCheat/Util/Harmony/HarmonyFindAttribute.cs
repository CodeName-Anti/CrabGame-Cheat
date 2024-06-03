using HarmonyLib;
using JNNJMods.CrabCheat.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JNNJMods.CrabCheat.Util;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class HarmonyFindAttribute : Attribute
{
	public Type TypeToPatch { get; private set; }
	public HarmonyPatchType PatchType { get; private set; } = HarmonyPatchType.Prefix;
	public Type[] ParameterTypes { get; private set; }

	private static bool Init = false;

	public HarmonyFindAttribute(Type typeToPatch, params Type[] parameterTypes)
	{
		TypeToPatch = typeToPatch;
		ParameterTypes = parameterTypes;
	}

	public static void InitPatches()
	{

		if (Init)
			return;

		// Find all Methods annotated with HarmonyFindAttribute
		Dictionary<MethodInfo, HarmonyFindAttribute[]> attributes = Assembly
			.GetExecutingAssembly()
			.GetTypes()
			.SelectMany(t => t.GetMethods())
			.Where(m => m.GetCustomAttributes(typeof(HarmonyFindAttribute), true).Length > 0)
			.ToDictionary(m => m, m => m.GetCustomAttributes<HarmonyFindAttribute>().ToArray());

		// Loop through all Methods
		foreach (KeyValuePair<MethodInfo, HarmonyFindAttribute[]> entry in attributes)
		{
			// Loop through all Attributes of Method
			foreach (HarmonyFindAttribute attrib in entry.Value)
			{
				PatchMethod(entry.Key, attrib);
			}
		}

		Init = true;

	}

	private static void PatchMethod(MethodInfo method, HarmonyFindAttribute attrib)
	{
		try
		{
			HarmonyMethodFinder.RegisterPatch(attrib.TypeToPatch, attrib.ParameterTypes, new HarmonyMethod(method), attrib.PatchType);
		}
		catch (Exception ex)
		{
			CheatLog.Error($"Exception while finding method for \"{attrib.TypeToPatch.FullName}\" with parameters {attrib.ParameterTypes.Format()}: {ex}");
		}
	}
}
