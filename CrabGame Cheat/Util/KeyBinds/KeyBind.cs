using UnityEngine;

namespace JNNJMods.CrabGameCheat.Util.KeyBinds
{
    public class KeyBind
    {
        public string Key { get; set; }

        public KeyCode[] Keys { get; set; }

        public object[] ModuleData { get; set; }

        public KeyBind(string key) : this()
        {
            Key = key;
        }

        public KeyBind()
        {
            Keys = new KeyCode[0];
            ModuleData = new object[0];
        }

    }
}
