using System;
using System.IO;

namespace JNNJMods.CrabGameCheat.Util
{
    public class ConfigPaths
    {

        public static readonly string ConfigDirectory = GetConfigDirectory();
            
        public static readonly string KeyBindFile =
            Path.Combine(ConfigDirectory, "KeyBinds.json");

        private static string GetConfigDirectory()
        {
            string path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "JNNJMods",
                "CrabGame Cheat"
            );
            
            if(!IsDirectoryWritable(path))
                path = Utilities.GetAssemblyLocation();


            return path;
        }

        private static bool IsDirectoryWritable(string dirPath)
        {
            try
            {
                using (FileStream fs = File.Create(
                    Path.Combine(
                        dirPath,
                        Path.GetRandomFileName()
                    ),
                    1,
                    FileOptions.DeleteOnClose)
                )
                { }
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
