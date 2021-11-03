using HarmonyLib;
using System.Collections.Generic;

namespace JNNJMods.CrabGameCheat.Patches
{
    [HarmonyPatch(typeof(ClientHandle))]
    public static class ClientHandlePatch
    {
        public static Dictionary<ulong, ulong> Spectators = new Dictionary<ulong, ulong>();
        public static bool NoFreeze;

        [HarmonyPrefix]
        [HarmonyPatch("SpectatingWho")]
        public static bool Prefix(Packet packet)
        {
            var spectator = packet.ReadUlong(true);
            var target = packet.ReadUlong(true);
            Spectators[spectator] = target;
            return true;
        }

        [HarmonyPostfix]
        [HarmonyPatch("FreezePlayers")]
        public static void FreezePlayers(Packet packet)
        {
            PersistentPlayerData.frozen = false;
        }

    }
}
