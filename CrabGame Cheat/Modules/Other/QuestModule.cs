using JNNJMods.CrabGameCheat.Util;
using JNNJMods.UI;
using JNNJMods.UI.Elements;

namespace JNNJMods.CrabGameCheat.Modules
{
    [CheatModule]
    public class QuestModule : MultiElementModuleBase
    {
        private static PlayerSave Save => SaveManager.Instance.state;

        public QuestModule(ClickGUI gui) : base("Quest", gui, WindowIDs.Other)
        {
        }

        public override void Init(ClickGUI gui, bool json = false)
        {
            base.Init(gui, json);

            ButtonInfo
                completeDaily = new(ID, "Complete Daily Quest"),
                resetDailyCooldown = new(ID, "Reset DailyCooldown");

            completeDaily.ButtonPress += CompleteDaily;
            resetDailyCooldown.ButtonPress += ResetDailyCooldown;

            Elements.Add(completeDaily);
            Elements.Add(resetDailyCooldown);

            foreach (ElementInfo info in Elements) 
                gui.AddElement(info);
            
        }

        private void ResetDailyCooldown()
        {
            Save.nextQuestAvailableTime = Il2CppSystem.DateTime.Now;
            SaveManager.Instance.Save();
        }

        private void CompleteDaily()
        {
            QuestManager.Instance.CompleteQuest();

            SaveManager.Instance.state.AddQuestProgress(187);

            SaveManager.Instance.Save();
        }
    }
}
