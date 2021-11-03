#if MELONLOADER
using MelonLoader;
#elif BEPINEX
using JNNJMods.CrabGameCheat.Loader;
using BepInEx.Logging;
#endif

namespace JNNJMods.CrabGameCheat.Util
{
    public class CheatLog
    {

#if BEPINEX
        private static ManualLogSource logSource
        {
            get
            {
                return BepInExLoader.Instance.Log;
            }
        }
#endif

        public static void Msg(object message)
        {
#if MELONLOADER
            MelonLogger.Msg(message);
#elif BEPINEX
            logSource.LogMessage(message);
#endif
        }

        public static void Error(object message)
        {
#if MELONLOADER
            MelonLogger.Error(message);
#elif BEPINEX
            logSource.LogError(message);
#endif
        }

        public static void Warning(object message)
        {
#if MELONLOADER
            MelonLogger.Warning(message);
#elif BEPINEX
            logSource.LogWarning(message);
#endif

        }

    }
}
