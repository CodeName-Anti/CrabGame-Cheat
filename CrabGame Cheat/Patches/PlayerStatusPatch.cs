using HarmonyLib;
using UnityEngine;

namespace JNNJMods.CrabGameCheat.Patches
{
    [HarmonyPatch(typeof(PlayerStatus), "DamagePlayer")]
    public static class PlayerStatusPatch
    {

        public static bool
            GodMode,
            NoFall;

        [HarmonyPrefix]
        public static bool DamagePlayer(int dmg, Vector3 damageDir, ulong damageDoerId, int itemId)
        {
            if(NoFall && itemId == -2)
            {
                return false;
            }

            return !GodMode;
        }

    }
}
