using JNNJMods.CrabGameCheat.Util;
using JNNJMods.UI;
using JNNJMods.UI.Elements;

namespace JNNJMods.CrabGameCheat.Modules
{
    [CheatModule]
    public class ForceStartModule : SingleElementModule<ButtonInfo>
    {

        public ForceStartModule(ClickGUI gui) : base("ForceStart", gui, WindowIDs.LobbyOwner)
        {
        }

        public override ElementInfo CreateElement(int windowId)
        {
            ButtonInfo forceStart = new(windowId, Name, true);

            forceStart.ButtonPress += ForceStartGame;

            return forceStart;
        }

        private void ForceStartGame()
        {
            GameLoop.Instance.StartGames();
        }
    }
}
