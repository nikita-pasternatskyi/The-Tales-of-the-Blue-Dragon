using MP.Game.Assets.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace MP.Game.Utils
{
    public class GlobalObjectPool : UnitySingleton<GlobalObjectPool>
    {
        private Dictionary<GameObject, List<GameObject>> _pooledObjects = new Dictionary<GameObject, List<GameObject>>();

        public GameObject Get(GameObject prefab)
        {
            GameObject InstantiateAndAddToList(in GameObject prefab, in List<GameObject> targetList)
            {
                var instance = Instantiate(prefab, transform);
                instance.SetActive(false);
                targetList.Add(instance);
                return instance;
            }

            if (_pooledObjects.TryGetValue(prefab, out List<GameObject> list))
            {
                foreach (var go in list)
                {
                    if(!go.activeInHierarchy)
                    {
                        return go;
                    }
                }
                return InstantiateAndAddToList(prefab, list);
            }
            var newList = new List<GameObject>();
            var instance = InstantiateAndAddToList(prefab, newList);
            _pooledObjects.Add(prefab, newList);
            return instance;
        }
    }
}
