using HarmonyLib;
using JNNJMods.CrabGameCheat.Util;
using Steamworks;
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
            Spectators[spectatorId] = targetId;


            if(targetId == SteamClient.SteamId.Value)
            {

                string spectatorMessage;

                PlayerManager spectator = GameManager.Instance.spectators[spectatorId];
                PlayerManager target = GameManager.Instance.activePlayers[targetId];

                if (targetId == SteamClient.SteamId.Value)
                {
                    spectatorMessage = "You are";
                } else
                    spectatorMessage = target.username + " is";

                bool owner = SteamManager.Instance.lobbyOwnerSteamId == spectatorId;

                spectatorMessage += " being spectated by " + (owner ? "<color=red>" : "") + spectator.username + (owner ? "</color>" : "");

                CheatLog.LogChatBox(spectatorMessage);
            }

            return true;
        }
    }
}
