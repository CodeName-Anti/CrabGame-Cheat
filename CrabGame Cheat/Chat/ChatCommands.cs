using JNNJMods.CrabGameCheat.Modules;
using JNNJMods.CrabGameCheat.Patches;
using JNNJMods.CrabGameCheat.Util;
using JNNJMods.UI.Elements;
using SteamworksNative;
using System;

namespace JNNJMods.CrabGameCheat.Chat
{
    public static class ChatCommands
    {

        private static void SendLocalMessage(object message)
        {
            CheatLog.LogChatBox(message);
        }

        private static string GetHelpMessage()
        {
            return ChatboxPatch.GetHelpMessage();
        }

        private static void SendHelpMessage()
        {
            SendLocalMessage(GetHelpMessage());
        }

        private static void InvalidSteamUser()
        {
            SendLocalMessage("<color=red>Please enter a valid User!</color>");
        }

        private static Tuple<ulong, PlayerManager> GetPlayerByName(string name, Il2CppSystem.Collections.Generic.Dictionary<ulong, PlayerManager> players)
        {

            foreach(var entry in players)
            {
                if(entry.Value.username.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return new Tuple<ulong, PlayerManager>(entry.Key, entry.Value);
                }
            }

            return null;
        }

        [ChatCommand("Toggle")]
        public static void Toggle(string message, string[] args)
        {
            foreach(ModuleBase mod in Config.Instance.Modules)
            {
                if(mod is SingleElementModule<ToggleInfo>)
                {
                    if (mod.Name.Equals(message, StringComparison.OrdinalIgnoreCase))
                    {
                        var toggle = mod as SingleElementModule<ToggleInfo>;

                        bool toggled = toggle.Element.Toggle();

                        string toggleText = ChatboxPatch.MakeColored(toggled ? "Enabled" : "Disabled", toggled ? "green" : "red");

                        CheatLog.LogChatBox(mod.Name + " is now " + toggleText);
                    }
                }
            }
        }

        [ChatCommand("Profile", "Opens the SteamProfile of an User.")]
        public static void Profile(string message, string[] args)
        {
            if(args.Length < 1)
            {
                SendHelpMessage();
                return;
            }

            if (!ulong.TryParse(args[0], out ulong id))
            {
                var active = GetPlayerByName(message, GameManager.Instance.activePlayers);
                var spectator = GetPlayerByName(message, GameManager.Instance.spectators);

                if(active != null)
                {
                    id = active.Item1;
                }
                else if(spectator != null)
                {
                    id = spectator.Item1;
                }

            }

            var cId = new CSteamID(id);

            if(!cId.IsValid())
            {
                InvalidSteamUser();
                return;
            }

            SteamFriends.ActivateGameOverlayToUser("steamid", cId);
        }
    }
}
