using MP.Game.Assets.Utils;
using MP.Game.Utils;
using UnityEngine;

namespace MP.Game.Objects
{
    public class Game : UnitySingleton<Game>
    {
        [SerializeField] private ScriptableEvent _pausedGame;
        [SerializeField] private ScriptableEvent _unpausedGame;

        public bool Paused { get; private set; }

        public void PauseGame()
        {
            Time.timeScale = 0;
            Paused = true;
            _pausedGame?.Invoke();
        }

        public void UnpauseGame() 
        {
            Time.timeScale = 1;
            Paused = false;
            _unpausedGame?.Invoke();
        }
    }
}
