using HarmonyLib;
using JNNJMods.UI;
using System.Threading.Tasks;

namespace JNNJMods.CrabGameCheat.Patches
{
    [HarmonyPatch(typeof(SteamManager))]
    public static class SteamManagerPatch
    {

        [HarmonyPatch(nameof(SteamManager.Method_Private_Void_LobbyEnter_t_PDM_3))]
        [HarmonyPrefix]
        public static void LobbyCreated()
        {
            Wait();
        }

        private static async void Wait()
        {
            await Task.Delay(5000);
            ClickGUI.Instance.GetWindow((int)WindowIDs.LobbyOwner).Visible = SteamManager.Instance.IsLobbyOwner();
        }

    }
}
