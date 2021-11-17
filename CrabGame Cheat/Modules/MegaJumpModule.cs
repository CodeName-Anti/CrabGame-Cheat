/*
using JNNJMods.CrabGameCheat.Translators;
using JNNJMods.CrabGameCheat.Util;
using JNNJMods.UI;
using JNNJMods.UI.Elements;

namespace JNNJMods.CrabGameCheat.Modules
{
    [CheatModule]
    public class MegaJumpModule : SingleElementModule<ToggleInfo>
    {

        private bool init;

        private float jumpForce;

        public MegaJumpModule(ClickGUI gui) : base("Mega Jump", gui, WindowIDs.MOVEMENT)
        {

        }

        public override ElementInfo CreateElement(int windowId)
        {
            Element = new ToggleInfo(windowId, Name, false, true);

            Element.ToggleChanged += Element_ToggleChanged;

            return Element;
        }

        public void Element_ToggleChanged(bool toggled)
        {
            if (!InGame) Element.SetValue(false);

            if (toggled)
                Instances.PlayerMovement.SetJumpForce(jumpForce * 2f);
            else
                Instances.PlayerMovement.SetJumpForce(jumpForce);
        }

        public override void Update()
        {
            if (InGame)
            {
                if (!init)
                {
                    init = true;

                    jumpForce = Instances.PlayerMovement.GetJumpForce();
                }

                if (Element.GetValue<bool>())
                {
                    Element_ToggleChanged(true);
                }
            }
        }

    }
}
*/