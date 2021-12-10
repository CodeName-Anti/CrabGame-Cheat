using HarmonyLib;
using JNNJMods.CrabGameCheat.Util;
using JNNJMods.UI;
using SteamworksNative;
using System.Reflection;
using System.Threading.Tasks;

namespace JNNJMods.CrabGameCheat.Patches
{
    [HarmonyPatch(typeof(SteamManager))]
    public static class SteamManagerPatch
    {

        [HarmonyFind(typeof(SteamManager), typeof(LobbyEnter_t))]
        [HarmonyPrefix]
        public static void LobbyCreated(MethodBase __originalMethod)
        {
            CheatLog.Error($"Hello from: {__originalMethod.Name}");
            Wait();
        }

        private static async void Wait()
        {
            await Task.Delay(3000);
            ClickGUI.Instance.GetWindow((int)WindowIDs.LobbyOwner).Visible = SteamManager.Instance.IsLobbyOwner();
        }

    }
}
