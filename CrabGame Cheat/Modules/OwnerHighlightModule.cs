using JNNJMods.CrabGameCheat.Translators;
using JNNJMods.CrabGameCheat.Util;
using JNNJMods.Render;
using JNNJMods.UI;
using JNNJMods.UI.Elements;
using System.Collections.Generic;
using UnityEngine;

namespace JNNJMods.CrabGameCheat.Modules
{
    [CheatModule]
    public class OwnerHighlightModule : SingleElementModule<ToggleInfo>
    {
        public OwnerHighlightModule(ClickGUI gui) : base("Owner Highlight", gui, WindowIDs.RENDER)
        {
        }

        public override void Init(ClickGUI gui, bool json = false)
        {
            base.Init(gui, json);
        }

        public override ElementInfo CreateElement(int windowId)
        {
            Element = new ToggleInfo(windowId, Name, false, true);

            Element.ToggleChanged += Element_ToggleChanged;

            return Element;
        }

        private void Element_ToggleChanged(bool toggled)
        {

            if (!InGame && toggled)
            {
                Element.SetValue(false);
                return;
            }

            if (InGame)
            {
                var owner = FindOwner();

                if (owner == null)
                    return;

                if (toggled)
                {
                    List<GameObject> outlines = new();

                    var customization = owner.playerCustomization;

                    outlines.Add(customization.sweater);
                    outlines.Add(customization.pants);

                    foreach(GameObject obj in outlines)
                    {
                        OutlineRenderer.Outline(obj, new Color(1F, 0.5333F, 0F), 7);
                    }
                }
                else
                {
                    List<GameObject> outlines = new List<GameObject>();

                    var customization = owner.playerCustomization;

                    outlines.Add(customization.sweater);
                    outlines.Add(customization.pants);

                    foreach (GameObject obj in outlines)
                    {
                        OutlineRenderer.UnOutline(obj);
                    }
                }
            }
        }

        private static MonoBehaviourPublicCSstReshTrheObplBojuUnique FindOwner()
        {
            var players = GameManager.Instance.activePlayers;

            if (players.ContainsKey(SteamManager.Instance.originalLobbyOwnerId.m_SteamID))
            {
                return GameManager.Instance.activePlayers[SteamManager.Instance.originalLobbyOwnerId.m_SteamID];
            }
            else
                return null;
        }

    }
}
