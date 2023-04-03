using UnityEngine;


namespace Shared
{
    public static class Utils
    {
        public static float CoordinateDifference(Vector3 p1, Vector3 p2)
        {
            return Mathf.Abs(p1.x - p2.x) + Mathf.Abs(p1.y - p2.y) + Mathf.Abs(p1.z - p2.z);
        }
    }
}