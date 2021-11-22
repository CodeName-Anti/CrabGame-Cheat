using UnhollowerRuntimeLib;
using UnityEngine;

namespace JNNJMods.Render
{
    public static class OutlineRenderer
    {
        public static void UnOutline(GameObject obj)
        {
            ClassInjector.RegisterTypeInIl2Cpp<Outline>();
            var outline = obj.GetComponent<Outline>();

            if(outline != null)
            {
                Object.Destroy(outline);
            }

        }

        public static void Outline(GameObject obj, Color color, int width)
        {
            

            if(HasComponent<Outline>(obj))
            {
                UnOutline(obj);
            }

            SetOutline(obj.AddComponent<Outline>(), color, width);
        }

        public static bool HasComponent<T>(GameObject obj) where T : Component
        {
            return obj.GetComponent<T>() != null;
        }

        private static void SetOutline(Outline outline, Color color, int width)
        {
            outline.OutlineColor = color;
            outline.OutlineWidth = width;
        }
    }
}
