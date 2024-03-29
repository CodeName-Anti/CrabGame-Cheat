﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace JNNJMods.CrabGameCheat.Util.KeyBinds
{
    public class KeyBindManager
    {
        [JsonIgnore]
        public static KeyBindManager Instance { get; private set; }

        public Dictionary<string, KeyBind> KeyBinds { get; private set; } = new();

        public KeyBindManager() => Instance = this;


        private void GetKeyBinds()
        {
            foreach (var module in Cheat.Instance.config.Modules)
            {
                // Get KeyBind from Module
                var bind = module.GetKeyBinds();

                // Make sure correct Name is set
                bind.Key = module.Name;

                KeyBinds[module.Name] = bind;
            }
        }

        private void SetKeyBinds()
        {
            foreach (var module in Cheat.Instance.config.Modules)
            {
                if (module == null) continue;

                // Add new KeyBind if it doesn't exist already
                if (!KeyBinds.ContainsKey(module.Name) || KeyBinds[module.Name] == null)
                    KeyBinds.Add(module.Name, new KeyBind(module.Name));

                try
                {
                    module.SetKeyBinds(KeyBinds[module.Name]);
                }
                catch (Exception)
                {
                    KeyBinds[module.Name] = module.GetKeyBinds();
                }
            }
        }

        public void WriteToFile(string file)
        {
            GetKeyBinds();

            File.WriteAllText(file, JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        public static KeyBindManager ReadFromFile(string file)
        {
            KeyBindManager manager = null;
            try
            {
                manager = JsonConvert.DeserializeObject<KeyBindManager>(File.ReadAllText(file));
            }
            catch (Exception) { }

            if (manager == null)
            {
                manager = new();

                manager.WriteToFile(file);
            }

            Instance = manager;
            manager.SetKeyBinds();

            return manager;
        }
    }
}
