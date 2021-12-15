using HarmonyLib;
using JNNJMods.CrabGameCheat.Loader;
using System;
using System.Linq;
using System.Reflection;

namespace JNNJMods.CrabGameCheat.Util
{
    public static class HarmonyMethodFinder
    {
        private static bool ParamsEqual(Type[] params1, Type[] params2)
        {
            if (params1.Length != params2.Length)
                return false;

            for (int i = 0; i < params1.Length; i++)
            {
                if (params1[i] != params2[i])
                    return false;

            }

            return true;
        }

        private static Type[] GetParams(this MethodBase mBase)
        {
            return mBase.GetParameters().Select(param => param.ParameterType).ToArray();
        }

        public static void RegisterPatch(Type t, Type[] parameters, HarmonyMethod customPatch = null, HarmonyPatchType patchType = HarmonyPatchType.Prefix)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic;
            Harmony harmony = BepInExLoader.Instance.HarmonyInstance;

            foreach (MethodInfo foundMethod in t.GetMethods(flags))
            {
                if (ParamsEqual(foundMethod.GetParams(), parameters))
                {
                    HarmonyMethod patchMethod = customPatch ?? new HarmonyMethod(typeof(HarmonyMethodFinder).GetMethod(nameof(HarmonyMethodFinder.Patch), flags));

                    harmony.Patch(foundMethod, patchMethod);

                    /*harmony.Patch(foundMethod,
                        patchType == HarmonyPatchType.Prefix ? patchMethod : null,
                        patchType == HarmonyPatchType.Postfix ? patchMethod : null);*/
                }

            }
        }

        private static void Patch(MethodBase __originalMethod)
        {
            CheatLog.Error("Found method: " + __originalMethod.Name);
        }

    }
}
