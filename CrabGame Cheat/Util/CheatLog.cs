using BepInEx.Logging;
using JNNJMods.CrabGameCheat.Loader;
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

        /// <summary>
        /// Logs a message to the Chatbox. <br></br>
        /// <b>Should only be called InGame!</b>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="displayUsername"></param>
        public static void LogChatBox(object message, bool displayUsername = true)
        {
            var chat = ChatBox.Instance;

            chat.messages.text += "\n" + (displayUsername ? "<color=#5100ff>CrabGame Cheat</color>:" : "") + $"{message}";
            //Show chat
            chat.Method_Private_Void_1();

            chat.CancelInvoke("HideChat");
            chat.Invoke("HideChat", 5f);
        }

    }
}
