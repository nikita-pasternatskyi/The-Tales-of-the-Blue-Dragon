using MP.Game.Utils;
using UnityEngine;

namespace MP.Game.Objects.Player.Scripts
{
    public class PausedController : MonoBehaviour 
    {
        [SerializeField] private ScriptableEvent _pausedGame;
        [SerializeField] private ScriptableEvent _unpausedGame;
        [SerializeField] private MonoBehaviour[] _componentsToDisable;

        private void OnEnable()
        {
            _pausedGame.Event += OnGamePaused;
            _unpausedGame.Event += OnGameUnpaused;
        }

        private void OnDisable()
        {
            _pausedGame.Event -= OnGamePaused;
            _unpausedGame.Event -= OnGameUnpaused;
        }

        private void OnGameUnpaused()
        {
            foreach (var item in _componentsToDisable)
            {
                if (!item.enabled)
                    item.enabled = true;
            }
        }

        private void OnGamePaused()
        {
            foreach (var item in _componentsToDisable)
            {
                if (item.enabled)
                    item.enabled = false;
            }
        }
    }
}
