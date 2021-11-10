using JNNJMods.CrabGameCheat.Translators;
using JNNJMods.UI;
using Newtonsoft.Json;

namespace JNNJMods.CrabGameCheat.Modules
{
    public abstract class ModuleBase
    {
        [JsonIgnore]
        protected ClickGUI Gui { get; private set; }

        public WindowIDs WindowId { get; protected set; }

        [JsonIgnore]
        protected bool InGame
        {
            get
            {
                return Instances.PlayerMovement != null;
            }
        }

        /// <summary>
        /// Value of <see cref="WindowId"/> presented as an <see cref="int"/>.
        /// </summary>
        [JsonIgnore]
        protected int ID
        {
            get
            {
                return (int)WindowId;
            }
        }

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

        public virtual void Update() { }

        public virtual void OnGUI() { }
    }
}
