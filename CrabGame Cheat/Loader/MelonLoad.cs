#if MELONLOADER
using MelonLoader;

namespace JNNJMods.CrabGameCheat.Loader
{
    public class MelonLoad : MelonMod, ICheatLoader
    {
        public Cheat Cheat
        { 
            get
            {
                return cheat ?? (cheat = new Cheat());
            }
        }

        private Cheat cheat;

        public override void OnApplicationQuit()
        {
            Cheat.OnApplicationQuit();
        }

        public override void OnApplicationStart()
        {
            Cheat.OnApplicationStart(HarmonyInstance);
        }

        public override void OnApplicationLateStart()
        {
            Cheat.OnApplicationLateStart();
        }

        public override void OnUpdate()
        {
            Cheat.OnUpdate();
        }

        public override void OnGUI()
        {
            Cheat.OnGUI();
        }
    }
}
#endif