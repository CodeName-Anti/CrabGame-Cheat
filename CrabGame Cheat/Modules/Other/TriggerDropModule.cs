using JNNJMods.CrabGameCheat.Util;
using JNNJMods.UI;
using JNNJMods.UI.Elements;

namespace JNNJMods.CrabGameCheat.Modules
{
    [CheatModule]
    public class TriggerDropModule : SingleElementModule<ButtonInfo>
    {
        public TriggerDropModule(ClickGUI gui) : base("Trigger drop", gui, WindowIDs.Other)
        {
        }

        public override ElementInfo CreateElement(int windowId)
        {

            ButtonInfo triggerDrop = new(windowId, Name);

            triggerDrop.ButtonPress += TriggerDrop;

            return triggerDrop;
        }

        private void TriggerDrop()
        {
            MonoBehaviourPublicStCaSt1ObSthaUIStmaUnique.TryTriggerDrop();
        }
    }
}
