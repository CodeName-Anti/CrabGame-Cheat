#if BEPINEX
using JNNJMods.CrabGameCheat.Loader;
#endif

namespace JNNJMods.CrabGameCheat.Util
{
    public class CheatLog
    {

        public static void Msg(string message)
        {
#if MELONLOADER
            MelonLoader.MelonLogger.Msg(message);
#elif BEPINEX
            BepInExLoader.Instance.Log.LogMessage(message);
#endif
        }

    }
}
