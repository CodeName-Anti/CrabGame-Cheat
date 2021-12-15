using JNNJMods.AimCheats;
using JNNJMods.CrabGameCheat.Util;
using JNNJMods.UI;
using JNNJMods.UI.Elements;
using Newtonsoft.Json;
using SteamworksNative;
using System.Collections.Generic;
using UnityEngine;

namespace JNNJMods.CrabGameCheat.Modules
{
    [CheatModule]
    public class AimModule : MultiElementModuleBase
    {
        [JsonIgnore]
        private Aimbot aim;

        public AimModule(ClickGUI gui) : base("Aim", gui, WindowIDs.Combat)
        {
        }

        public override void Init(ClickGUI gui, bool json = false)
        {
            base.Init(gui, json);

            aim = new Aimbot()
            {
                mask = LayerMask.GetMask("Default", "Player", "Ground")
            };

            ToggleInfo aimBot = new(ID, "AimBot", false, true);
            aimBot.ToggleChanged += Aimbot_ToggleChanged;

            ToggleInfo triggerBot = new(ID, "TriggerBot", false, true);
            triggerBot.ToggleChanged += TriggerBot_ToggleChanged;

            Elements.Add(aimBot);
            Elements.Add(triggerBot);

            foreach (ElementInfo info in Elements)
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

            foreach (PlayerManager manager in GameManager.Instance.activePlayers.Values)
            {
                if (manager.dead || manager.steamProfile.m_SteamID == SteamUser.GetSteamID().m_SteamID)
                    continue;
                heads.Add(manager.head.gameObject);
            }

            return heads.ToArray();
        }

        private bool AimbotValid()
        {
            return
                // Should be InGame and alive
                InGame &&
                // GUI should be hidden
                !Gui.Shown &&
                // PlayerInput should be active
                PlayerInput.Instance.active &&
                // Game shouldn't be pause
                !PauseUI.paused &&
                // Cursor shouldn't be visible
                !Cursor.visible &&
                // Application should be focused
                Application.isFocused;
        }

        public override void Update()
        {
            if (AimbotValid())
            {
                if (Elements[0].GetValue<bool>())
                    aim.Aim(GetHeads());

                if (Elements[1].GetValue<bool>())
                    aim.Trigger();
            }

        }

    }
}
