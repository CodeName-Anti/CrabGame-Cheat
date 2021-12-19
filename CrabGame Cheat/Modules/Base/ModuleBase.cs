using HarmonyLib;
using JNNJMods.CrabGameCheat.Loader;
using JNNJMods.CrabGameCheat.Translators;
using JNNJMods.CrabGameCheat.Util.KeyBinds;
using JNNJMods.UI;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace JNNJMods.CrabGameCheat.Modules
{
    public class ModuleBase
    {
        [JsonIgnore]
        protected Harmony HarmonyInstance => BepInExLoader.Instance.HarmonyInstance;

        [JsonIgnore]
        protected ClickGUI Gui { get; private set; }

        public WindowIDs WindowId { get; protected set; }

        [JsonIgnore]
        protected bool InGame => Instances.PlayerMovement != null;

        /// <summary>
        /// Value of <see cref="WindowId"/> presented as an <see cref="int"/>.
        /// </summary>
        [JsonIgnore]
        protected int ID => (int)WindowId;

        public string Name { get; protected set; }

        /// <summary>
        /// Called from Constructor or after Json Deserialization.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="gui"></param>
        /// <param name="windowId"></param>
        /// <param name="json"></param>
        public virtual void Init(ClickGUI gui, bool json = false)
        {
            Gui = gui;
        }

        public ModuleBase(string name, ClickGUI gui, WindowIDs windowId)
        {
            Name = name;
            WindowId = windowId;

            Init(gui);
        }

        public ModuleBase(string name, ClickGUI gui)
        {
            Name = name;

            Init(gui);
        }

        public virtual void SetKeyBinds(KeyBind keybind) { }

        public virtual KeyBind GetKeyBinds() { return new KeyBind(); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Update() { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void FixedUpdate() { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void OnGUI() { }

    }
}
