using JNNJMods.UI;
using JNNJMods.UI.Elements;
using JNNJMods.UI.Utils;
using MelonLoader;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace JNNJMods.CrabGameCheat.Modules
{
    public class ESPModule : Module<ToggleInfo>
    {
        [JsonIgnore]
        private Dictionary<GameObject, string> targets = new Dictionary<GameObject, string>();

        [JsonIgnore]
        private ESP esp;

        public ESPModule(ClickGUI gui) : base("ESP", gui, WindowIDs.MAIN) { }

        public override void Init(ClickGUI gui, bool json = false)
        {
            base.Init(gui, json);
            esp = new ESP(true, Color.green, false, Color.green, true, Color.green);
        }

        public override ElementInfo CreateElement(int windowId)
        {
            return new ToggleInfo(windowId, "ESP");
        }

        public override void OnGUI()
        {
            if(Element.GetValue<bool>())
            {

                if(targets.Count != GameManager.Instance.activePlayers.Count)
                {
                    targets.Clear();

                    foreach(OnlinePlayerMovement script in Object.FindObjectsOfType<OnlinePlayerMovement>())
                    {
                        targets.Add(script.gameObject, script.playerManager.username);
                    }
                }

                esp.Draw(targets);
            }
        }

    }
}
