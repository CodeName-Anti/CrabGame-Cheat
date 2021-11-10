using HarmonyLib;
using UnityEngine;

namespace JNNJMods.CrabGameCheat.Patches
{
    [HarmonyPatch(typeof(PlayerStatus))]
    public static class PlayerStatusPatch
    {

        public static bool
            GodMode,
            NoFall;

        [HarmonyPrefix]
        [HarmonyPatch(nameof(PlayerStatus.DamagePlayer))]
        public static bool DamagePlayer(int HCKPONNGGDO, Vector3 FBPHGLGCLBA, ulong NKLGABMFOHI, int EPJLKPEMLNP)
        {
            int itemId = EPJLKPEMLNP;
            if (NoFall && itemId == -2)
            {
                return false;
            }

            return !GodMode;
        }

    }
}
