using JNNJMods.CrabGameCheat.Translators;
using JNNJMods.CrabGameCheat.Util;
using JNNJMods.UI;
using JNNJMods.UI.Elements;

namespace JNNJMods.CrabGameCheat.Modules
{
    [CheatModule]
    public class SpeedModule : MultiElementModuleBase
    {
        public float SpeedAmount { get; private set; }

        public SpeedModule(ClickGUI gui) : base("Speed", gui, WindowIDs.MOVEMENT)
        {

        }

        public override void Init(ClickGUI gui, bool json = false)
        {
            base.Init(gui, json);

            ToggleInfo speedToggle = new(ID, Name, false, true);
            speedToggle.ToggleChanged += SpeedToggle_ToggleChanged;
            Elements.Add(speedToggle);

            SliderInfo speedSlider = new(ID, 1, 40);
            speedSlider.ValueChanged += SpeedSlider_ValueChanged;

            Elements.Add(speedSlider);

            foreach (ElementInfo info in Elements)
            {
                gui.AddElement(info);
            }
        }

        private void SpeedSlider_ValueChanged(object oldValue, object newValue)
        {
            SpeedAmount = (float)newValue;
        }

        void SpeedToggle_ToggleChanged(bool toggled)
        {
            var move = Instances.PlayerMovement;
            if (toggled)
            {
                move.SetMaxRunSpeed(13 * SpeedAmount);
                move.SetMaxSpeed(6.5f * SpeedAmount);
            } else
            {
                move.SetMaxRunSpeed(13);
                move.SetMaxSpeed(6.5f);
            }
            
        }

        public override void Update()
        {
            if (!InGame)
                return;

            SpeedToggle_ToggleChanged(Elements[0].GetValue<bool>());
        }

    }
}