using JNNJMods.CrabCheat.Util;
using System;
using UnityEngine;
using static JNNJMods.CrabCheat.Rendering.DrawingUtil;

namespace JNNJMods.CrabCheat.Rendering;

public static class WelcomeScreen
{
	public static Action OnClose = delegate { };

	private static bool
		init,
		updateAvailable;

	private static int fontSize;

	public static bool Draw;

	private static bool AnyKeyDown()
	{
		Event e = Event.current;

		if (e == null)
			return false;

		return e.isKey && Input.GetKeyDown(e.keyCode);
	}

	public static void OnGUI()
	{
		if (!init)
		{
			init = true;

			// Check for Updates
			updateAvailable = UpdateChecker.UpdateAvailable;

			fontSize = GetFontSizeForResolution(40);
		}


		if (AnyKeyDown())
		{
			Draw = false;
			OnClose();
			return;
		}

		DrawWelcome();
	}

	private static void DrawWelcome()
	{
		if (!Draw)
			return;

		// Black out
		DrawFullScreenColor(TransparentBlack);

		// Position Calculation
		float divider = 2f;
		float x = (Screen.width - (Screen.width / divider)) / 2f;
		float y = (Screen.height - (Screen.height / divider)) / 2f;
		Rect rect = new(x, y, Screen.width / divider, Screen.height / divider);

		// Draw middle rect
		DrawColor(new Color(0, 1, 0, 0.6f), rect);

		string waterMark = "<b>JNNJ's CrabCheat</b>";
		Rect waterMarkRect = CenteredTextRect(waterMark, fontSize);
		waterMarkRect.y -= rect.y / 1.6f;
		GUI.Label(waterMarkRect, waterMark, GetTextStyle(fontSize, Color.black));

		string continueText = "<i><b>Press any Key to continue...</b></i>";

		Rect continueRect = CenteredTextRect(continueText, fontSize);
		continueRect.y += rect.y / 1.6f;

		GUI.Label(continueRect, continueText, GetTextStyle(fontSize, Color.black));


		DrawCenteredText(
			"Welcome to JNNJ's CrabCheat!" +
			(updateAvailable ? "\n<color=red><b>There's an update available!</b></color>" : "") + "\n" +
			"To open the ClickGUI press \"" + KeyCodeHelper.KeyNames[Cheat.Instance.ModuleManager.ClickGuiKeyBind] + "\"!", fontSize, Color.white);
	}

}
