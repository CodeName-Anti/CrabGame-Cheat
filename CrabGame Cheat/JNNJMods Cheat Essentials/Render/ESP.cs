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

        public static Texture2D lineTex;

        public ESP(Color lineColor, Color boxColor, Color stringColor)
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

        public void DrawSingle(GameObject target, string name = null)
        {
            Rect rect = default;
            Vector3 vector = target.transform.position;
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
                Vector2 txtPos = new(rect.x, rect.y - 40);

                DrawString(txtPos, name ?? target.name, StringColor, StringFontSize, false);
            }

            if (Box)
            {
                DrawRectangle(rect, BoxColor);
            }

            if (Line)
            {
                DrawLine(new Vector3(Screen.width / 2f, Screen.height / 2f), new Vector3(rect.center.x, rect.center.y), LineColor, 2);
            }
        }

        public void Draw(GameObject[] targets, string[] names)
        {
            if (targets == null || names == null)
                return;

            for (int i = 0; i < targets.Length; i++)
            {
                if (targets[i] == null)
                    continue;

                DrawSingle(targets[i], names[i]);
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
        private static void DrawLine(Vector2 pointA, Vector2 pointB, Color color, float width)
        {
            Matrix4x4 matrix = GUI.matrix;
            if (!lineTex)
            {
                lineTex = new Texture2D(1, 1);
                lineTex.SetPixel(1, 1, color);

                lineTex.wrapMode = TextureWrapMode.Repeat;
                lineTex.Apply();

            }
            float num = Vector3.Angle(pointB - pointA, Vector2.right);
            if (pointA.y > pointB.y)
            {
                num = -num;
            }
            GUIUtility.ScaleAroundPivot(new Vector2((pointB - pointA).magnitude, width), new Vector2(pointA.x, pointA.y + 0.5f));
            GUIUtility.RotateAroundPivot(num, pointA);
            GUI.DrawTexture(new Rect(pointA.x, pointA.y, 1f, 1f), lineTex);
            GUI.matrix = matrix;
        }

        /// <summary>
        /// Draws a Rectangle.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="color"></param>
        /// <param name="thickness"></param>
        private static void DrawRectangle(Rect rect, Color color)
        {
            Vector3 vector = new(rect.x, rect.y, 0f);
            Vector3 vector2 = new(rect.x + rect.width, rect.y, 0f);
            Vector3 vector3 = new(rect.x + rect.width, rect.y + rect.height, 0f);
            Vector3 vector4 = new(rect.x, rect.y + rect.height, 0f);
            DrawLine(vector, vector2, color, 2);
            DrawLine(vector2, vector3, color, 2);
            DrawLine(vector3, vector4, color, 2);
            DrawLine(vector4, vector, color, 2);
        }
    }
}
