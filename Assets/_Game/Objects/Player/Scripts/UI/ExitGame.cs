using MP.Game.Utils;
using UnityEngine;

namespace MP.Game.Objects.Player.Scripts.UI
{
    public class ExitGame : MonoBehaviour
    {
        [SerializeField] private ScriptableEvent _event;

        private void OnEnable()
        {
            _event.Event += OnExitRequested;
        }

        private void OnDisable()
        {
            _event.Event -= OnExitRequested;
        }

        private void OnExitRequested()
        {
            Application.Quit();
        }

    }
}
