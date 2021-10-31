using UnityEngine;

namespace JNNJMods.CrabGameCheat.Util
{
    public class RainbowColor
    {
        private Color color;

        public float Speed;

        public RainbowColor(float speed = 0.25f)
        {
            Speed = speed;
            color = Color.HSVToRGB(.34f, .84f, .67f);
        }

        public Color GetColor()
        {
            Color.RGBToHSV(color, out float h, out float s, out float v);
            return color = Color.HSVToRGB(h + Time.deltaTime * Speed, s, v);
        }

    }
}
