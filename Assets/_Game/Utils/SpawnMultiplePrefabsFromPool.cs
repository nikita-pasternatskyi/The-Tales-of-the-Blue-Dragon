using UnityEngine;

namespace MP.Game.Utils
{
    public class SpawnMultiplePrefabsFromPool : MonoBehaviour
    {
        [SerializeField] private GameObject[] _prefabs;
        public void Spawn()
        {
            foreach (var prefab in _prefabs)
            {
                var instance = GlobalObjectPool.Instance.Get(prefab);
                instance.transform.position = transform.position;
                instance.transform.rotation = transform.rotation;
                instance.transform.localScale = transform.localScale;
                instance.SetActive(true);
            }
        }
    }
}
