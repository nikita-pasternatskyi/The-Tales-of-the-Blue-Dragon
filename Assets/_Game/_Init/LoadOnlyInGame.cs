using MP.Game.Assets._Game.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MP.Game
{
    public class LoadOnlyInGame : MonoBehaviour
    {
        [SerializeField] private LoadSceneEvent _event;
        [SerializeField] private int _sceneToLoad;

        private void Start()
        {
            if(Application.isEditor == false)
                _event.Invoke(_sceneToLoad);
        }
    }
}
