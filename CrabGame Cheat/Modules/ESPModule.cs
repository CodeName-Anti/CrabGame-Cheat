using JNNJMods.CrabGameCheat.Util;
using JNNJMods.Render;
using JNNJMods.UI;
using JNNJMods.UI.Elements;
using JNNJMods.UI.Utils;
using Newtonsoft.Json;
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
            return new ToggleInfo(windowId, "ESP", false, true);
        }

        public override void OnGUI()
        {
            if (!InGame) return;

            if (!Element.GetValue<bool>())
                return;

            if(espTargets.Count != GameManager.Instance.activePlayers.Count - 1)
            {
                CheatLog.Msg("Getting ESP-Targets!");

                foreach(GameObject obj in espTargets.Keys)
                {
                    OutlineRenderer.UnOutlinePlayer(obj.GetComponent<PlayerManager>());
                }

                espTargets.Clear();

                foreach(PlayerManager manager in GameManager.Instance.activePlayers.values)
                {
                    if(manager.onlinePlayerMovement != null)
                    {
                        var outline = manager.gameObject.AddComponent<Outline>();

                        OutlineRenderer.OutlinePlayer(manager, Color.red, 7);

                        espTargets.Add(manager.onlinePlayerMovement.gameObject, manager.username);
                    }
                }
            }

            esp.Draw(espTargets);
            
        }

    }
}
