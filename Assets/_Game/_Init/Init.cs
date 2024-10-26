using UnityEngine;
using UnityEngine.SceneManagement;

namespace MP.Game.Init
{

    public class Init : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void OnAfterSceneLoad()
        {
            if (SceneManager.GetActiveScene().buildIndex != 0)
                SceneManager.LoadSceneAsync(0, LoadSceneMode.Additive);
        }
    }
}
