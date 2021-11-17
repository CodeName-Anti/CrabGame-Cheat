using HarmonyLib;
using SteamworksNative;
using UnityEngine;

namespace JNNJMods.CrabGameCheat.Patches
{
    [HarmonyPatch(typeof(MonoBehaviourPublicDi2UIObacspDi2UIObUnique))]
    public static class GameManagerPatch
    {
        public static bool NoPush;

        [HarmonyPrefix]
        [HarmonyPatch(nameof(MonoBehaviourPublicDi2UIObacspDi2UIObUnique.PunchPlayer))]
        public static bool PunchPlayer(ulong param_1, ulong param_2, Vector3 param_3)
        {
            return !(NoPush && param_2 == SteamUser.GetSteamID().m_SteamID);
        }

    }
}
