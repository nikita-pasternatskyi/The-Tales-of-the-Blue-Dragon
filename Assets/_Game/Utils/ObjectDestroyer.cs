using UnityEngine;

namespace MP.Game.Utils
{
    public class ObjectDestroyer : MonoBehaviour
    {
        public GameObject[] ObjectsToDestroy;

        public void DestroyAll()
        {
            foreach (var obj in ObjectsToDestroy)
            {
                Destroy(obj);
            }
        }
    }
}