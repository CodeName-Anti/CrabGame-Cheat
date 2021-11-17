using JNNJMods.CrabGameCheat.Translators;
using JNNJMods.CrabGameCheat.Util;
using JNNJMods.Render;
using JNNJMods.UI;
using JNNJMods.UI.Elements;
using JNNJMods.UI.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JNNJMods.CrabGameCheat.Modules
{
    [CheatModule]
    public class ESPModule : SingleElementModule<ToggleInfo>
    {

        private static readonly Action<GameObject>
            Outline = delegate (GameObject obj)
            {
                OutlineRenderer.Outline(obj, Color.red, 7);
            },
            UnOutline = delegate (GameObject obj)
            {
                OutlineRenderer.UnOutline(obj);
            };

        private readonly Dictionary<GameObject, string> espTargets = new Dictionary<GameObject, string>();

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
            if (toggled)
            {
                if (!InGame) return;
                if (espTargets.Count != Instances.GameManager.activePlayers.Count - 1)
                {
                    CheatLog.Msg("Getting ESP-Targets!");

                    //Remove Outline
                    foreach (GameObject obj in espTargets.Keys)
                    {
                        if (obj == null) continue;

                        ExecutePlayer(obj.GetComponent<MonoBehaviourPublicCSstReshTrheObplBojuUnique>(), Outline);
                    }

                    espTargets.Clear();

                    foreach (var manager in Instances.GameManager.activePlayers.values)
                    {
                        if (manager.GetOnlinePlayerMovement() != null)
                        {
                            ExecutePlayer(manager, UnOutline);

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

                    ExecutePlayer(obj.GetComponent<MonoBehaviourPublicCSstReshTrheObplBojuUnique>(), UnOutline);

                }

                espTargets.Clear();
            }
        }

        private void ExecutePlayer(MonoBehaviourPublicCSstReshTrheObplBojuUnique manager, Action<GameObject> ac)
        {
            List<GameObject> outlines = new List<GameObject>();

            var customization = manager.playerCustomization;

            outlines.Add(customization.sweater);
            outlines.Add(customization.pants);

            foreach (GameObject obj in outlines)
            {
                ac.Invoke(obj);
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
