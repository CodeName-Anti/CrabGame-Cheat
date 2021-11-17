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

        public static void Msg(object message)
        {
            logSource.LogMessage(message);
        }

        public static void Error(object message)
        {
            logSource.LogError(message);
        }

        public static void Warning(object message)
        {
            logSource.LogWarning(message);

        }

        public static void LogChatBox(object message, bool displayUsername = true)
        {
            var chat = Instances.ChatBox;

            chat.messages.text += "\n" + (displayUsername ? "<color=#5100ff>CrabGame Cheat</color>:" : "") + $"{message}";
            //Show chat
            chat.Method_Private_Void_1();

            chat.CancelInvoke("HideChat");
            chat.Invoke("HideChat", 5f);
        }

    }
}
