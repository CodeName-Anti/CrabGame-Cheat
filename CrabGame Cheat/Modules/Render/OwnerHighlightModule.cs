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
        private PlayerManager owner;

        public OwnerHighlightModule(ClickGUI gui) : base("Owner Highlight", gui, WindowIDs.Render)
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
                if(owner == null)
                    owner = FindOwner();

                if (owner == null)
                    return;

                if (toggled)
                {
                    foreach(GameObject obj in GetOutlines(owner))
                    {
                        OutlineRenderer.Outline(obj, new Color(1F, 0.5333F, 0F), 7);
                    }
                }
                else
                {
                    foreach (GameObject obj in GetOutlines(owner))
                    {
                        OutlineRenderer.UnOutline(obj);
                    }
                }
            }
        }

        public override void FixedUpdate()
        {
            if(InGame && Element.GetValue<bool>())
            {
                if (owner == null)
                    owner = FindOwner();

                if (owner.GetComponent<Outline>() == null)
                {
                    Element_ToggleChanged(true);
                }
            }
        }

        private List<GameObject> GetOutlines(PlayerManager manager)
        {
            var customization = manager.playerCustomization;
            List<GameObject> outlines = new();

            outlines.Add(customization.sweater);
            outlines.Add(customization.pants);

            return outlines;
        }

        private static PlayerManager FindOwner()
        {
            var players = GameManager.Instance.activePlayers;

            foreach(var player in players.Values)
            {
                if (player.steamProfile.m_SteamID == SteamManager.Instance.originalLobbyOwnerId.m_SteamID)
                    return player;
            }
            
            return null;
        }

    }
}
