using JNNJMods.Render;
using JNNJMods.UI.Elements;
using System.Collections.Generic;
using UnityEngine;

namespace JNNJMods.UI
{
    /// <summary>
    /// A Class for creating a ClickGUI.
    /// </summary>
    public class ClickGUI
    {
        #region Variables
        /// <summary>
        /// Current Instance(only 1 Instance is allowed)
        /// </summary>
        public static ClickGUI Instance { get; private set; }

        private bool shown;

        /// <summary>
        /// Defines if Windows are shown.
        /// </summary>
        public bool Shown
        {
            get
            {
                return shown;
            }

            set
            {
                if (value)
                    Show(true);
                else
                    Hide(true);
            }
        }

        /// <summary>
        /// Defines if a Black transparent Background is drawn when shown
        /// </summary>
        public bool BlackOut;

        /// <summary>
        /// <see cref="KeyBindSelection"/> Handler for Key Binding
        /// </summary>
        public readonly KeyBindSelection keyBindSelection;

        private readonly float margin,
            controlHeight,
            controlDist;

        private readonly SortedDictionary<int, ElementManager> elementMap;

        public ElementInfo[] AllElements
        {
            get
            {
                List<ElementInfo> infos = new();

                foreach (ElementManager em in elementMap.Values)
                {
                    infos.AddRange(em.Elements);
                }

                return infos.ToArray();
            }
        }

        #endregion

        /// <summary>
        /// Create a ClickGUI with preconfigured Values.
        /// </summary>
        public ClickGUI() : this(10, 40, 10) { }

        /// <summary>
        /// ClickGUI with custom Values.
        /// </summary>
        /// <param name="margin"></param>
        /// <param name="controlHeight">Element Height</param>
        /// <param name="controlDist">Element Distance</param>
        public ClickGUI(float margin, float controlHeight, float controlDist)
        {
            keyBindSelection = new KeyBindSelection();
            Instance = this;

            this.margin = margin;
            this.controlHeight = controlHeight;
            this.controlDist = controlDist;
            elementMap = new SortedDictionary<int, ElementManager>();
        }

        private CursorLockMode prevLockMode;
        private bool prevVisible;

        /// <summary>
        /// Show the ClickGUI.
        /// </summary>
        public void Show(bool setPrevs = true)
        {
            shown = true;
            if (setPrevs)
            {
                prevLockMode = Cursor.lockState;
                prevVisible = Cursor.visible;
            }

            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            PauseUI.paused = true;

        }

        /// <summary>
        /// Hide the ClickGUI.
        /// </summary>
        public void Hide(bool setPrevs = true)
        {
            shown = false;

            if (setPrevs)
            {
                Cursor.lockState = prevLockMode;
                Cursor.visible = prevVisible;
            }

            PauseUI.paused = false;
        }

        /// <summary>
        /// Adds an Element.
        /// </summary>
        public void AddElement(ElementInfo info)
        {
            if (!elementMap.ContainsKey(info.WindowId)) return;
            elementMap[info.WindowId].Elements.Add(info);
            info.manager = elementMap[info.WindowId];
        }

        public ElementManager GetWindow(int windowId)
        {
            return elementMap[windowId];
        }

        #region DrawWindows
        /// <summary>
        /// Adds an Window.
        /// </summary>
        /// <param name="id">ID of the Window.</param>
        /// <param name="text">Title of the Window.</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void AddWindow(int id, string text, float x, float y, float width, float height)
        {
            elementMap.Add(id, new ElementManager(id, text, x, y, width, height, margin, controlHeight, controlDist));
        }

        /// <summary>
        /// Draws added Windows, needs to be called from OnGUI().
        /// </summary>
        public void DrawWindows()
        {
            keyBindSelection.DrawSelection();
            if (Shown)
            {
                if (BlackOut)
                {
                    DrawingUtil.DrawFullScreenColor(new Color(0, 0, 0, 0.7f));
                }
            }
            else
                return;

            foreach (ElementManager manager in elementMap.Values)
            {
                if (!manager.Visible)
                    continue;

                if (manager.WindowStyle == null || manager.WindowStyle == GUIStyle.none)
                {
                    manager.WindowStyle = GUI.skin.window;
                }

                manager.WindowRect = GUI.Window(manager.WindowId, manager.WindowRect, (GUI.WindowFunction)DrawWindow, manager.text, manager.WindowStyle);

                if (!manager.AllowOffscreen)
                {
                    manager.WindowRect.x = Mathf.Clamp(manager.WindowRect.x, 0, Screen.currentResolution.width - manager.WindowRect.width);
                    manager.WindowRect.y = Mathf.Clamp(manager.WindowRect.y, 0, Screen.currentResolution.height - manager.WindowRect.height);
                }
            }
        }

        /// <summary>
        /// Update function hook, needs to be called from Update().
        /// </summary>
        public void Update()
        {
            if (keyBindSelection.Shown) return;

            if (PlayerInput.Instance != null && !Shown && !PauseUI.paused && !Cursor.visible)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

            if (Shown && Cursor.lockState != CursorLockMode.Confined)
            {
                Cursor.lockState = CursorLockMode.Confined;
            }

            if (Shown && !Cursor.visible)
            {
                Cursor.visible = true;
            }

            if (ChatBox.Instance != null && ChatBox.Instance.prop_Boolean_0)
                return;

            foreach (ElementInfo info in AllElements)
            {
                if (info.KeyBindable && info.KeyBind != KeyCode.None)
                {
                    if (Input.GetKeyDown(info.KeyBind))
                    {
                        info.Activate();
                    }
                }
            }
        }

        /// <summary>
        /// Draws the Elements of the Window.
        /// </summary>
        /// <param name="id"></param>
        void DrawWindow(int id)
        {
            ElementManager manager = elementMap[id];

            foreach (ElementInfo info in manager.Elements)
            {
                info.Render();
            }

            if (manager.Draggable)
            {
                GUI.DragWindow(new Rect(0, 0, 10000, 20));
            }
        }
        #endregion
    }
}