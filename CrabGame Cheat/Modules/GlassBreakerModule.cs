using JNNJMods.CrabGameCheat.Util;
using JNNJMods.UI;
using JNNJMods.UI.Elements;
using SteamworksNative;

namespace JNNJMods.CrabGameCheat.Modules
{
    [CheatModule]
    public class GlassBreakerModule : SingleElementModule<ButtonInfo>
    {

        public GlassBreakerModule(ClickGUI gui) : base("GlassBreaker", gui, WindowIDs.OTHER)
        {

        }

        public override ElementInfo CreateElement(int windowId)
        {
            Element = new ButtonInfo(windowId, Name, true);

            Element.ButtonPress += Element_ButtonPress;

            return Element;
        }

        private void Element_ButtonPress()
        {
            foreach (GlassBreak glass in GlassManager.Instance.pieces)
            {
                if (glass == null) continue;
                glass.AllInteract(SteamUser.GetSteamID().m_SteamID);
            }
        }
    }
}
