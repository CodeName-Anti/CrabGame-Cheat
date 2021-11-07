using Il2CppSystem.Collections.Generic;
using JNNJMods.UI;
using JNNJMods.UI.Elements;
using Newtonsoft.Json;
using JNNJMods.CrabGameCheat.Util;

namespace JNNJMods.CrabGameCheat.Modules
{
    [CheatModule]
    public class ItemSpawnerModule : MultiElementModuleBase
    {
        [JsonIgnore]
        private bool init;

        public ItemSpawnerModule(ClickGUI gui) : base("Spawner", gui, WindowIDs.ITEM_SPAWNER)
        {

        }

        public static void SpawnItem(ItemData data)
        {
            PlayerInventory.Instance.ForceGiveItem(ItemManager.GetItemById(data.itemID));
        }

        public override void Update()
        {

            if (InGame && !init && ItemManager.Instance != null)
            {
                init = true;

                foreach (KeyValuePair<int, ItemData> entry in ItemManager.idToItem)
                {
                    ButtonInfo info = new ButtonInfo(ID, "Spawn " + entry.value.name, true);
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