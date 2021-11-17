using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JNNJMods.UI.Utils
{
    public class ESP
    {
        public bool Line = true;
        public Color LineColor = Color.green;
        public int LineThickness = 1;

        public bool Box = true;
        public Color BoxColor = Color.green;
        public int BoxThickness = 4;

        public bool String = true;
        public Color StringColor = Color.green;
        public int StringFontSize = 25;

        private static readonly Material LineMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));

        public ESP()
        {
            LineMaterial.hideFlags = HideFlags.HideAndDontSave;
            LineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
        }

        public ESP(Color lineColor, Color boxColor, Color stringColor) : this()
        {
            LineColor = lineColor;
            BoxColor = boxColor;
            StringColor = stringColor;
        }

        public ESP(
            bool line, Color lineColor,
            bool box, Color boxColor,
            bool @string, Color stringColor) :

            this(lineColor, boxColor, stringColor)
        {
            Line = line;
            Box = box;
            String = @string;
        }

        public ESP(
            bool line, Color lineColor, int lineThickness,
            bool box, Color boxColor, int boxThickness,
            bool @string, Color stringColor, int stringFontSize) :

            this(line, lineColor, box, boxColor, @string, stringColor)
        {
            LineThickness = lineThickness;
            BoxThickness = boxThickness;
            StringFontSize = stringFontSize;
        }

        public void Draw(Dictionary<GameObject, string> objs)
        {
            Draw(objs.Keys.ToArray(), objs.Values.ToArray());
        }

        public void Draw(GameObject[] targets, string[] names)
        {
            Rect rect = default;
            if (targets == null)
            {
                return;
            }
            for (int i = 0; i < targets.Length; i++)
            {
                Vector3 vector = targets[i].transform.position;
                Vector3 vector2 = vector;
                vector2.y += 1.8f;
                vector = Camera.main.WorldToScreenPoint(vector);
                vector2 = Camera.main.WorldToScreenPoint(vector2);
                if (vector.z > 0f && vector2.z > 0f)
                {
                    Vector3 vector3 = GUIUtility.ScreenToGUIPoint(vector);
                    vector3.y = Screen.height - vector3.y;
                    Vector3 vector4 = GUIUtility.ScreenToGUIPoint(vector2);
                    vector4.y = Screen.height - vector4.y;
                    float num = Math.Abs(vector3.y - vector4.y) / 2.2f;
                    float num2 = num / 2f;
                    rect = new Rect(new Vector2(vector4.x - num2, vector4.y), new Vector2(num, vector3.y - vector4.y));
                }

                if (String)
                {
                    Vector2 txtPos = new Vector2(rect.x, rect.y - 40);

                    DrawString(txtPos, names[i], StringColor, StringFontSize, false);
                }

                if (Box)
                {
                    DrawRectangle(rect, BoxColor, 4);
                }

                if (Line)
                {
                    DrawLine(new Vector3(Screen.width / 2f, Screen.height / 2f), new Vector3(rect.center.x, rect.center.y), LineColor, 1);
                }
            }
        }

        private static GUIStyle StringStyle { get; set; }

        private static void DrawString(Vector2 position, string label, Color textColor, int fontSize, bool centered = true)
        {
            if (StringStyle == null)
            {
                StringStyle = GUI.skin.label;
                StringStyle.fontSize = fontSize;
                StringStyle.normal.textColor = textColor;
            }
            var content = new GUIContent(label);
            var size = StringStyle.CalcSize(content);
            var upperLeft = centered ? position - size / 2f : position;
            GUI.Label(new Rect(upperLeft, size), content, StringStyle);
        }

        /// <summary>
        /// Draws a Line in 3D.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="color"></param>
        /// <param name="thickness"></param>
        private static void DrawLine(Vector3 start, Vector3 end, Color color, int thickness)
        {
            LineMaterial.SetPass(0);
            if (thickness == 0)
            {
                return;
            }
            if (thickness == 1)
            {
                GL.Begin(1);
                GL.Color(color);
                GL.Vertex3(start.x, start.y, start.z);
                GL.Vertex3(end.x, end.y, end.z);
                GL.End();
                return;
            }
            thickness /= 2;
            GL.Begin(7);
            GL.Color(color);
            GL.Vertex3(start.x - thickness, start.y - thickness, start.z - thickness);
            GL.Vertex3(start.x + thickness, start.y + thickness, start.z + thickness);
            GL.Vertex3(end.x + thickness, end.y + thickness, end.z + thickness);
            GL.Vertex3(end.x - thickness, end.y - thickness, end.z - thickness);
            GL.End();
        }

        /// <summary>
        /// Draws a Rectangle.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="color"></param>
        /// <param name="thickness"></param>
        private static void DrawRectangle(Rect rect, Color color, int thickness)
        {
            Vector3 vector = new Vector3(rect.x, rect.y, 0f);
            Vector3 vector2 = new Vector3(rect.x + rect.width, rect.y, 0f);
            Vector3 vector3 = new Vector3(rect.x + rect.width, rect.y + rect.height, 0f);
            Vector3 vector4 = new Vector3(rect.x, rect.y + rect.height, 0f);
            DrawLine(vector, vector2, color, thickness);
            DrawLine(vector2, vector3, color, thickness);
            DrawLine(vector3, vector4, color, thickness);
            DrawLine(vector4, vector, color, thickness);
        }
    }
}
