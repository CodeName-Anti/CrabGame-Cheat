using JNNJMods.CrabGameCheat.Util;
using JNNJMods.UI;
using JNNJMods.UI.Elements;
using System;

namespace JNNJMods.CrabGameCheat.Modules.Combat
{
    [CheatModule]
    public class InfinityPunchModule : SingleElementModule<ToggleInfo>
    {
        public InfinityPunchModule(ClickGUI gui) : base("Infinity Punch", gui, WindowIDs.Combat)
        {
        }

        public override ElementInfo CreateElement(int windowId)
        {
            var infPunch = new ToggleInfo(windowId, Name);

            infPunch.ToggleChanged += InfinityPunch_Toggled;

            return infPunch;
        }

        private void InfinityPunch_Toggled(bool toggled)
        {
            CurrentSettings.Instance.UpdateCamShake(!toggled);
            CurrentSettings.Instance.UpdateSave();
        }

        public override void Update()
        {
            if(InGame && Element.GetValue<bool>())
            {
                PunchPlayers[] punchPlayers = UnityEngine.Object.FindObjectsOfType<PunchPlayers>();

                foreach (PunchPlayers punch in punchPlayers)
                {
                    punch.field_Private_Boolean_0 = true;
                    punch.field_Private_Single_0 = 3.1f;
                }
            }
        }

    }
}
