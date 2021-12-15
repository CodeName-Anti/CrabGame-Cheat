using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using UnityEngine;

namespace JNNJMods.CrabGameCheat.Util
{
    public static class Utilities
    {
        public static string Format(this Type[] types)
        {
            if (types.Length == 0)
                return string.Empty;

            string formatted = "";

            foreach (var type in types)
            {
                formatted += type.FullName + ", ";
            }

            return formatted.Substring(0, formatted.Length - 2);
        }

        public static bool GetKeyDown(int key)
        {
            return Input.GetKeyDown((KeyCode)key);
        }

        public static bool GetKey(int key)
        {
            return Input.GetKey((KeyCode)key);
        }

        public static Dictionary<TKey, TValue> ToSystem<TKey, TValue>(this Il2CppSystem.Collections.Generic.Dictionary<TKey, TValue> cppDic)
        {
            Dictionary<TKey, TValue> sysDic = new();

            foreach (var entry in cppDic)
            {
                sysDic.Add(entry.key, entry.value);
            }

            return sysDic;
        }

        /// <summary>
        /// Converts a System list to Il2CPP
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sysList"></param>
        /// <returns></returns>
        public static Il2CppSystem.Collections.Generic.List<T> ToIL2CPP<T>(this List<T> sysList)
        {
            var cppList = new Il2CppSystem.Collections.Generic.List<T>();

            foreach (var elem in sysList)
            {
                cppList.Add(elem);
            }

            return cppList;
        }

        /// <summary>
        /// Downloads a File from an URL
        /// </summary>
        /// <param name="fileName">File Location</param>
        /// <param name="url">Download URL</param>
        public static void DownloadFile(string fileName, string url)
        {
            using var client = new WebClient();
            client.DownloadFile(url, fileName);
        }

        /// <summary>
        /// Formats the version of an Assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="pretty"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets bytes from an embedded resource.
        /// </summary>
        /// <param name="embeddedResource"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static byte[] GetResourceBytes(string embeddedResource)
        {
            using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(embeddedResource))
            {
                byte[] buffer = manifestResourceStream != null ? new byte[(int)manifestResourceStream.Length] :
                    throw new Exception(embeddedResource + " is not found in Embedded Resources.");
                manifestResourceStream.Read(buffer, 0, (int)manifestResourceStream.Length);
                if (buffer.Length > 1000)
                    return buffer;
            }
            return null;
        }

        public static string GetAssemblyLocation()
        {
            return GetAssemblyLocation(Assembly.GetExecutingAssembly());
        }

        public static string GetAssemblyLocation(Assembly assembly)
        {
            return Path.GetDirectoryName(assembly.Location);
        }

    }
}
