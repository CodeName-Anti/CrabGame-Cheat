using Il2CppSystem.Collections.Generic;
using JNNJMods.UI;
using JNNJMods.UI.Elements;
using Newtonsoft.Json;
using Steamworks;

namespace JNNJMods.CrabGameCheat.Modules
{
    public class SpawnerModule : MultiElementModuleBase
    {
        [JsonIgnore]
        private bool init;

        public SpawnerModule(ClickGUI gui) : base("Spawner", gui, WindowIDs.ITEM_SPAWNER)
        {

        }

        public override void Init(ClickGUI gui, bool json = false)
        {
            base.Init(gui, json);

            gui.AddWindow(ID, "Item Spawner <color=red>BROKEN</color>", 550, 100, 400, 500);
        }

        public static void SpawnItem(ItemData data)
        {
            ServerSend.ForceGiveItem(SteamClient.SteamId.Value, data.itemID, data.objectID);
        }

        public override void Update()
        {

            if (InGame && !init && ItemManager.Instance != null)
            {
                init = true;

                foreach (KeyValuePair<int, ItemData> entry in ItemManager.idToItem)
                {
                    ButtonInfo info = new ButtonInfo(ID, "Spawn " + entry.value.name);
                    info.ButtonPress += () =>
                    {
                        if (!InGame)
                            return;
                        SpawnItem(entry.value);
                    };

                    Elements.Add(info);
                }

                foreach (ElementInfo eInfo in Elements)
                {
                    Gui.AddElement(eInfo);
                }
            }
        }

    }
}
