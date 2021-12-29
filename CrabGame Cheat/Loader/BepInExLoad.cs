using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using JNNJMods.CrabGameCheat.Util;
using System;
using System.Reflection;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace JNNJMods.CrabGameCheat.Loader
{
    [BepInPlugin(Constants.GUID, "CrabGame Cheat", Constants.Version)]
    public class BepInExLoader : BasePlugin
    {
        public static BepInExLoader Instance { get; private set; }

        public Harmony HarmonyInstance { get; private set; }

        public Cheat Cheat { get; private set; }
        private Cheat cheat;

        public override void Load()
        {
            Instance = this;

            // Initialize Harmony
            try
            {
                HarmonyInstance = new Harmony(Constants.GUID);

                CustomPatchAll();
            }
            catch (Exception ex)
            {
                CheatLog.Error("Harmony patching error: " + ex.ToString());
            }

            // Register Cheat in IL2CPP
            ClassInjector.RegisterTypeInIl2Cpp<Cheat>();

            // Create GameObject
            GameObject obj = new("JNNJs CrabGame Cheat");
            UnityEngine.Object.DontDestroyOnLoad(obj);
            obj.hideFlags |= HideFlags.HideAndDontSave;

            // Add CheatObject Component
            cheat = obj.AddComponent<Cheat>();

            cheat.OnLoad();
        }

        private void CustomPatchAll()
        {
            // Get all types from current assembly.
            AccessTools.GetTypesFromAssembly(Assembly.GetExecutingAssembly()).Do(type =>
            {
                try
                {
                    // Patch type
                    new PatchClassProcessor(HarmonyInstance, type).Patch();
                }
                catch (Exception ex)
                {
                    CheatLog.Error($"Failed to patch \"{type.FullName}\": {ex}!");
                }
            });
        }

    }
}