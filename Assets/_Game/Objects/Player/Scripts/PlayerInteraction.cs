using MP.Game.Objects.Interactable;
using MP.Game.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace MP.Game.Objects.Player.Scripts
{

    public class PlayerInteraction : MonoBehaviour
    {
        public InputActionReference InteractionAction;
        public float ActivationDotProductRange = 0.5f;
        public float InteractionSphereRadius;
        public LayerMask InteractableLayer;

        public IInteractable CurrentInteractable { get; private set; }

        public UnityEvent<IInteractable> TargetInteractableChanged;
        private Collider[] _overlappedColliders = new Collider[8];

        private void OnEnable()
        {
            InteractionAction.action.performed += OnInteracted;
        }

        private void OnDisable()
        {
            InteractionAction.action.performed -= OnInteracted;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, InteractionSphereRadius);
        }

        private void FixedUpdate()
        {
            (float comparingValue, Interactable.IInteractable interactable) selectedInteractable;
            selectedInteractable.comparingValue = 180;
            selectedInteractable.interactable = null;

            int objectCount = Physics.OverlapSphereNonAlloc(
                transform.position,
                InteractionSphereRadius,
                _overlappedColliders,
                InteractableLayer,
                QueryTriggerInteraction.Collide
                );

            float WrapAngle(float angle)
            {
                return angle < 0 ? 180 + (180 + angle) : angle;
            }

            for (int i = 0; i < objectCount; i++)
            {
                if (!_overlappedColliders[i].TryGetComponent(out IInteractable interactable))
                {
                    continue;
                }
                if (interactable.Active == false)
                    continue;
                var currentCollider = _overlappedColliders[i];

                var direction = (currentCollider.transform.position - transform.position).normalized;

                var angle = Vector3.SignedAngle(-direction, currentCollider.transform.forward, currentCollider.transform.up);
                bool interacting = false;
                float comparingValue = 180;
                if (Vector3.Distance(transform.position, currentCollider.transform.position) > interactable.CloseDetectionRange)
                {
                    if (angle > -interactable.ActiveDotProductRange && angle < interactable.ActiveDotProductRange)
                    {
                        angle = Vector3.SignedAngle(transform.forward, direction, currentCollider.transform.up);
                        if (angle > -ActivationDotProductRange && angle < ActivationDotProductRange)
                        {
                            interacting = true;
                            comparingValue = Mathf.Abs(angle);
                        }
                    }
                }
                else
                {
                    comparingValue = 0;
                    interacting = true;
                }
                
                if (!interacting)
                    continue;
                if (interactable.RequireButtonPrompt == false)
                {
                    interactable.Interact(gameObject);
                    continue;
                }

                if (comparingValue < selectedInteractable.comparingValue)
                {
                    selectedInteractable.comparingValue = comparingValue;
                    selectedInteractable.interactable = currentCollider.GetComponent<IInteractable>();
                }
            }
            if (CurrentInteractable != selectedInteractable.interactable)
            {
                CurrentInteractable = selectedInteractable.interactable;
                if (CurrentInteractable == null)
                {
                    TargetInteractableChanged?.Invoke(null);
                    return;
                }
                TargetInteractableChanged?.Invoke(CurrentInteractable);
            }
        }

        private void OnInteracted(InputAction.CallbackContext obj)
        {
            if (CurrentInteractable == null)
                return;
            CurrentInteractable.Interact(gameObject);
            CurrentInteractable = null;
            TargetInteractableChanged?.Invoke(CurrentInteractable);
        }


    }
}
