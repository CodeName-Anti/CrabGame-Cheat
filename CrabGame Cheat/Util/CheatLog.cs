using BepInEx.Logging;
using JNNJMods.CrabGameCheat.Loader;
namespace JNNJMods.CrabGameCheat.Util
{
    public class CheatLog
    {

        private static ManualLogSource LogSource
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
            LogSource.LogMessage(message);
        }

        /// <summary>
        /// Logs an error to the Console.
        /// </summary>
        /// <param name="message"></param>
        public static void Error(object message)
        {
            LogSource.LogError(message);
        }

        /// <summary>
        /// Logs a warning to the Console.
        /// </summary>
        /// <param name="message"></param>
        public static void Warning(object message)
        {
            LogSource.LogWarning(message);
        }

        /// <summary>
        /// Logs an info to the Console.
        /// </summary>
        /// <param name="message"></param>
        public static void Info(object message)
        {
            LogSource.LogInfo(message);
        }

    }
}
