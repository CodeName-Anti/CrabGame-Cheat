using HarmonyLib;
using JNNJMods.CrabGameCheat.Loader;
using System;
using System.Linq;
using System.Reflection;

namespace JNNJMods.CrabGameCheat.Util
{
    public static class HarmonyMethodFinder
    {

        private static Type[] GetParams(this MethodBase mBase)
        {
            return mBase.GetParameters().Select(param => param.ParameterType).ToArray();
        }

        public static void RegisterPatch(Type t, Type[] parameters, MethodInfo customPatch = null)
        {
            Harmony harmony = BepInExLoader.Instance.HarmonyInstance;

            MethodInfo prefix = typeof(HarmonyMethodFinder).GetMethod(nameof(HarmonyMethodFinder.Patch));

            foreach (MethodInfo info in t.GetMethods())
            {                
                if (info.GetParams() == parameters)
                {
                    harmony.Patch(info, new HarmonyMethod(customPatch ?? prefix));
                }

            }
        }

        private static void Patch(MethodBase __originalMethod)
        {
            CheatLog.Error("Found method: " + __originalMethod.Name);
        }

    }
}
