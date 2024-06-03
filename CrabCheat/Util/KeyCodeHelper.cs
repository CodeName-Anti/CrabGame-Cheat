using ImGuiNET;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JNNJMods.CrabCheat.Util;

public static class KeyCodeHelper
{
	private static Dictionary<KeyCode, string> keyNames;

	public static Dictionary<KeyCode, string> KeyNames
	{
		get
		{
			if (keyNames == null)
			{
				InitKeyNames();
			}
			return keyNames;
		}
	}

	private static Dictionary<KeyCode, ImGuiKey> keyMap;

	public static Dictionary<KeyCode, ImGuiKey> KeyMap
	{
		get
		{
			if (keyMap == null)
			{
				InitKeyMap();
			}
			return keyMap;
		}
	}

	public static ImGuiKey ToImGuiKey(this KeyCode key)
	{
		if (!KeyMap.TryGetValue(key, out ImGuiKey imguiKey))
		{
			imguiKey = ImGuiKey.None;
		}

		return imguiKey;
	}


	private static void InitKeyNames()
	{
		keyNames = [];

		foreach (KeyCode k in Enum.GetValues(typeof(KeyCode)))
		{
			if (!keyNames.ContainsKey(k))
				keyNames.Add(k, Enum.GetName(typeof(KeyCode), k));
		}

		// replace Alpha0, Alpha1, .. and Keypad0... with "0", "1", ...
		for (int i = 0; i < 10; i++)
		{
			keyNames[(KeyCode)((int)KeyCode.Alpha0 + i)] = i.ToString();
			keyNames[(KeyCode)((int)KeyCode.Keypad0 + i)] = "Num " + i.ToString();
		}
		keyNames[KeyCode.CapsLock] = "Caps";
		keyNames[KeyCode.ScrollLock] = "Scroll";
		keyNames[KeyCode.RightShift] = "R-Shift";
		keyNames[KeyCode.RightControl] = "R-Control";
		keyNames[KeyCode.LeftShift] = "L-Shift";
		keyNames[KeyCode.LeftControl] = "L-Control";
		keyNames[KeyCode.DoubleQuote] = "\"";
		keyNames[KeyCode.Escape] = "Esc";
		keyNames[KeyCode.UpArrow] = "Up";
		keyNames[KeyCode.DownArrow] = "Down";
		keyNames[KeyCode.LeftArrow] = "Left";
		keyNames[KeyCode.RightArrow] = "Right";
	}

