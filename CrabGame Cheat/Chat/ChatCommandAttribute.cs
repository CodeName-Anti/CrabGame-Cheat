using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace JNNJMods.CrabGameCheat.Chat
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ChatCommandAttribute : Attribute
    {

        public string
            Command,
            Name,
            Description;

        public ChatCommandAttribute(string command, string description = "No description provided!") : this(command, command, description) { }

        public ChatCommandAttribute(string command, string name, string description = "No description provided!")
        {
            Command = command;
            Name = name;
            Description = description;
        }

        public static Dictionary<ChatCommandAttribute, MethodInfo> GetChatCommands()
        {
            return Assembly.GetExecutingAssembly()
            .GetTypes()
            .SelectMany(x => x.GetMethods())
            .Where(y =>
            y.GetCustomAttributes().OfType<ChatCommandAttribute>().Any() &&
            y.IsStatic &&
            y.IsPublic).
            ToDictionary(y => y.GetCustomAttributes().OfType<ChatCommandAttribute>().First());
        }

    }
}
