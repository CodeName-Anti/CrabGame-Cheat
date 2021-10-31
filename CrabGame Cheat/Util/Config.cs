using JNNJMods.CrabGameCheat.Modules;
using JNNJMods.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace JNNJMods.CrabGameCheat.Util
{
    public class Config
    {
        public List<ModuleBase> Modules { get; private set; }

        public KeyCode ClickGuiKeyBind = KeyCode.RightShift;

        public void ExecuteForModules(Action<ModuleBase> action)
        {
            foreach(ModuleBase module in Modules)
            {
                action.Invoke(module);
            }
        }

        public T GetModule<T>() where T : ModuleBase
        {
            return Modules.OfType<T>().FirstOrDefault();
        }

        public static Type[] GetModuleTypes()
        {
            return Assembly.GetExecutingAssembly().GetTypes().Where(t =>

                t.IsSubclassOf(typeof(ModuleBase)) &&
                !t.IsGenericType &&
                !t.IsAbstract &&
                t.IsClass

            ).ToArray();
        }

        private List<ModuleBase> GetModules(ClickGUI gui)
        {
            List<ModuleBase> modules = new List<ModuleBase>
            {
                new ClickTPModule(gui),
                new ESPModule(gui),
                new SpawnerModule(gui),
                new FlyModule(gui),
                new AirJumpModule(gui),
                new GodModeModule(gui),
                new SpeedModule(gui),
                new GlassBreakESPModule(gui),
                new MegaSlapModule(gui),
                new MegaJumpModule(gui)
            };

            return modules;
        }

        public Config(ClickGUI gui)
        {
            Modules = GetModules(gui);
        }

        public void WriteToFile(string file)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(file));

            File.WriteAllText(file, ToJson());
        }

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
            } catch(JsonException)
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

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static Config FromJson(string json, ClickGUI gui)
        {
            try
            {
                Config instance = JsonConvert.DeserializeObject<Config>(json);

                if(instance.Modules.Count <= 0 || instance.Modules.Count != GetModuleTypes().Length)
                {
                    instance.Modules = instance.GetModules(gui);
                }

                return instance;
            } catch(Exception)
            {
                return new Config(gui);
            }
        }
    }
}
