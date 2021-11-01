using JNNJMods.UI;
using JNNJMods.UI.Elements;
using System;
using UnityEngine;

namespace JNNJMods.CrabGameCheat.Modules
{
    public class GlassBreakESPModule : SingleElementModule<ToggleInfo>
    {

        public GlassBreakESPModule(ClickGUI gui) : base("GlassBreak ESP", gui, WindowIDs.MAIN)
        {

        }

        public override ElementInfo CreateElement(int windowId)
        {
            return new ToggleInfo(windowId, Name, false, true);
        }

        public override void OnGUI()
        {
            if(InGame && Element.GetValue<bool>())
            {

                foreach (GlassBreak glassBreak in GlassManager.Instance.pieces)
                {
                    //Distance from Player to Glass
                    int distance = (int)Vector3.Distance(PlayerStatus.Instance.transform.position, glassBreak.transform.position);

                    //Create Style
                    GUIStyle style = new GUIStyle(GUI.skin.label)
                    {
                        alignment = TextAnchor.MiddleCenter
                    };
                    style.normal.textColor = Color.yellow;

                    //UI Position of the glass
                    Vector3 screenPoint = Camera.main.WorldToScreenPoint(glassBreak.transform.position);
                    if (screenPoint.z > 0.0)
                        //Render Text above glass
                        GUI.Label(new Rect(screenPoint.x, Screen.currentResolution.height - screenPoint.y, 0.0f, 0.0f), glassBreak.name + " [" + distance + "m]", style);
                }
            }
            
        }
    }
}
