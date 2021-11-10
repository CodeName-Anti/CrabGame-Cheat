using JNNJMods.UI.Elements;
using JNNJMods.UI;
using Newtonsoft.Json;
using JNNJMods.CrabGameCheat.Translators;
using JNNJMods.CrabGameCheat.Util;

namespace JNNJMods.CrabGameCheat.Modules
{
    [CheatModule]
    public class SpeedModule : MultiElementModuleBase
    {
        private bool init = false;

        private float
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

            InitSpeeds();
        }

        void SpeedToggle_ToggleChanged(bool toggled)
        {
            InitSpeeds();

            float SpeedAmount = Elements[1].GetValue<float>();

            PlayerMovement move = Instances.PlayerMovement;
            if (toggled)
            {
                move.SetMoveSpeed(moveSpeed * SpeedAmount);
                move.SetMaxRunSpeed(maxRunSpeed * SpeedAmount);
                move.SetMaxSpeed(maxSpeed * SpeedAmount);
                move.SetMaxSlopeAngle(maxSlopeAngle * SpeedAmount);
                move.SetSlowDownSpeed(slowDownSpeed * SpeedAmount);
            } else
            {
                move.SetMoveSpeed(moveSpeed);
                move.SetMaxRunSpeed(maxRunSpeed);
                move.SetMaxSpeed(maxSpeed);
                move.SetMaxSlopeAngle(maxSlopeAngle);
                move.SetSlowDownSpeed(slowDownSpeed);
            }
        }

        private void InitSpeeds()
        {
            if (!init && InGame)
            {
                init = true;

                var move = Instances.PlayerMovement;

                moveSpeed = move.GetMoveSpeed();
                maxRunSpeed = move.GetMaxRunSpeed();
                maxSpeed = move.GetMaxSpeed();
                maxSlopeAngle = move.GetMaxSlopeAngle();
                slowDownSpeed = move.GetSlowDownSpeed();

                CheatLog.Msg("move: " + moveSpeed);
                CheatLog.Msg("maxRun: " + maxRunSpeed);
                CheatLog.Msg("maxSpeed: " + maxSpeed);
                CheatLog.Msg("maxSlopeAngle: " + maxSlopeAngle);
                CheatLog.Msg("slowDownSpeed: " + slowDownSpeed);
            }
        }

        public override void Update()
        {
            if (!InGame)
                return;

            InitSpeeds();

            var toggled = Elements[0].GetValue<bool>();
            
            if(toggled)
            {
                SpeedToggle_ToggleChanged(toggled);
            }
        }

    }
}
