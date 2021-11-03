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
        private readonly List<Component> chamsTargets = new List<Component>();

        [JsonIgnore]
        private ESP esp;

        [JsonIgnore]
        private Chams chams;

        public ESPModule(ClickGUI gui) : base("ESP", gui, WindowIDs.RENDER) { }

        public override void Init(ClickGUI gui, bool json = false)
        {
            base.Init(gui, json);
            esp = new ESP(true, Color.green, false, Color.green, true, Color.green);
            chams = new Chams();
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

                espTargets.Clear();
                chamsTargets.Clear();

                foreach(PlayerManager manager in GameManager.Instance.activePlayers.values)
                {
                    if(manager.onlinePlayerMovement != null)
                    {
                        chamsTargets.Add(manager.onlinePlayerMovement);
                        espTargets.Add(manager.onlinePlayerMovement.gameObject, manager.username);
                    }
                }
                chams.UnChamTargets();
                chams.ChamTargets(chamsTargets.ToArray());
            }

            esp.Draw(espTargets);
            
        }

    }
}
