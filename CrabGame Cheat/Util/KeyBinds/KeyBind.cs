using UnityEngine;

namespace JNNJMods.CrabGameCheat.Util.KeyBinds
{
    public class KeyBind
    {
        
        public string Key { get; private set; }

        public KeyCode[] Keys { get; set; }

        public KeyBind(string key)
        {
            Key = key;
            Keys = new KeyCode[0];
        }

    }
}
