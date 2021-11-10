using UnityEngine;

namespace JNNJMods.Render
{
    public static class OutlineRenderer
    {
        public static void UnOutlinePlayer(PlayerManager manager)
        {
            var customization = manager.playerCustomization;

            var sOutline = customization.sweater.GetComponent<Outline>();
            var pOutline = customization.pants.GetComponent<Outline>();

            if(sOutline != null)
            {
                Object.Destroy(sOutline);
            }

            if(pOutline != null)
            {
                Object.Destroy(pOutline);
            }
        }

        public static void OutlinePlayer(PlayerManager manager, Color color, int width)
        {
            var customization = manager.playerCustomization;

            if(HasComponent<Outline>(customization.sweater) || HasComponent<Outline>(customization.pants))
            {
                UnOutlinePlayer(manager);
            }

            SetOutline(customization.sweater.AddComponent<Outline>(), color, width);
            SetOutline(customization.pants.AddComponent<Outline>(), color, width);
        }

        public static bool HasComponent<T>(GameObject obj) where T : Component
        {
            return obj.GetComponent<T>() != null;
        }

        private static void SetOutline(Outline outline, Color color, int width)
        {
            outline.outlineColor = color;
            outline.outlineWidth = width;
        }
    }
}
