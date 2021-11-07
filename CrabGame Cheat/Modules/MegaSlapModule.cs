using JNNJMods.UI.Elements;
using JNNJMods.UI;
using Newtonsoft.Json;
using JNNJMods.CrabGameCheat.Util;

namespace JNNJMods.CrabGameCheat.Modules
{
    [CheatModule]
    public class MegaSlapModule : SingleElementModule<ToggleInfo>
    {

        [JsonIgnore]
        private bool init;

        [JsonIgnore]
        private float maxDistance;

        public MegaSlapModule(ClickGUI gui) : base("Mega Slap", gui, WindowIDs.COMBAT)
        {

        }

        public override ElementInfo CreateElement(int windowId)
        {
            Element = new ToggleInfo(windowId, Name, false, true);

            Element.ToggleChanged += Element_ToggleChanged;

            return Element;
        }

        private void Element_ToggleChanged(bool toggled)
        {
            var punchPlayers = PlayerMovement.Instance.punchPlayers;
            if(toggled)
            {
                punchPlayers.ready = true;
                punchPlayers.maxDistance = float.PositiveInfinity;
                SetCamShake(false);
            } else
            {
                punchPlayers.maxDistance = maxDistance;
                SetCamShake(true);
            }
        }

        private void SetCamShake(bool value)
        {
            CurrentSettings.Instance.UpdateCamShake(value);
        }

        public override void Update()
        {
            if(InGame)
            {
                if (!init)
                {
                    init = true;
                    var punchPlayers = PlayerMovement.Instance.punchPlayers;

                    maxDistance = punchPlayers.maxDistance;
                }

                if (Element.GetValue<bool>())
                {
                    Element_ToggleChanged(true);
                }
            }
        }
    }
}
