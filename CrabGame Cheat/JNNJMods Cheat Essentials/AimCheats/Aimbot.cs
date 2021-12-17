using JNNJMods.Utils;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace JNNJMods.AimCheats
{
    public class Aimbot
    {
        public double
            Smooth = 2,
            YOffset = 0;

        public bool Enabled = false;
        public bool TriggerBot = false;
        public float FOV = 900;
        public LayerMask mask;

        private bool IsOnEnemy()
        {
            // RayCast to see if Collider is player
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 9999f, mask))
            {
                return IsEnemy(hit);
            }
            return false;
        }

        private bool IsEnemy(RaycastHit hit)
        {
            string name = hit.transform.name.ToLower();
            if (name.Contains("player") || hit.collider.gameObject.layer == mask)
            {
                return true;
            }
            else
                return false;
        }

        private bool IsVisable(Vector3 toCheck)
        {
            // Raycast to enemy position, to see if the enemy is visible
            if (Physics.Linecast(Camera.main.transform.position, toCheck, out RaycastHit hit, mask))
            {
                return IsEnemy(hit);
            }
            return false;
        }

        public void Trigger()
        {
            if (!TriggerBot)
                return;

            if (IsOnEnemy())
            {
                Shoot();
            }
        }

        // Update is called once per frame
        public void Aim(GameObject[] targets, bool randomize = false)
        {
            if (!Enabled)
                return;

            float minDist = 99999;
            Vector2 AimTarget = Vector2.zero;

            // Randomize aim targets
            if (randomize)
            {
                targets = targets.OrderBy(m => Random.RandomRangeInt(0, 99999)).ToArray();
            }

            foreach (GameObject obj in targets)
            {
                if (!IsVisable(obj.transform.position)) continue;

                var shit = Camera.main.WorldToScreenPoint(obj.transform.position);
                if (shit.z > -8)
                {
                    float dist = System.Math.Abs(Vector2.Distance(new Vector2(shit.x, Screen.height - shit.y), new Vector2((Screen.width / 2), (Screen.height / 2))));

                    // In fov
                    if (FOV == -1 || dist < FOV) 
                    {
                        if (dist < minDist)
                        {
                            minDist = dist;
                            AimTarget = new Vector2(shit.x, Screen.height - shit.y);
                        }
                    }
                }
            }

            if (AimTarget != Vector2.zero)
            {
                double DistX = AimTarget.x - Screen.width / 2.0f;
                double DistY = AimTarget.y - Screen.height / 2.0f + YOffset;

                // Aimsmooth
                DistX /= Smooth;
                DistY /= Smooth;

                if (Enabled)
                {
                    // Send Mouse Event for moving mouse
                    MouseOperations.mouse_event(0x0001, (int)DistX, (int)DistY, 0, 0);
                }
            }
        }

        private static async void Shoot()
        {
            MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftDown);
            await Task.Delay(Random.RandomRangeInt(0, 300));
            MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp);
        }

    }
}
