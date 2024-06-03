using HarmonyLib;
using JNNJMods.CrabCheat.Modules;
using JNNJMods.CrabCheat.Modules.Render;
using JNNJMods.CrabCheat.Rendering;
using JNNJMods.CrabCheat.Util;
using SteamworksNative;
using System;
using System.Threading.Tasks;

namespace JNNJMods.CrabCheat.Patches;

public static class SteamManagerPatch
{

	[HarmonyFind(typeof(SteamManager), typeof(LobbyEnter_t))]
	[HarmonyPrefix]
	public static void LobbyCreated()
	{
		Wait();
	}

	private static async void Wait()
	{
		await Task.Delay(3000);

		try
		{
			OwnerHighlightModule module = ModuleManager.Instance.GetModule<OwnerHighlightModule>();

			if (module.Enabled)
				module.FixOutline();
		}
		catch (Exception)
		{
			// Ignored, OwnerHighlight has less priority than the Lobby Owner tab.
		}

		//TODO: Read hiding the owner tab
		Cheat.Instance.renderer.Tabs[TabID.LobbyOwner].Enabled = SteamManager.Instance.IsLobbyOwner();
	}

}
