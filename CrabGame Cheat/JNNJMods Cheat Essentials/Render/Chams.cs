using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JNNJMods.Render
{
    public class Chams
    {
        public bool Cham;

        private Component[] chammed;
        private Dictionary<int, Material> origMaterials;

        public Chams(bool cham = true)
        {
            origMaterials = new Dictionary<int, Material>();
            Cham = cham;
        }

        public void UnChamTargets()
        {
            if (chammed == null || chammed.Length <= 0)
                return;

            foreach (Component component in chammed)
            {
                foreach (SkinnedMeshRenderer componentsInChild in component.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    componentsInChild.material = origMaterials[componentsInChild.GetInstanceID()];
                }
            }
            chammed = null;
        }

        
        public void ChamTargets(Component[] targets)
        {
            ChamTargets(targets, Color.red);
        }

        public void ChamTargets(Component[] targets, Color color)
        {
            if (chammed != null)
                chammed = chammed.Concat(targets).ToArray();
            else
                chammed = targets;
            
            
            if (Cham && targets != null)
            {
                foreach (Component component in targets)
                {
                    
                    foreach (SkinnedMeshRenderer componentsInChild in component.GetComponentsInChildren<SkinnedMeshRenderer>())
                    {
                        if (!origMaterials.ContainsKey(componentsInChild.GetInstanceID()))
                            origMaterials.Add(componentsInChild.GetInstanceID(), new Material(componentsInChild.material));

                        componentsInChild.material.shader = Shader.Find("Hidden/Internal-Colored");
                        componentsInChild.material.SetInt("_ZTest", 0);
                        componentsInChild.material.color = color;
                    }
                    
                }
            }
        }

    }
}
