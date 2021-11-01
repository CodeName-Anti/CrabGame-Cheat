using System.Net;
using System.Reflection;

namespace JNNJMods.CrabGameCheat.Util
{
    public static class Utilities
    {
        /// <summary>
        /// Downloads a File from an URL
        /// </summary>
        /// <param name="fileName">File Location</param>
        /// <param name="url">Download URL</param>
        public static void DownloadFile(string fileName, string url)
        {
            using (var client = new WebClient())
            {
                client.DownloadFile(url, fileName);
            }
        }

        public static string FormatAssemblyVersion(Assembly assembly, bool pretty = false)
        {
            if (assembly == null)
            {
                assembly = Assembly.GetExecutingAssembly();
            }

            string version = assembly.GetName().Version.ToString();
            for (int i = 0; i < 100; i++)
            {
                if (version.EndsWith(".0"))
                    version = version.Trim().Substring(0, version.Length - 2);
                else
                    break;
            }

            //Readd a single ".0" to make it look nicer
            if (pretty && !version.Contains("."))
            {
                version += ".0";
            }

            return version;
        }
    }
}
