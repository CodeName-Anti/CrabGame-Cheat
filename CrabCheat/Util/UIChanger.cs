using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace JNNJMods.CrabCheat.Util;

public class UIChanger
{
	public static bool Init
	{
		get => init;
		set
		{
			init = value;
			versionUIInit = value;
			aboutUIInit = value;
		}
	}

	private static bool
		init,
		versionUIInit,
		aboutUIInit;

	private static TextMeshProUGUI GetCreditsTMP()
	{
		MonoBehaviourPublicGalomeGacrsemiGaupObUnique ui = MonoBehaviourPublicGalomeGacrsemiGaupObUnique.Instance;

		// Find CreditsWindow
		GameObject creditsWindow = ui.gameObject.GetChildren().Where(obj => obj.name.Contains("Credits")).First();

		// Find tab0
		GameObject tab0 = creditsWindow.GetChildren().Where(obj => obj.name.Contains("Tab")).First();

		// Find Content
		GameObject content = tab0.GetChildren().First().GetChildren().Where(obj => obj.name.Contains("Content")).First();

		// Find TextMeshPro
		TextMeshProUGUI textMesh = content.GetChildren().Where(obj => obj.name.Contains("Text")).First().GetComponent<TextMeshProUGUI>();

		return textMesh;
	}

	public static void OnUpdate()
	{
		if (init)
			return;

		// Change the VersionUI
		if (!versionUIInit)
		{
			try
			{
				// Find TextMeshPro
				TextMeshProUGUI versionText = VersionUI.Instance.versionText;

				// Get current version
				string gameVersion = versionText.text;

				versionText.text = $"CrabCheat {Cheat.FormattedVersion} by JNNJ Game Version {gameVersion}";
				versionUIInit = true;
				CheatLog.Msg("Version Changed");
			}
			catch (Exception) { }
		}

		// Change the AboutUI
		if (!aboutUIInit)
		{
			try
			{
				TextMeshProUGUI textMesh = GetCreditsTMP();

				// Add Custom Text
				textMesh.text +=
					"<br><br>" +
					"<color=red><size=150%>CrabCheat </size>is a Cheat made by JNNJ.</color>";

				CheatLog.Msg("Credits Changed");

				aboutUIInit = true;
			}
			catch (Exception) { }
		}

		if (versionUIInit && aboutUIInit)
		{
			init = true;
			CheatLog.Msg("UI Updated!");
		}
	}

}
