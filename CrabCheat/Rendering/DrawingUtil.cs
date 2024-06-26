﻿using UnityEngine;

namespace JNNJMods.CrabCheat.Rendering;

public class DrawingUtil
{

	public static GUIStyle GetTextStyle(int fontSize, Color textColor)
	{
		GUIStyle style = new(GUI.skin.label)
		{
			font = Resources.GetBuiltinResource<Font>("Arial.ttf"),
			fontSize = fontSize,
			alignment = TextAnchor.MiddleCenter,
		};

		style.normal.textColor = textColor;

		return style;
	}

	public static int GetFontSizeForResolution(int fontSize)
	{
		return Mathf.Min(Mathf.FloorToInt(Screen.width * fontSize / 1000), Mathf.FloorToInt(Screen.height * fontSize / 1000));
	}

	public static GUIStyle GetTextStyle(int fontSize)
	{
		return GetTextStyle(fontSize, Color.white);
	}

	public static Rect CenteredTextRect(string text, int fontSize)
	{
		GUIStyle style = GetTextStyle(fontSize);

		// Calculate text size with matching Rect
		Vector2 size = style.CalcSize(new GUIContent(text));
		float textWidth = size.x * 2;
		float textHeight = size.y * 2;

		return new Rect((Screen.width / 2) - (textWidth / 2), (Screen.height / 2) - size.y,
			textWidth, textHeight);
	}

	public static Rect CalcTextSize(float x, float y, GUIStyle style, string text)
	{
		Vector2 size = style.CalcSize(new GUIContent(text));
		float textWidth = size.x * 2;
		float textHeight = size.y * 2;

		return new Rect(x, y, textWidth, textHeight);
	}

	public static void DrawText(string text, float x, float y, int fontSize, Color textColor)
	{
		GUIStyle style = GetTextStyle(fontSize, textColor);

		GUI.Label(CalcTextSize(x, y, style, text), text, style);
	}

	public static void DrawText(string text, Rect pos, int fontSize, Color textColor)
	{
		GUIStyle style = GetTextStyle(fontSize, textColor);

		GUI.Label(pos, text, style);
	}

	public static void DrawCenteredText(string text, int fontSize, Color textColor)
	{
		DrawText(text, CenteredTextRect(text, fontSize), fontSize, textColor);
	}

	public static readonly Color TransparentBlack = new(0, 0, 0, 0.3f);

	/// <summary>
	/// Makes the Input color Transparent
	/// </summary>
	/// <param name="color">Color to make Transparent</param>
	/// <returns>Transparent version of the Input Color</returns>
	public static Color MakeColorTransparent(Color color)
	{
		color.a = 0.3f;
		return color;
	}

	/// <summary>
	/// Draw a Color at a location
	/// </summary>
	/// <param name="color"></param>
	/// <param name="rect"></param>
	public static void DrawColor(Color color, Rect rect)
	{
		// Create new Texture
		Texture2D tex = new(1, 1);

		// Set Color of Texture
		tex.SetPixel(1, 1, color);

		tex.wrapMode = TextureWrapMode.Repeat;
		tex.Apply();

		GUI.DrawTexture(rect, tex);
	}

	/// <summary>
	/// Draws a Color fullscreen
	/// </summary>
	/// <param name="color"></param>
	public static void DrawFullScreenColor(Color color)
	{
		DrawColor(color, new Rect(-10, -10, Screen.width + 100, Screen.height + 100));
	}
}
