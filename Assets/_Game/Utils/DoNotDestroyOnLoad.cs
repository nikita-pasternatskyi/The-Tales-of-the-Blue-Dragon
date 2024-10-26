using UnityEngine;

namespace MP.Game.Utils
{
    public class DoNotDestroyOnLoad : MonoBehaviour
    {
        private void Start()
        {
            gameObject.transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }
    }
}