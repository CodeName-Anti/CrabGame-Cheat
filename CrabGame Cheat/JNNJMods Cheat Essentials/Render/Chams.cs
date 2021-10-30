using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace JNNJMods.Render
{
    public class Chams
    {
        private static AssetBundle assets;
        private static Material ChamMaterial;
        private static Material[] ChamMats;

        public bool Cham = true;

        public Chams()
        {
            LoadResources();
        }

        #region Resources
        /// <summary>
        /// Unloads the Resources used by the <see cref="Chams"/>.
        /// </summary>
        public static void UnloadResources()
        {
            if(assets != null)
                assets.Unload(true);
        }

        private static void LoadResources()
        {
            if(assets == null)
            {
                assets = AssetBundle.LoadFromMemory(GetResourceBytes(typeof(Chams) + ".Resources.chams.assets"));
            }

            if(ChamMaterial == null)
            {
                foreach (UnityEngine.Object obj in assets.LoadAllAssets())
                {
                    if (obj is Material)
                    {
                        ChamMaterial = obj as Material;
                    }
                }

                ChamMaterial.hideFlags = HideFlags.HideAndDontSave;
                ChamMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
            }

            if(ChamMats == null || ChamMats.Length != 10)
            {
                //Populate ChamMats
                ChamMats = new Material[10];

                for (int i = 0; i < 10; i++)
                {
                    ChamMats[i] = ChamMaterial;
                }
            }
        }

        private static byte[] GetResourceBytes(string embeddedResource)
        {
            using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(embeddedResource))
            {
                byte[] buffer = manifestResourceStream != null ? new byte[(int)manifestResourceStream.Length] :
                    throw new Exception(embeddedResource + " is not found in Embedded Resources.");
                manifestResourceStream.Read(buffer, 0, (int)manifestResourceStream.Length);
                if (buffer.Length > 1000)
                    return buffer;
            }
            return null;
        }
        #endregion

        public void Draw(Renderer[] targets)
        {
            if (Cham && targets != null)
            {
                foreach (Renderer renderer in targets)
                {
                    if (!Camera.main || (renderer.transform.root.position - Camera.main.transform.position).magnitude >= 3f)
                    {
                        renderer.materials = ChamMats;
                    }
                }
            }
        }

    }
}
