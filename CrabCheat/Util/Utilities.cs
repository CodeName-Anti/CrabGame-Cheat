using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using UnityEngine;

namespace JNNJMods.CrabCheat.Util;

public static class Utilities
{
	public static string Format(this Type[] types)
	{
		// Return Empty string if no types are in array
		if (types.Length == 0)
			return string.Empty;

		string formatted = "";

		foreach (Type type in types)
		{
			formatted += type.FullName + ", ";
		}

		// Remove last 2 characters
		return formatted[..^2];
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
		Dictionary<TKey, TValue> sysDic = [];

		foreach (Il2CppSystem.Collections.Generic.KeyValuePair<TKey, TValue> entry in cppDic)
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
		Il2CppSystem.Collections.Generic.List<T> cppList = new();

		foreach (T elem in sysList)
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
		using HttpClient client = new();

		HttpRequestMessage request = new(HttpMethod.Get, url);

		using FileStream fs = new(fileName, FileMode.OpenOrCreate);

		client.Send(request).Content.CopyToAsync(fs).GetAwaiter().GetResult();
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
			assembly = Assembly.GetExecutingAssembly();

		string version = assembly.GetName().Version.ToString();
		for (int i = 0; i < 100; i++)
		{
			if (version.EndsWith(".0"))
				version = version.Trim()[..(version.Length - 2)];
			else
				break;
		}

		// Read a single ".0" to make it look nicer
		if (pretty && !version.Contains('.'))
		{
			version += ".0";
		}

		return version;
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
