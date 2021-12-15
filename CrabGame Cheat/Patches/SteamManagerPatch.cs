using HarmonyLib;
using JNNJMods.CrabGameCheat.Modules;
using JNNJMods.CrabGameCheat.Util;
using JNNJMods.UI;
using SteamworksNative;
using System;
using System.Threading.Tasks;

namespace JNNJMods.CrabGameCheat.Patches
{
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
                Config.Instance.GetModule<OwnerHighlightModule>().FixOutline();
            }
            catch (Exception)
            {
                // Ignored, OwnerHighlight has less priority than the Lobby Owner window.
            }

            ClickGUI.Instance.GetWindow((int)WindowIDs.LobbyOwner).Visible = SteamManager.Instance.IsLobbyOwner();
        }

    }
}