	private static void InitKeyMap()
	{
		keyMap = new Dictionary<KeyCode, ImGuiKey>
		{
			[KeyCode.None] = ImGuiKey.None,
			[KeyCode.Backspace] = ImGuiKey.Backspace,
			[KeyCode.Delete] = ImGuiKey.Delete,
			[KeyCode.Tab] = ImGuiKey.Tab,
			[KeyCode.Return] = ImGuiKey.Enter,
			[KeyCode.Pause] = ImGuiKey.Pause,
			[KeyCode.Escape] = ImGuiKey.Escape,
			[KeyCode.Space] = ImGuiKey.Space,
			[KeyCode.Keypad0] = ImGuiKey.Keypad0,
			[KeyCode.Keypad1] = ImGuiKey.Keypad1,
			[KeyCode.Keypad2] = ImGuiKey.Keypad2,
			[KeyCode.Keypad3] = ImGuiKey.Keypad3,
			[KeyCode.Keypad4] = ImGuiKey.Keypad4,
			[KeyCode.Keypad5] = ImGuiKey.Keypad5,
			[KeyCode.Keypad6] = ImGuiKey.Keypad6,
			[KeyCode.Keypad7] = ImGuiKey.Keypad7,
			[KeyCode.Keypad8] = ImGuiKey.Keypad8,
			[KeyCode.Keypad9] = ImGuiKey.Keypad9,
			[KeyCode.KeypadPeriod] = ImGuiKey.KeypadDecimal,
			[KeyCode.KeypadDivide] = ImGuiKey.KeypadDivide,
			[KeyCode.KeypadMultiply] = ImGuiKey.KeypadMultiply,
			[KeyCode.KeypadMinus] = ImGuiKey.KeypadSubtract,
			[KeyCode.KeypadPlus] = ImGuiKey.KeypadAdd,
			[KeyCode.KeypadEnter] = ImGuiKey.KeypadEnter,
			[KeyCode.KeypadEquals] = ImGuiKey.KeypadEqual,
			[KeyCode.UpArrow] = ImGuiKey.UpArrow,
			[KeyCode.DownArrow] = ImGuiKey.DownArrow,
			[KeyCode.RightArrow] = ImGuiKey.RightArrow,
			[KeyCode.LeftArrow] = ImGuiKey.LeftArrow,
			[KeyCode.Insert] = ImGuiKey.Insert,
			[KeyCode.Home] = ImGuiKey.Home,
			[KeyCode.End] = ImGuiKey.End,
			[KeyCode.PageUp] = ImGuiKey.PageUp,
			[KeyCode.PageDown] = ImGuiKey.PageDown,
			[KeyCode.F1] = ImGuiKey.F1,
			[KeyCode.F2] = ImGuiKey.F2,
			[KeyCode.F3] = ImGuiKey.F3,
			[KeyCode.F4] = ImGuiKey.F4,
			[KeyCode.F5] = ImGuiKey.F5,
			[KeyCode.F6] = ImGuiKey.F6,
			[KeyCode.F7] = ImGuiKey.F7,
			[KeyCode.F8] = ImGuiKey.F8,
			[KeyCode.F9] = ImGuiKey.F9,
			[KeyCode.F10] = ImGuiKey.F10,
			[KeyCode.F11] = ImGuiKey.F11,
			[KeyCode.F12] = ImGuiKey.F12,
			[KeyCode.Alpha0] = ImGuiKey._0,
			[KeyCode.Alpha1] = ImGuiKey._1,
			[KeyCode.Alpha2] = ImGuiKey._2,
			[KeyCode.Alpha3] = ImGuiKey._3,
			[KeyCode.Alpha4] = ImGuiKey._4,
			[KeyCode.Alpha5] = ImGuiKey._5,
			[KeyCode.Alpha6] = ImGuiKey._6,
			[KeyCode.Alpha7] = ImGuiKey._7,
			[KeyCode.Alpha8] = ImGuiKey._8,
			[KeyCode.Alpha9] = ImGuiKey._9,
			[KeyCode.Comma] = ImGuiKey.Comma,
			[KeyCode.Minus] = ImGuiKey.Minus,
			[KeyCode.Period] = ImGuiKey.Period,
			[KeyCode.Slash] = ImGuiKey.Slash,
			[KeyCode.Semicolon] = ImGuiKey.Semicolon,
			[KeyCode.Equals] = ImGuiKey.Equal,
			[KeyCode.LeftBracket] = ImGuiKey.LeftBracket,
			[KeyCode.Backslash] = ImGuiKey.Backslash,
			[KeyCode.RightBracket] = ImGuiKey.RightBracket,
			[KeyCode.Caret] = ImGuiKey.GraveAccent,
			[KeyCode.BackQuote] = ImGuiKey.GraveAccent,
			[KeyCode.A] = ImGuiKey.A,
			[KeyCode.B] = ImGuiKey.B,
			[KeyCode.C] = ImGuiKey.C,
			[KeyCode.D] = ImGuiKey.D,
			[KeyCode.E] = ImGuiKey.E,
			[KeyCode.F] = ImGuiKey.F,
			[KeyCode.G] = ImGuiKey.G,
			[KeyCode.H] = ImGuiKey.H,
			[KeyCode.I] = ImGuiKey.I,
			[KeyCode.J] = ImGuiKey.J,
			[KeyCode.K] = ImGuiKey.K,
			[KeyCode.L] = ImGuiKey.L,
			[KeyCode.M] = ImGuiKey.M,
			[KeyCode.N] = ImGuiKey.N,
			[KeyCode.O] = ImGuiKey.O,
			[KeyCode.P] = ImGuiKey.P,
			[KeyCode.Q] = ImGuiKey.Q,
			[KeyCode.R] = ImGuiKey.R,
			[KeyCode.S] = ImGuiKey.S,
			[KeyCode.T] = ImGuiKey.T,
			[KeyCode.U] = ImGuiKey.U,
			[KeyCode.V] = ImGuiKey.V,
			[KeyCode.W] = ImGuiKey.W,
			[KeyCode.X] = ImGuiKey.X,
			[KeyCode.Y] = ImGuiKey.Y,
			[KeyCode.Z] = ImGuiKey.Z,
			[KeyCode.Numlock] = ImGuiKey.NumLock,
			[KeyCode.CapsLock] = ImGuiKey.CapsLock,
			[KeyCode.ScrollLock] = ImGuiKey.ScrollLock,
			[KeyCode.RightShift] = ImGuiKey.RightShift,
			[KeyCode.LeftShift] = ImGuiKey.LeftShift,
			[KeyCode.RightControl] = ImGuiKey.RightCtrl,
			[KeyCode.LeftControl] = ImGuiKey.LeftCtrl,
			[KeyCode.RightAlt] = ImGuiKey.RightAlt,
			[KeyCode.LeftAlt] = ImGuiKey.LeftAlt,
			[KeyCode.Print] = ImGuiKey.PrintScreen,
			[KeyCode.Menu] = ImGuiKey.Menu
		};
	}

}
