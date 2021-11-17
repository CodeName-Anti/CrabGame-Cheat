/*using JNNJMods.CrabGameCheat.Translators;
using JNNJMods.CrabGameCheat.Util;
using JNNJMods.UI;
using JNNJMods.UI.Elements;

namespace JNNJMods.CrabGameCheat.Modules
{
    [CheatModule]
    public class SpeedModule : MultiElementModuleBase
    {
        private readonly float
            moveSpeed,
            maxRunSpeed,
            maxSpeed,
            maxSlopeAngle,
            slowDownSpeed;

        public SpeedModule(ClickGUI gui) : base("Speed", gui, WindowIDs.MOVEMENT)
        {

        }

        public override void Init(ClickGUI gui, bool json = false)
        {
            base.Init(gui, json);

            ToggleInfo speedToggle = new ToggleInfo(ID, Name, false, true);
            speedToggle.ToggleChanged += SpeedToggle_ToggleChanged;
            Elements.Add(speedToggle);

            Elements.Add(new SliderInfo(ID, 1, 20));

            foreach (ElementInfo info in Elements)
            {
                gui.AddElement(info);
            }
        }

        void SpeedToggle_ToggleChanged(bool toggled)
        {
            float SpeedAmount = Elements[1].GetValue<float>();

            var move = Instances.PlayerMovement;
            if (toggled)
            {
                move.SetMoveSpeed(moveSpeed * SpeedAmount);
                move.SetMaxRunSpeed(maxRunSpeed * SpeedAmount);
                move.SetMaxSpeed(maxSpeed * SpeedAmount);
                move.SetMaxSlopeAngle(maxSlopeAngle * SpeedAmount);
                move.SetSlowDownSpeed(slowDownSpeed * SpeedAmount);
            }
            else
            {
                move.SetMoveSpeed(moveSpeed);
                move.SetMaxRunSpeed(maxRunSpeed);
                move.SetMaxSpeed(maxSpeed);
                move.SetMaxSlopeAngle(maxSlopeAngle);
                move.SetSlowDownSpeed(slowDownSpeed);
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
*/