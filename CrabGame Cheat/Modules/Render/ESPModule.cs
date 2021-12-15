using JNNJMods.CrabGameCheat.Util;
using JNNJMods.UI;
using JNNJMods.UI.Elements;
using JNNJMods.UI.Utils;
using SteamworksNative;
using UnityEngine;

namespace JNNJMods.CrabGameCheat.Modules
{
    [CheatModule]
    public class ESPModule : SingleElementModule<ToggleInfo>
    {
        private ESP esp;

        public ESPModule(ClickGUI gui) : base("ESP", gui, WindowIDs.Render) { }

        public override void Init(ClickGUI gui, bool json = false)
        {
            base.Init(gui, json);
            esp = new ESP(true, Color.green, false, Color.green, true, Color.green);
        }

        public override ElementInfo CreateElement(int windowId)
        {
            Element = new ToggleInfo(windowId, "ESP", false, true);

            Element.ToggleChanged += Element_ToggleChanged;

            return Element;
        }

        private void Element_ToggleChanged(bool toggled)
        {
        }

        public override void OnGUI()
        {
            if (Element.GetValue<bool>() && InGame)
            {
                foreach (var player in GameManager.Instance.activePlayers.Values)
                {
                    if (player.steamProfile.m_SteamID == SteamUser.GetSteamID().m_SteamID)
                        continue;

                    if (player.dead)
                        continue;

                    esp.DrawSingle(player.gameObject, player.username);
                }
            }
        }

    }
}
