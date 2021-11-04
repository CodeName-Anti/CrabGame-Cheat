using JNNJMods.CrabGameCheat.Patches;
using JNNJMods.CrabGameCheat.Util;
using JNNJMods.UI;
using JNNJMods.UI.Elements;
using Newtonsoft.Json;
using SteamworksNative;
using System.Collections.Generic;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace JNNJMods.CrabGameCheat.Modules
{
    [CheatModule]
    public class SpectatorListModule : SingleElementModule<ToggleInfo>
    {
        [JsonIgnore]
        private GUILayoutOption[] labelOptions;

        public SpectatorListModule(ClickGUI gui) : base("Spectator List", gui, WindowIDs.OTHER)
        {

        }

        public override void Init(ClickGUI gui, bool json = false)
        {
            base.Init(gui, json);

            labelOptions = new GUILayoutOption[]
            {
                GUILayout.Width(50)
            };

        }

        public override ElementInfo CreateElement(int windowId)
        {
            return new ToggleInfo(windowId, Name, true, false);
        }

        public override void OnGUI()
        {
            if (!InGame)
                return;

            if (!Element.GetValue<bool>())
                return;

            //Draw Spectator List

            GUI.Window((int)WindowIDs.SPECTATORLIST, new Rect(20, 20, 400, 300), (GUI.WindowFunction)DrawList, "SpectatorList");

        }

        void DrawList(int id)
        {
            //spectator, target
            foreach (KeyValuePair<ulong, ulong> entry in ClientHandlePatch.Spectators)
            {
                if(entry.Value == SteamUser.GetSteamID().m_SteamID)
                {
                    PlayerManager spectator = GameManager.Instance.spectators[entry.Key];
                    PlayerManager target = GameManager.Instance.activePlayers[entry.Value];

                    if(SteamUser.GetSteamID().m_SteamID == entry.Value)
                    {
                        bool owner = entry.Key == SteamManager.Instance.lobbyOwnerSteamId.m_SteamID;
                        GUILayout.Label(new GUIContent(owner ? ChatboxPatch.MakeColored(spectator.username, "red") : spectator.username), labelOptions);
                    }
                }
            }
            
        }

    }
}
