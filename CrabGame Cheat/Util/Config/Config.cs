using JNNJMods.CrabGameCheat.Modules;
using JNNJMods.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JNNJMods.CrabGameCheat.Util
{
    public class Config
    {
        public static Config Instance { get; private set; }

        public List<ModuleBase> Modules { get; private set; }

        public KeyCode ClickGuiKeyBind = KeyCode.RightShift;

        public void ExecuteForModules(Action<ModuleBase> action)
        {
            foreach (ModuleBase module in Modules)
            {
                try
                {
                    action(module);
                }
                catch (Exception ex)
                {
                    CheatLog.Error("Exception in Module \"" + module.Name + "\": " + ex.ToString());
                }
            }
        }

        public T GetModule<T>() where T : ModuleBase
        {
            return Modules.OfType<T>().FirstOrDefault();
        }

        private List<ModuleBase> GetModules(ClickGUI gui)
        {
            List<ModuleBase> modules = new(CheatModuleAttribute.InstantiateAll(gui));

            return modules;
        }

        public Config(ClickGUI gui)
        {
            Instance = this;
            Modules = GetModules(gui);
        }
    }
}
