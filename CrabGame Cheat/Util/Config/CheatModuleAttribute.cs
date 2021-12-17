using JNNJMods.CrabGameCheat.Modules;
using JNNJMods.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JNNJMods.CrabGameCheat.Util
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class CheatModuleAttribute : Attribute
    {

        public static ModuleBase[] InstantiateAll(ClickGUI gui)
        {
            return InstantiateAll(gui, Assembly.GetExecutingAssembly());
        }

        private static ModuleBase[] instances;
        public static ModuleBase[] InstantiateAll(ClickGUI gui, Assembly assembly)
        {
            if (instances == null)
            {
                List<ModuleBase> instances = new();

                // Get all Modules
                foreach (Type t in GetAllModules(assembly))
                {
                    // Instantiate Module
                    instances.Add(Activator.CreateInstance(t, new object[] { gui }) as ModuleBase);
                }

                CheatModuleAttribute.instances = instances.ToArray();
            }

            return instances;
        }

        public static Type[] GetAllModules()
        {
            return GetAllModules(Assembly.GetExecutingAssembly());
        }

        public static Type[] GetAllModules(Assembly assembly)
        {
            return assembly.GetTypes()
                      .Where(m => m.GetCustomAttributes(typeof(CheatModuleAttribute), false).Length > 0 && m.IsSubclassOf(typeof(ModuleBase)))
                      .ToArray();
        }

    }
}
