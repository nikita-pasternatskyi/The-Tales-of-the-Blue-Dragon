using Sirenix.OdinInspector;
using UnityEngine;

namespace MP.Game.Assets.Utils
{
    public class Singleton<T> where T : new()
    {
        public static T Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new T();
                return _instance;
            }
            private set
            {
                _instance = value;
            }
        }
        private static T _instance;
    }

    public class UnitySingleton<T> : SerializedMonoBehaviour where T : Component
    {
        [SerializeField] private bool _dontDestroyOnLoad = true;
        public static T Instance { get; private set; }

        private void Awake()
        {
            EnsureSingleton();
        }

        protected void EnsureSingleton()
        {
            if (Instance == null)
            {
                Instance = this as T;
                if (_dontDestroyOnLoad)
                    DontDestroyOnLoad(gameObject);
                return;
            }
            Destroy(this);
        }
    }
}


