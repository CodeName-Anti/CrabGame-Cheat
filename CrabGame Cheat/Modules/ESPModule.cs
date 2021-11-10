using JNNJMods.CrabGameCheat.Translators;
using JNNJMods.CrabGameCheat.Util;
using JNNJMods.Render;
using JNNJMods.UI;
using JNNJMods.UI.Elements;
using JNNJMods.UI.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JNNJMods.CrabGameCheat.Modules
{
    [CheatModule]
    public class ESPModule : SingleElementModule<ToggleInfo>
    {
        [JsonIgnore]
        private readonly Dictionary<GameObject, string> espTargets = new Dictionary<GameObject, string>();

        [JsonIgnore]
        private ESP esp;

        public ESPModule(ClickGUI gui) : base("ESP", gui, WindowIDs.RENDER) { }

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
            SetEsp(toggled);
            
        }

        private void SetEsp(bool toggled)
        {
            if(toggled)
            {
                if (!InGame) return;
                if (espTargets.Count != GameManager.Instance.activePlayers.Count - 1)
                {
                    CheatLog.Msg("Getting ESP-Targets!");

                    //Remove Outline
                    foreach (GameObject obj in espTargets.Keys)
                    {
                        if (obj == null) continue;
                        OutlineRenderer.UnOutlinePlayer(obj.GetComponent<PlayerManager>());
                    }

                    espTargets.Clear();

                    foreach (PlayerManager manager in GameManager.Instance.activePlayers.values)
                    {
                        if (manager.GetOnlinePlayerMovement() != null)
                        {
                            OutlineRenderer.OutlinePlayer(manager, Color.red, 7);

                            espTargets.Add(manager.GetOnlinePlayerMovement().gameObject, manager.username);
                        }
                    }
                }
            } 
            else
            {
                foreach (GameObject obj in espTargets.Keys)
                {
                    if (obj == null) continue;
                    OutlineRenderer.UnOutlinePlayer(obj.GetComponent<PlayerManager>());
                }

                espTargets.Clear();
            }
        }

        public override void OnGUI()
        {
            if (!InGame) return;

            if (!Element.GetValue<bool>())
                return;

            SetEsp(Element.GetValue<bool>());

            esp.Draw(espTargets);
            
        }

    }
}
