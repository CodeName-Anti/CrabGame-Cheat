using JNNJMods.CrabGameCheat.Util;
using JNNJMods.UI;
using JNNJMods.UI.Elements;
using Steamworks;

namespace JNNJMods.CrabGameCheat.Modules
{
    [CheatModule]
    public class TilePainterModule : SingleElementModule<ButtonInfo>
    {

        public TilePainterModule(ClickGUI gui) : base("Tile Painter", gui, WindowIDs.OTHER)
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
            if (!InGame)
                return;

            if (LobbyManager.Instance.gameMode.type != GameModeData.GameModeType.TileDrive)
                return;

            var tiles = TileManager.Instance.tiles;

            if (tiles == null || tiles.Count <= 0)
                return;


            int teamId = TileManager.Instance.idToTeam[SteamClient.SteamId.Value];

            foreach(Tile t in tiles)
            {
                t.SetTileColor(teamId);
            }
            
        }
    }
}
