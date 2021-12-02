using BepInEx.Logging;
using JNNJMods.CrabGameCheat.Loader;
using JNNJMods.CrabGameCheat.Patches;
using JNNJMods.CrabGameCheat.Translators;

namespace JNNJMods.CrabGameCheat.Util
{
    public class CheatLog
    {

        private static ManualLogSource logSource
        {
            get
            {
                return BepInExLoader.Instance.Log;
            }
        }

        /// <summary>
        /// Logs a message to the Console
        /// </summary>
        /// <param name="message"></param>
        public static void Msg(object message)
        {
            logSource.LogMessage(message);
        }

        /// <summary>
        /// Logs an error to the Console.
        /// </summary>
        /// <param name="message"></param>
        public static void Error(object message)
        {
            logSource.LogError(message);
        }

        /// <summary>
        /// Logs a warning to the Console.
        /// </summary>
        /// <param name="message"></param>
        public static void Warning(object message)
        {
            logSource.LogWarning(message);

        }
    }
}
