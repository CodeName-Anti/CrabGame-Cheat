using HarmonyLib;
using JNNJMods.CrabGameCheat.Chat;
using JNNJMods.CrabGameCheat.Translators;
using JNNJMods.CrabGameCheat.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JNNJMods.CrabGameCheat.Patches
{

    [HarmonyPatch(typeof(ChatBox))]
    public static class ChatboxPatch
    {
        public const string CommandPrefix = "/";

        private static Dictionary<ChatCommandAttribute, MethodInfo> commands;

        public static string MakeColored(string text, string color)
        {
            return "<color=" + color + ">" + text + "</color>";
        }

        public static string GetHelpMessage()
        {
            StringBuilder b = new();

            foreach (var attrib in commands.Keys)
            {
                b.Append(MakeColored(CommandPrefix + attrib.Command, "#00ffff"))
                    .Append(" - ")
                    .Append(MakeColored(attrib.Description, "#00ffff"))
                    .Append("\n");
            }

            return b.ToString(0, b.Length - 1);
        }

        public static void SendHelpMessage()
        {
            CheatLog.LogChatBox(GetHelpMessage());
        }

        private static void HideAndClearChat(ChatBox chat)
        {
            chat.ClearMessage();
            chat.prop_Boolean_0 = false;
            chat.CancelInvoke("HideChat");
            chat.Invoke("HideChat", 5f);
        }

        /// <summary>
        /// SendMessage Patch for intercepting chat messages.
        /// </summary>
        /// <param name="__instance"></param>
        /// <param name="param_1"></param>
        /// <returns></returns>
        [HarmonyPatch(nameof(ChatBox.SendMessage))]
        [HarmonyPrefix]
        public static bool SendMessage(ref ChatBox __instance, string param_1)
        {
            try
            {
                string message = param_1;
                //Get all Commands
                if (commands == null)
                    commands = ChatCommandAttribute.GetChatCommands();

                //Execute Commands
                foreach (KeyValuePair<ChatCommandAttribute, MethodInfo> entry in commands)
                {
                    if (message.StartsWith(CommandPrefix + entry.Key.Command, System.StringComparison.OrdinalIgnoreCase))
                    {
                        string[] args = message.Split(' ');

                        string messageParam = message.Replace(args[0] + " ", "");

                        args = args.Where(arg => !arg.Equals(args[0])).ToArray();

                        try
                        {
                            entry.Value.Invoke(null, new object[] { messageParam, args });
                        }
                        catch (Exception e)
                        {
                            CheatLog.Error(e.ToString());
                        }

                        HideAndClearChat(__instance);

                        return false;
                    }
                }

                //Send help message if no Command has been found
                if (message.StartsWith(CommandPrefix))
                {
                    SendHelpMessage();
                    HideAndClearChat(__instance);
                    return false;
                }
            } catch(Exception e)
            {
                CheatLog.Error("Chat Commands Error: " + e.ToString());
            }
            
            return true;
        }
    }
}
