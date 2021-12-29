using BepInEx;
using System.IO;

namespace JNNJMods.CrabGameCheat.Util
{
    public class ConfigPaths
    {

        public static readonly string ConfigDirectory = Paths.ConfigPath;

        public static readonly string KeyBindFile =
            Path.Combine(ConfigDirectory, "KeyBinds.json");

    }
}
