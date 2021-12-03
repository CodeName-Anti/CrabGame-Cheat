using JNNJMods.AimCheats;
using JNNJMods.CrabGameCheat.Util;
using JNNJMods.UI;
using JNNJMods.UI.Elements;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace JNNJMods.CrabGameCheat.Modules
{
    [CheatModule]
    public class AimModule : MultiElementModuleBase
    {
        [JsonIgnore]
        private Aimbot aim;

        public AimModule(ClickGUI gui) : base("Aim", gui, WindowIDs.COMBAT)
        {
        }

        public override void Init(ClickGUI gui, bool json = false)
        {
            base.Init(gui, json);

            aim = new Aimbot()
            {
                mask = LayerMask.GetMask("Default", "Player", "Ground")
            };

            ToggleInfo aimBot = new(ID, "AimBot");
            aimBot.ToggleChanged += Aimbot_ToggleChanged;

            ToggleInfo triggerBot = new(ID, "TriggerBot");
            triggerBot.ToggleChanged += TriggerBot_ToggleChanged;

            Elements.Add(aimBot);
            Elements.Add(triggerBot);

            foreach(ElementInfo info in Elements)
            {
                gui.AddElement(info);
            }
        }

        #region Toggles
        private void TriggerBot_ToggleChanged(bool toggled)
        {
            aim.TriggerBot = toggled;
        }

        private void Aimbot_ToggleChanged(bool toggled)
        {
            aim.Enabled = toggled;
        }
        #endregion

        private GameObject[] GetHeads()
        {
            List<GameObject> heads = new();

            foreach(PlayerManager manager in GameManager.Instance.activePlayers.Values)
            {
                heads.Add(manager.head.gameObject);
            }

            return heads.ToArray();
        }

        public override void Update()
        {
            if(InGame &&
                !Gui.Shown &&
                PlayerInput.Instance.active &&
                !PauseUI.paused &&
                !Cursor.visible &&
                Application.isFocused &&
                Elements[0].GetValue<bool>()
                )

                aim.Aim(GetHeads());
        }

    }
}
