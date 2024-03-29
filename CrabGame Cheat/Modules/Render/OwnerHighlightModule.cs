﻿using JNNJMods.CrabGameCheat.Util;
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
                if (owner == null)
                    owner = FindOwner();

                if (owner == null)
                    return;

                foreach (GameObject obj in GetOutlines(owner))
                {
                    if (obj == null) continue;
                    if (toggled)
                        OutlineRenderer.Outline(obj, new Color(1F, 0.5333F, 0F), 7);
                    else
                        OutlineRenderer.UnOutline(obj);
                }

            }
        }

        public void FixOutline()
        {
            // Remove outline
            Element_ToggleChanged(false);

            // Add new outline
            Element_ToggleChanged(true);
        }

        public override void FixedUpdate()
        {
            if (InGame && Element.GetValue<bool>())
            {
                if (owner == null)
                    owner = FindOwner();

                if (owner.GetComponent<Outline>() == null)
                {
                    FixOutline();
                }
            }
        }

        private List<GameObject> GetOutlines(PlayerManager manager)
        {
            List<GameObject> outlines = new();

            if (manager != null)
            {
                var customization = manager.playerCustomization;
                outlines.Add(customization.sweater);
                outlines.Add(customization.pants);
            }

            return outlines;
        }

        private static PlayerManager FindOwner()
        {
            var players = GameManager.Instance.activePlayers;

            foreach (var player in players.Values)
            {
                if (player.steamProfile.m_SteamID == SteamManager.Instance.originalLobbyOwnerId.m_SteamID)
                    return player;
            }

            return null;
        }

    }
}
