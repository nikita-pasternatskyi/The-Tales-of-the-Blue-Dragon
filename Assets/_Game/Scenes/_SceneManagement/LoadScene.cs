using UnityEngine;

namespace MP.Game.Assets._Game.SceneManagement
{
    public class LoadScene : MonoBehaviour
    {
        [SerializeField] private LoadSceneEvent _loadSceneEvent;
        [SerializeField] private int _sceneIDToLoad;
        [SerializeField] private bool _loadOnAwake;

        private void Awake()
        {
            if (_loadOnAwake)
                Load();
        }

        public void Load() => _loadSceneEvent.Invoke(_sceneIDToLoad);
    }

}
