using ImGuiNET;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;

using SysVec2 = System.Numerics.Vector2;
using SysVec3 = System.Numerics.Vector3;
using SysVec4 = System.Numerics.Vector4;

namespace JNNJMods.CrabCheat.Util;

public static class ImGuiExtensions
{

	public static SysVec2 ToSysVec(this Vector2 vector)
	{
		return new SysVec2(vector.x, vector.y);
	}

	public static SysVec3 ToSysVec(this Vector3 vector)
	{
		return new SysVec3(vector.x, vector.y, vector.z);
	}

	public static SysVec2 ToSysVec2(this Vector3 vector)
	{
		return new SysVec2(vector.x, vector.y);
	}

	public static SysVec4 ToSysVec(this Color color)
	{
		return new SysVec4(color.r, color.g, color.b, color.a);
	}

	public static uint ToImguiColor(this SysVec4 vector)
	{
		return ImGui.ColorConvertFloat4ToU32(vector);
	}

	public static uint ToImGuiColor(this Color color)
	{
		return color.ToSysVec().ToImguiColor();
	}

	private static IntPtr GetFontDataFromResources(string resourceName, Assembly assembly, out int fontDataLength)
	{
		using Stream fontStream = assembly.GetManifestResourceStream(resourceName);

		byte[] fontData = new byte[fontStream.Length];
		fontStream.Read(fontData, 0, (int)fontStream.Length);

		IntPtr fontPtr = ImGui.MemAlloc((uint)fontData.Length);
		Marshal.Copy(fontData, 0, fontPtr, fontData.Length);

		fontDataLength = fontData.Length;

		return fontPtr;
	}

	public static unsafe ImFontPtr LoadFontFromResources(this ImFontAtlasPtr fontAtlas, string resourceName, Assembly assembly, float fontSize)
	{
		IntPtr fontPtr = GetFontDataFromResources(resourceName, assembly, out int fontDataLength);

		return fontAtlas.AddFontFromMemoryTTF(fontPtr, fontDataLength, fontSize);
	}

	public static unsafe ImFontPtr LoadFontFromResources(this ImFontAtlasPtr fontAtlas, string resourceName, Assembly assembly, float fontSize, ImFontConfigPtr fontConfig)
	{
		IntPtr fontPtr = GetFontDataFromResources(resourceName, assembly, out int fontDataLength);

		return fontAtlas.AddFontFromMemoryTTF(fontPtr, fontDataLength, fontSize, fontConfig);
	}

	public static unsafe ImFontPtr LoadFontFromResources(this ImFontAtlasPtr fontAtlas, string resourceName, Assembly assembly, float fontSize, ImFontConfigPtr fontConfig, IntPtr glyphRanges)
	{
		IntPtr fontPtr = GetFontDataFromResources(resourceName, assembly, out int fontDataLength);

		return fontAtlas.AddFontFromMemoryTTF(fontPtr, fontDataLength, fontSize, fontConfig, glyphRanges);
	}

	public static unsafe ImFontPtr LoadIconFontFromResources(this ImFontAtlasPtr fontAtlas, string resourceName, Assembly assembly, float size, (ushort, ushort) range)
	{
		ImFontConfigPtr configuration = ImGuiNative.ImFontConfig_ImFontConfig();

		configuration.MergeMode = true;
		configuration.PixelSnapH = true;
		configuration.GlyphOffset = new SysVec2(0, 1);

		GCHandle rangeHandle = GCHandle.Alloc(new ushort[]
		{
			range.Item1,
			range.Item2,
			0
		}, GCHandleType.Pinned);

		try
		{
			return fontAtlas.LoadFontFromResources(resourceName, assembly, size, configuration, rangeHandle.AddrOfPinnedObject());
		}
		finally
		{
			configuration.Destroy();

			if (rangeHandle.IsAllocated)
			{
				rangeHandle.Free();
			}
		}
	}

}
