using UnityEngine;
using UnityEngine.Events;

namespace MP.Game.Utils
{
    public class UnityLifetimeCallbacks : MonoBehaviour
    {
        public UnityEvent OnAwake;
        public UnityEvent OnStart;
        public UnityEvent Enabled;
        public UnityEvent Disabled;

        private void Awake()
        {
            OnAwake?.Invoke();
        }

        private void Start()
        {
            OnStart?.Invoke();
        }

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
