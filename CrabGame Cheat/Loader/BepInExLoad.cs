using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using JNNJMods.CrabGameCheat.Util;
using System;
using System.IO;
using System.IO.Compression;
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

            // Download Newtonsoft.Json
            DownloadJsonLibrary();

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

            cheat.OnLoad(HarmonyInstance);
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

        /// <summary>
        /// Downloads Newtonsoft Json.
        /// </summary>
        private void DownloadJsonLibrary()
        {
            // Paths
            string path = Utilities.GetAssemblyLocation();
            string dllLoadPath = Path.Combine(path, "Newtonsoft.Json.dll");


            if (!File.Exists(dllLoadPath))
            {
                // Temp Paths
                string tempPath = Path.Combine(Path.GetTempPath(), "CrabGame Cheat " + Guid.NewGuid().ToString());
                string zipPath = Path.Combine(tempPath, "Unzipped");

                string zip = Path.Combine(tempPath, "Json.zip");

                // Create zipPath
                Directory.CreateDirectory(zipPath);

                // Download Newtonsoft.Json.
                Utilities.DownloadFile(zip,
                    "https://github.com/JamesNK/Newtonsoft.Json/releases/download/13.0.1/Json130r1.zip");

                // Extract the zip.
                ZipFile.ExtractToDirectory(zip, zipPath);

                string dllFile = Path.Combine(zipPath, "Bin", "net45", "Newtonsoft.Json.dll");

                // Copy Newtonsoft.Json to plugins folder, to not download it every start.
                File.Copy(dllFile, dllLoadPath);

                // Load Newtonsoft.Json
                Assembly.LoadFrom(dllLoadPath);
            }
        }

    }
}