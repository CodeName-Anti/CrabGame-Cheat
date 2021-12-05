using JNNJMods.CrabGameCheat.Translators;
using JNNJMods.CrabGameCheat.Util;
using JNNJMods.CrabGameCheat.Util.KeyBinds;
using JNNJMods.UI;
using JNNJMods.UI.Elements;
using System;
using UnityEngine;

namespace JNNJMods.CrabGameCheat.Modules
{
    [CheatModule]
    public class SpeedModule : MultiElementModuleBase
    {
        public float SpeedAmount
        {
            get
            {
                return Elements[1].GetValue<float>();
            }
            set
            {
                Elements[1].SetValue(value);
            }
        }

        public SpeedModule(ClickGUI gui) : base("Speed", gui, WindowIDs.Movement)
        {

        }

        public override void Init(ClickGUI gui, bool json = false)
        {
            base.Init(gui, json);

            ToggleInfo speedToggle = new(ID, Name, false, true);
            speedToggle.ToggleChanged += SpeedToggle_ToggleChanged;
            Elements.Add(speedToggle);

            SliderInfo speedSlider = new(ID, 1, 40);

            Elements.Add(speedSlider);

            foreach (ElementInfo info in Elements)
            {
                gui.AddElement(info);
            }
        }

        public override void SetKeyBinds(KeyBind keybind)
        {
            base.SetKeyBinds(keybind);

            try
            {

                CheatLog.Error(keybind.ModuleData[0].ToString());

                SpeedAmount = float.Parse(keybind.ModuleData[0].ToString());

                if(SpeedAmount < 0)
                    SpeedAmount = 1;

            } catch(Exception e)
            {
                CheatLog.Error(e.ToString());
                //Data invalid
            }
        }

        public override KeyBind GetKeyBinds()
        {
            var bind = base.GetKeyBinds();

            bind.ModuleData = new object[] { SpeedAmount };

            return bind;
        }

        void SpeedToggle_ToggleChanged(bool toggled)
        {
            if(InGame)
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
            
        }

        public override void Update()
        {
            if (!InGame)
                return;

            SpeedToggle_ToggleChanged(Elements[0].GetValue<bool>());
        }

    }
}