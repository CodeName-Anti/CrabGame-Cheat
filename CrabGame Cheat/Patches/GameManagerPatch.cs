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
        public static bool PunchPlayer(ulong NBJFFFOCIFF, ulong BPPACELKHNO, Vector3 JMPOCAKFDAH)
        {
            return !(NoPush && BPPACELKHNO == SteamUser.GetSteamID().m_SteamID);
        }

    }
}
