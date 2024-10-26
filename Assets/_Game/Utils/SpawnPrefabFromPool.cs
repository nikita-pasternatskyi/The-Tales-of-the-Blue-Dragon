using UnityEngine;

namespace MP.Game.Utils
{
    public class SpawnPrefabFromPool : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;
        public void Spawn()
        {
            var instance = GlobalObjectPool.Instance.Get(_prefab);
            instance.transform.position = transform.position;
            instance.transform.rotation = transform.rotation;
            instance.transform.localScale = transform.localScale;
            instance.SetActive(true);
        }
    }
}
