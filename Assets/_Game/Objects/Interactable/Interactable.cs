using MP.Game.Utils;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace MP.Game.Objects.Interactable
{
    public class Interactable : MonoBehaviour, IInteractable
    {
        [field: SerializeField] public bool RequireButtonPrompt { get; private set; }
        [field: SerializeField] public string InteractionName { get; private set; }
        [field: SerializeField] public float CloseDetectionRange { get; private set; }
        [field: SerializeField, Range(0, 180)] public float ActiveDotProductRange { get; private set; }

        public bool Active => enabled;


        public UnityEvent<GameObject> InteractedUnity;
        public event Action<GameObject> Interacted;

        private void OnDrawGizmosSelected()
        {
            var min = Quaternion.AngleAxis(ActiveDotProductRange, Vector3.up) * transform.forward;
            var max = Quaternion.AngleAxis(-ActiveDotProductRange, Vector3.up) * transform.forward;
            Gizmos.DrawRay(transform.position, min * 10);
            Gizmos.DrawRay(transform.position, max * 10);
            Gizmos.DrawWireSphere(transform.position, CloseDetectionRange);

        }

        private void OnEnable()
        {
            Interacted += InteractedUnity.Invoke;
        }

        private void OnDisable()
        {
            Interacted -= InteractedUnity.Invoke;
        }

        public virtual void Interact(GameObject sender)
        {
            Interacted?.Invoke(sender);
        }
    }
}
