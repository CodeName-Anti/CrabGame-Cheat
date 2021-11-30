using JNNJMods.CrabGameCheat.Modules;
using JNNJMods.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace JNNJMods.CrabGameCheat.Util
{
    public class Config
    {
        [JsonIgnore]
        public static Config Instance { get; private set; }

        public List<ModuleBase> Modules { get; private set; }

        public KeyCode ClickGuiKeyBind = KeyCode.RightShift;

        public void ExecuteForModules(Action<ModuleBase> action)
        {
            Modules.ForEach(m => action.Invoke(m));
        }

        public T GetModule<T>() where T : ModuleBase
        {
            return Modules.OfType<T>().FirstOrDefault();
        }

        private List<ModuleBase> GetModules(ClickGUI gui)
        {
            List<ModuleBase> modules = new List<ModuleBase>(CheatModuleAttribute.InstantiateAll(gui));

            return modules;
        }

        public Config(ClickGUI gui)
        {
            Instance = this;
            Modules = GetModules(gui);
        }

        /// <summary>
        /// Writes the Config to a File.
        /// </summary>
        /// <param name="file"></param>
        public void WriteToFile(string file)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(file));

            File.WriteAllText(file, ToJson());
        }

        /// <summary>
        /// Reads the Config from a File.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="gui"></param>
        /// <returns></returns>
        public static Config FromFile(string file, ClickGUI gui)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(file));

            if (!File.Exists(file))
            {
                return CreateFile(file, gui);
            }

            try
            {
                return FromJson(File.ReadAllText(file), gui);
            }
            catch (JsonException)
            {
                File.Delete(file);
                return CreateFile(file, gui);
            }
        }

        private static Config CreateFile(string file, ClickGUI gui)
        {
            File.CreateText(file);

            Config config = new Config(gui);
            config.WriteToFile(file);

            return config;
        }

        /// <summary>
        /// Converts the current Instance to json.
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Reads the Config from a json string.
        /// </summary>
        /// <param name="json"></param>
        /// <param name="gui"></param>
        /// <returns></returns>
        public static Config FromJson(string json, ClickGUI gui)
        {
            try
            {
                Config instance = JsonConvert.DeserializeObject<Config>(json);

                if (instance.Modules.Count <= 0 || instance.Modules.Count != CheatModuleAttribute.GetAllModules().Length)
                {
                    instance.Modules = instance.GetModules(gui);
                } else
                    instance.ExecuteForModules(m => m.Init(gui, true));

                return Instance = instance;
            }
            catch (Exception)
            {
                return new Config(gui);
            }
        }
    }
}
