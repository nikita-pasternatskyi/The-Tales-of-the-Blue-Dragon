using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MP.Game.Assets._Game.SceneManagement
{
    public class SceneLoad : MonoBehaviour
    {
        [SerializeField] private LoadSceneEvent _loadSceneEvent;
        [SerializeField] private LoadSceneEvent _loadSceneAdditivelyEvent;
        [SerializeField] private GameObject _loadingScreen;
        [SerializeField] private Image _loadingScreenProgressBar;
        [SerializeField] private int _firstSceneID;

        public UnityEvent LoadingStarted;
        public UnityEvent LoadingFinished;
        private int _lastLoadedSceneIndex;

        private void OnEnable()
        {
            _loadSceneEvent.GenericEvent += OnLoadSceneAsked;
            _loadSceneAdditivelyEvent.GenericEvent += OnLoadSceneAdditivelyAsked;
        }

        private void Start()
        {
            if(!Application.isEditor)
                StartCoroutine(LoadScene(_firstSceneID, false));
        }

        private void OnDisable()
        {
            _loadSceneEvent.GenericEvent -= OnLoadSceneAsked;
            _loadSceneAdditivelyEvent.GenericEvent -= OnLoadSceneAdditivelyAsked;
        }

        private void OnLoadSceneAdditivelyAsked(int obj)
        {
            SceneManager.LoadSceneAsync(obj, LoadSceneMode.Additive);
        }

        private void OnLoadSceneAsked(int obj)
        {
            StartCoroutine(LoadScene(obj));
        }

        private IEnumerator LoadScene(int scene, bool showLoadingScreen = true)
        {
            LoadingStarted?.Invoke();
            if(showLoadingScreen)
                _loadingScreen.SetActive(true);
            yield return null;

            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            var progress = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            _lastLoadedSceneIndex = scene;
            while (!progress.isDone)
            {
                _loadingScreenProgressBar.fillAmount = progress.progress;
                yield return null;
            }
            var sceneInstance = SceneManager.GetSceneByBuildIndex(scene);
            SceneManager.SetActiveScene(sceneInstance);
            if (showLoadingScreen)
                _loadingScreen.SetActive(false);
            LoadingFinished?.Invoke();
        }

        private void OnSceneLoadCompleted(AsyncOperation obj)
        {
            var scene = SceneManager.GetSceneByBuildIndex(_lastLoadedSceneIndex);
            Debug.Log(scene.isLoaded);
            SceneManager.SetActiveScene(scene);
        }
    }

}
