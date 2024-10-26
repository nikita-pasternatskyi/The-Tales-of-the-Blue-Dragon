using MP.Game.Objects.Player.Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace MP.Game.Utils
{

    public class CollisionCallbacks : MonoBehaviour
    {
        public UnityEvent Entered;
        public UnityEvent Exited;
        public UnityEvent<Player> PlayerEntered;
        public UnityEvent<Player> PlayerExited;

        [SerializeField] private bool _checkForPlayerOnly;

        private void OnTriggerEnter(Collider other)
        {
            if (!enabled)
                return;
            var isPlayer = other.TryGetComponent<Player>(out var player);
            if (_checkForPlayerOnly)
            {
                if (!isPlayer)
                    return;
            }
            if(isPlayer)
                PlayerEntered?.Invoke(player);
            Entered?.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!enabled)
                return;
            var isPlayer = other.TryGetComponent<Player>(out var player);
            if (_checkForPlayerOnly)
            {
                if (!isPlayer)
                    return;
            }
            if (isPlayer)
                PlayerExited?.Invoke(player);
            Exited?.Invoke();
        }
    }
}
