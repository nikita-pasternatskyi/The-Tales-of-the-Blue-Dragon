using UnityEngine;

namespace MP.Game.Utils.Extensions
{
    public static class Vector3Extensions
    {
        public static Vector3 XZ(this Vector3 v)
        {
            return new Vector3(v.x, 0, v.z);
        }
    }
}
