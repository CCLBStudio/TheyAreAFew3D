using UnityEngine;

namespace Gameplay.Extensions
{
    public static class FrustumHelper
    {
        public static bool TryGetOutsideFrustumPoint(Camera cam, Vector3 targetPos, Vector3 playerPos, out Vector3 result, float offset = 0f)
        {
            result = Vector3.zero;

            Vector3 dir = (playerPos - targetPos).normalized;
            Ray ray = new Ray(targetPos, dir);

            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);

            float closestDistance = float.MaxValue;
            Vector3 intersectionPoint = Vector3.zero;
            bool found = false;

            foreach (Plane plane in planes)
            {
                if (!plane.Raycast(ray, out float dist))
                {
                    continue;
                }

                if (!(dist < closestDistance))
                {
                    continue;
                }
            
                closestDistance = dist;
                intersectionPoint = ray.GetPoint(dist);
                found = true;
            }

            if (!found)
            {
                return false;
            }

            result = intersectionPoint + dir * offset;
            return true;
        }
    }
}