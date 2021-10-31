using UnityEngine;

namespace JNNJMods.Render
{
    public class DrawingUtil
    {

        public static GUIStyle GetTextStyle(int fontSize, Color textColor)
        {
            GUIStyle style = new GUIStyle(GUI.skin.label)
            {
                font = Resources.GetBuiltinResource<Font>("Arial.ttf"),
                fontSize = fontSize,
                alignment = TextAnchor.MiddleCenter,
            };
            
            style.normal.textColor = textColor;

            return style;
        }

        public static GUIStyle GetTextStyle(int fontSize)
        {
            return GetTextStyle(fontSize, Color.clear);
        }

        public static Rect CenteredTextRect(string text, int fontSize)
        {
            GUIStyle style = GetTextStyle(fontSize);

            Vector2 size = style.CalcSize(new GUIContent(text));
            float textWidth = size.x * 2;
            float textHeight = size.y * 2;

            return new Rect(Screen.currentResolution.width / 2 - textWidth / 2, Screen.currentResolution.height / 2 - size.y,
                textWidth, textHeight);
        }

        private static Rect CalcTextSize(float x, float y, GUIStyle style, string text)
        {
            Vector2 size = style.CalcSize(new GUIContent(text));
            float textWidth = size.x * 2;
            float textHeight = size.y * 2;

            return new Rect(x, y, textWidth, textHeight);
        }

        public static void DrawText(string text, float x, float y, int fontSize, Color textColor)
        {
            GUIStyle style = GetTextStyle(fontSize, textColor);

            GUI.Label(CalcTextSize(x, y, style, text), text, style);
        }

        public static void DrawText(string text, Rect pos, int fontSize, Color textColor)
        {
            GUIStyle style = GetTextStyle(fontSize, textColor);

            GUI.Label(pos, text, style);
        }

        public static void DrawCenteredText(string text, int fontSize, Color textColor)
        {
            DrawText(text, CenteredTextRect(text, fontSize), fontSize, textColor);
        }

        public static readonly Color TransparentBlack = new Color(0, 0, 0, 0.3f);

        /// <summary>
        /// Makes the Input color Transparent
        /// </summary>
        /// <param name="color">Color to make Transparent</param>
        /// <returns>Transparent version of the Input Color</returns>
        public static Color MakeColorTransparent(Color color)
        {
            color.a = 0.3f;
            return color;
        }

        /// <summary>
        /// Draw a Color at a location
        /// </summary>
        /// <param name="color"></param>
        /// <param name="rect"></param>
        public static void DrawColor(Color color, Rect rect)
        {
            Texture2D tex = new Texture2D(1, 1);

            tex.SetPixel(1, 1, color);

            tex.wrapMode = TextureWrapMode.Repeat;
            tex.Apply();

            GUI.DrawTexture(rect, tex);
        }

        /// <summary>
        /// Draws a Color fullscreen
        /// </summary>
        /// <param name="color"></param>
        public static void DrawFullScreenColor(Color color)
        {
            Texture2D tex = new Texture2D(1, 1);

            tex.SetPixel(1, 1, color);

            GUI.Box(new Rect(0, 0, Screen.width + 100, Screen.height + 100), tex);
        }
    }
}
