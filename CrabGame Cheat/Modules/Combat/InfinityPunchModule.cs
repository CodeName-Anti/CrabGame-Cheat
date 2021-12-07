using JNNJMods.CrabGameCheat.Translators;
using JNNJMods.CrabGameCheat.Util;
using JNNJMods.UI;
using JNNJMods.UI.Elements;

namespace JNNJMods.CrabGameCheat.Modules
{
    [CheatModule]
    public class InfinityPunchModule : SingleElementModule<ToggleInfo>
    {
        public InfinityPunchModule(ClickGUI gui) : base("Infinity Punch", gui, WindowIDs.Combat)
        {
        }

        public override ElementInfo CreateElement(int windowId)
        {
            var infPunch = new ToggleInfo(windowId, Name, false, true);

            infPunch.ToggleChanged += InfinityPunch_Toggled;

            return infPunch;
        }

        private void InfinityPunch_Toggled(bool toggled)
        {
            CurrentSettings.Instance.UpdateCamShake(!toggled);
        }

        public override void FixedUpdate()
        {
            if(InGame && Element.GetValue<bool>())
            {
                var punch = Instances.PlayerMovement.punchPlayers;
                
                punch.field_Private_Boolean_0 = true;
                punch.field_Private_Single_0 = 3.1f;
                
            }
        }

    }
}
