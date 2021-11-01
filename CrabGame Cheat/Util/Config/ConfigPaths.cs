using System;
using System.IO;

namespace JNNJMods.CrabGameCheat.Util
{
    public class ConfigPaths
    {

        public static readonly string ConfigDirectory =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "JNNJMods", "CrabGame Cheat");

        public static readonly string ConfigFile =
            Path.Combine(ConfigDirectory, "config.json");

    }
}
