using HarmonyLib;
using SteamworksNative;
using UnityEngine;

namespace JNNJMods.CrabGameCheat.Patches
{
    [HarmonyPatch(typeof(GameManager))]
    public static class GameManagerPatch
    {
        public static bool NoPush;

        [HarmonyPrefix]
        [HarmonyPatch(nameof(GameManager.PunchPlayer))]
        public static bool PunchPlayer(ulong param_1, ulong param_2, Vector3 param_3)
        {
            //Checks if pushed player is local player
            return !(NoPush && param_2 == SteamUser.GetSteamID().m_SteamID);
        }

    }
}
