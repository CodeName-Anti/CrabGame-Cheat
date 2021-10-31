using JNNJMods.UI.Elements;
using JNNJMods.UI;
using System;
using Newtonsoft.Json;

namespace JNNJMods.CrabGameCheat.Modules
{
    public class MegaJumpModule : SingleElementModule<ToggleInfo>
    {

        [JsonIgnore]
        private bool init;

        [JsonIgnore]
        private float jumpForce;

        public MegaJumpModule(ClickGUI gui) : base("Mega Jump", gui, WindowIDs.MAIN)
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
                PlayerMovement.Instance.jumpForce = jumpForce * 2f;
            else
                PlayerMovement.Instance.jumpForce = jumpForce;
        }

        public override void Update()
        {
            if(InGame && !init)
            {
                init = true;

                jumpForce = PlayerMovement.Instance.jumpForce;
            }

            if(InGame && Element.GetValue<bool>())
            {
                Element_ToggleChanged(true);
            }
        }

    }
}
