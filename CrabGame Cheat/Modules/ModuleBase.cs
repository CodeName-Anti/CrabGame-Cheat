using JNNJMods.UI;
using JNNJMods.UI.Elements;
using Newtonsoft.Json;
using UnityEngine;

namespace JNNJMods.CrabGameCheat.Modules
{
    public abstract class ModuleBase
    {
        [JsonIgnore]
        protected ClickGUI Gui { get; private set; }

        [JsonIgnore]
        public ElementInfo Element { get; protected set; }

        public WindowIDs WindowId { get; protected set; }

        [JsonIgnore]
        protected bool InGame
        {
            get
            {
                return PlayerMovement.Instance != null;
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

        public KeyCode KeyBind
        {
            get
            {
                return Element.KeyBind;
            }
            set
            {
                if (Element.KeyBindable)
                    Element.KeyBind = value;
            }
        }

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

            gui.AddElement(Element = CreateElement(ID));
        }

        public ModuleBase(string name, ClickGUI gui, WindowIDs windowId, bool empty = false)
        {
            Name = name;
            WindowId = windowId;
            
            if(!empty) Init(gui);
        }

        public virtual void Update() { }

        public virtual void OnGUI() { }

        /// <summary>
        /// Called after Module initializes.
        /// </summary>
        /// <returns></returns>
        public abstract ElementInfo CreateElement(int windowId);
    }
}
