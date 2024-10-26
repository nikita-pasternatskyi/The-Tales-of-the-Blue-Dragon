using UnityEngine;
using UnityEngine.Events;

namespace MP.Game.Utils
{
    public class ScriptableEventListner : MonoBehaviour
    {
        [SerializeField] private ScriptableEvent _target;
        public UnityEvent Response;

        private void OnEnable()
        {
            _target.Event += Response.Invoke;
        }

        private void OnDisable()
        {
            _target.Event -= Response.Invoke;
        }
    }
}
