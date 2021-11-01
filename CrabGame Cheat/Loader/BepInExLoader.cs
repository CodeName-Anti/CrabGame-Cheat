#if BEPINEX
using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using BepInEx;
using BepInEx.IL2CPP;
using JNNJMods.CrabGameCheat.Util;
using UnhollowerRuntimeLib;
using UnityEngine;
using static JNNJMods.CrabGameCheat.Loader.BepInExLoader;

namespace JNNJMods.CrabGameCheat.Loader
{
    [BepInPlugin(Constants.GUID, "CrabGame Cheat", Constants.Version)]
    public class BepInExLoader : BasePlugin, ICheatLoader
    {
        public static BepInExLoader Instance { get; private set; }

        public Cheat Cheat => cheat ?? (cheat = new Cheat());
        private Cheat cheat;

        public override void Load()
        {
            Instance = this;
            //Download Newtonsoft.Json
            DownloadLibrary();

            Cheat.OnApplicationStart(new HarmonyLib.Harmony(Constants.GUID));

            CheatObject.CreateInstance(Cheat, this);

            Cheat.OnApplicationLateStart();
        }

        private void DownloadLibrary()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string dllLoadPath = Path.Combine(path, "Newtonsoft.Json.dll");
            if (!File.Exists(dllLoadPath))
            {
                string tempPath = Path.Combine(Path.GetTempPath(), "CrabGame Cheat " + Guid.NewGuid().ToString());
                string zipPath = Path.Combine(tempPath, "Unzipped");

                string zip = Path.Combine(tempPath, "Json.zip");

                Directory.CreateDirectory(zipPath);

                Utilities.DownloadFile(zip,
                    "https://github.com/JamesNK/Newtonsoft.Json/releases/download/13.0.1/Json130r1.zip");

                ZipFile.ExtractToDirectory(zip, zipPath);

                string dllFile = Path.Combine(zipPath, "Bin", "net45", "Newtonsoft.Json.dll");


                File.Copy(dllFile, dllLoadPath);

                Assembly.LoadFrom(dllLoadPath);
            }
        }

        public class CheatObject : MonoBehaviour
        {
            private Cheat cheat;

            public CheatObject(IntPtr ptr) : base(ptr) { }

            public static void CreateInstance(Cheat cheat, BepInExLoader loader)
            {
                //ClassInjector.RegisterTypeInIl2Cpp<CheatObject>();

                CheatObject obj = loader.AddComponent<CheatObject>();

                DontDestroyOnLoad(obj.gameObject);
                obj.hideFlags = HideFlags.HideAndDontSave;

                obj.cheat = cheat;
            }

            void Update()
            {
                if(cheat != null)
                    cheat.OnUpdate();
            }

            void OnGUI()
            {
                if (cheat != null)
                    cheat.OnGUI();
            }
        }

    }
}
#endif