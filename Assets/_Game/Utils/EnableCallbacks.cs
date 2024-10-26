using UnityEngine;
using UnityEngine.Events;

namespace MP.Game.Utils
{
    public class EnableCallbacks : MonoBehaviour
    {
        public UnityEvent Enabled;
        public UnityEvent Disabled;

        private void OnEnable()
        {
            Enabled?.Invoke();
        }

        private void OnDisable()
        {
            Disabled?.Invoke();
        }
    }
}
