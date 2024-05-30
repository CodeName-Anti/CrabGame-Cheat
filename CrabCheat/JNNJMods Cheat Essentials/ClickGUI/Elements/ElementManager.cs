using System.Collections.Generic;
using UnityEngine;

namespace JNNJMods.UI.Elements
{
    public class ElementManager
    {
        #region Variables
        /// <summary>
        /// Elements of the Window.
        /// </summary>
        public readonly List<ElementInfo> Elements;

        /// <summary>
        /// ID of the Window.
        /// </summary>
        public readonly int WindowId;

        /// <summary>
        /// Defines if the Window is draggable.
        /// </summary>
        public bool Draggable = true;

        /// <summary>
        /// Position of the Window.
        /// </summary>
        public Rect WindowRect;

        /// <summary>
        /// Style of the Window.
        /// </summary>
        /// 
        public GUIStyle WindowStyle;

        /// <summary>
        /// Defines if the Window can be dragged of Screen.
        /// </summary>
        public bool AllowOffscreen;

        public bool Visible = true;

        public float
            x, y,
            width, height,
            margin,
            controlHeight,
            controlDist;
        private float nextControlY;

        /// <summary>
        /// Title of the Window
        /// </summary>
        public string text;
        #endregion

        public ElementManager(int windowId, string text, float _x, float _y, float _width, float _height, float _margin, float _controlHeight, float _controlDist)
        {
            x = _x;
            y = _y;
            width = _width;
            height = _height;
            margin = _margin;
            controlHeight = _controlHeight;
            controlDist = _controlDist;
            this.text = text;
            Elements = new List<ElementInfo>();
            WindowRect = new Rect(x, y, width, height);
            WindowId = windowId;
            x = 0;
            y = 0;
            nextControlY = y + 20;
        }

        /// <summary>
        /// Next Element position based on the index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Rect NextControlRect(int index)
        {

            //Rect r = new Rect(x, nextControlY, width - margin * 2, controlHeight);
            //Rect r = new Rect(x + margin, nextControlY, width - margin * 2, controlHeight);
            Rect r = new Rect(0 + margin, (index + 1) * (controlDist + controlHeight), width - margin * 2, controlHeight);
            return r;
        }

    }
}
