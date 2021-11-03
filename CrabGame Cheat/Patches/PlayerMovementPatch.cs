using HarmonyLib;

namespace JNNJMods.CrabGameCheat.Patches
{
    [HarmonyPatch(typeof(PlayerMovement))]
    public static class PlayerMovementPatch
    {

        [HarmonyPatch("PushPlayer")]
        [HarmonyPrefix]
        public static bool PushPlayer()
        {
            return !GameManagerPatch.NoPush;
        }

    }
}
