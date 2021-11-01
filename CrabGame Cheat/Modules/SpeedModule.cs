using JNNJMods.UI.Elements;
using JNNJMods.UI;
using Newtonsoft.Json;

namespace JNNJMods.CrabGameCheat.Modules
{
    public class SpeedModule : MultiElementModuleBase
    {

        public float SpeedAmount { get; private set; }

        [JsonIgnore]
        private bool init;

        [JsonIgnore]
        private float
            maxWalkSpeed,
            moveSpeed,
            maxRunSpeed,
            maxSpeed,
            maxSlopeAngle,
            slowDownSpeed;

        public SpeedModule(ClickGUI gui) : base("Speed", gui, WindowIDs.MAIN)
        {

        }

        public override void Init(ClickGUI gui, bool json = false)
        {
            base.Init(gui, json);

            ToggleInfo speedToggle = new ToggleInfo(ID, Name, false, true);
            speedToggle.ToggleChanged += SpeedToggle_ToggleChanged;
            Elements.Add(speedToggle);

            SliderInfo speedSlider = new SliderInfo(ID, 1, 20);
            speedSlider.ValueChanged += SpeedSlider_ValueChanged;
            Elements.Add(speedSlider);


            foreach (ElementInfo info in Elements)
            {
                gui.AddElement(info);
            }
        }

        void SpeedSlider_ValueChanged(object oldValue, object newValue)
        {

            SpeedAmount = (float)newValue;

        }

        void SpeedToggle_ToggleChanged(bool toggled)
        {
            var move = PlayerMovement.Instance;
            if(toggled)
            {
                move.maxWalkSpeed = maxWalkSpeed * SpeedAmount;
                move.moveSpeed = moveSpeed * SpeedAmount;
                move.maxRunSpeed = maxRunSpeed * SpeedAmount;
                move.maxSpeed = maxSpeed * SpeedAmount;
                move.maxSlopeAngle = maxSlopeAngle * SpeedAmount;
                move.slowDownSpeed = slowDownSpeed * SpeedAmount;
            } else
            {
                move.maxWalkSpeed = maxWalkSpeed;
                move.moveSpeed = moveSpeed;
                move.maxRunSpeed = maxRunSpeed;
                move.maxSpeed = maxSpeed;
                move.maxSlopeAngle = maxSlopeAngle;
                move.slowDownSpeed = slowDownSpeed;
            }
        }

        public override void Update()
        {
            if(InGame && !init)
            {
                init = true;

                var move = PlayerMovement.Instance;

                maxWalkSpeed = move.maxWalkSpeed;
                moveSpeed = move.moveSpeed;
                maxRunSpeed = move.maxRunSpeed;
                maxSpeed = move.maxSpeed;
                maxSlopeAngle = move.maxSlopeAngle;
                slowDownSpeed = move.slowDownSpeed;
            }


            if(InGame)
            {
                SpeedToggle_ToggleChanged(Elements[0].GetValue<bool>());
            }
        }

    }
}
