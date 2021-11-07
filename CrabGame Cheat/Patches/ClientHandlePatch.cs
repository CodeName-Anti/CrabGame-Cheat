using HarmonyLib;
using JNNJMods.CrabGameCheat.Util;
using SteamworksNative;
using System.Collections.Generic;

namespace JNNJMods.CrabGameCheat.Patches
{
    [HarmonyPatch(typeof(ClientHandle))]
    public static class ClientHandlePatch
    {
        public static Dictionary<ulong, ulong> Spectators = new Dictionary<ulong, ulong>();

        [HarmonyPrefix]
        [HarmonyPatch("SpectatingWho")]
        public static bool Prefix(Packet packet)
        {
            var spectatorId = packet.ReadUlong(true);
            var targetId = packet.ReadUlong(true);

            packet.Reset();
            Spectators[spectatorId] = targetId;

            if(targetId == SteamUser.GetSteamID().m_SteamID)
            {
                PlayerManager spectator = GameManager.Instance.spectators[spectatorId];
                PlayerManager target = GameManager.Instance.activePlayers[targetId];

                bool owner = SteamManager.Instance.lobbyOwnerSteamId.m_SteamID == spectatorId;
                string spectatorMessage = "You are being spectated by " + (owner ? "<color=red>" : "") + spectator.username + (owner ? "</color>" : "");

                CheatLog.LogChatBox(spectatorMessage);
            }

            return true;
        }
    }
}
