using UnityEngine;

namespace MP.Game.Utils
{
    public class UnparentOnAwake : MonoBehaviour
    {
        private void Awake()
        {
            transform.parent = null;
        }
    }
}
