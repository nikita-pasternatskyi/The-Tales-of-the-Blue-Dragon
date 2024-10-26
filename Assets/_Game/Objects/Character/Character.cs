using MP.Game.Utils;
using MP.Game.Utils.Extensions;
using UnityEngine;

namespace MP.Game.Objects.Character
{
    [RequireComponent(typeof(CharacterController))]
    public class Character : MonoBehaviour
    {
        [SerializeField] private ScriptableEvent _enableCharacterEvent;
        [SerializeField] private ScriptableEvent _disableCharacterEvent;
        public CharacterConfig Configuration;

        [Header("Ground Check")]
        public float GroundCheckDistance;
        public Vector3 GroundCheckBoxSize;
        public LayerMask GroundLayer;

        private float _characterControllerDisableTimer;

        public bool Grounded { get; private set; }
        public Vector3 Velocity;
        private CharacterController _characterController;
        private float _rotationSmoothDampVelocity;

        private void OnEnable()
        {
            _enableCharacterEvent.Event += OnCharacterEnable;
            _disableCharacterEvent.Event += OnCharacterDisable;
        }
        
        private void OnDisable()
        {
            _enableCharacterEvent.Event -= OnCharacterEnable;
            _disableCharacterEvent.Event -= OnCharacterDisable;
        }

        private void OnCharacterDisable()
        {
            _characterController.enabled = false;
            Velocity = Vector3.zero;
        }

        private void OnCharacterEnable()
        {
            _characterController.enabled = true;
        }

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            if (Configuration == null)
            {
                Debug.LogError("No character config found, Character won't work", gameObject);
                enabled = false;
                return;
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (Configuration == null)
            {
                Debug.LogWarning("No character config found, Character won't work", gameObject);
                return;
            }
            Gizmos.color = Color.green;
            if (IsGrounded())
                Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, Vector3.down * GroundCheckDistance);
            Gizmos.DrawWireCube(transform.position + Vector3.down * GroundCheckDistance, GroundCheckBoxSize);
        }

        private void FixedUpdate()
        {
            Grounded = IsGrounded();
            float drag = Grounded ? Configuration.GroundDrag : Configuration.AirDrag;

            ApplyGravity();
            ApplyDrag(drag);

            _characterController.Move(Velocity * Time.fixedDeltaTime);
        }

        private void Update()
        {
            if (_characterControllerDisableTimer > 0)
            {
                _characterController.enabled = false;
                _characterControllerDisableTimer -= Time.deltaTime;
                if (_characterControllerDisableTimer <= 0)
                {
                    _characterController.enabled = true;
                }
            }
        }

        public void DisableCharacterController(float time)
        {
            _characterController.enabled = false;
            _characterControllerDisableTimer = time;
        }

        public void EnableCharacterController()
        {
            _characterController.enabled = true;
        }

        public void Move(float speed, Vector3 directionNormalized, float acceleration)
        {
            if (directionNormalized == Vector3.zero)
                return;

            var flatVelocity = Velocity.XZ();

            if (flatVelocity.magnitude < speed)
                flatVelocity += directionNormalized * acceleration * Time.deltaTime;
            else
                flatVelocity += -flatVelocity.normalized * acceleration * Time.deltaTime;

            if (Mathf.Abs(flatVelocity.magnitude - speed) < 0.1f)
                flatVelocity = directionNormalized * speed;

            flatVelocity.y = Velocity.y;

            Velocity = flatVelocity;
        }

        public void Jump(float jumpHeight)
        {
            Velocity.y = Mathf.Sqrt(jumpHeight * -2 * Configuration.Gravity);
        }

        public void Jump(float jumpHeight, float forwardForce, Vector3 directionNormalized)
        {
            Velocity = directionNormalized * forwardForce;
            Jump(jumpHeight);
        }

        public void AlignToVector(Vector3 vector, float speed)
        {
            if (vector == Vector3.zero) return;
            float targetAngle = Mathf.Atan2(vector.x, vector.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _rotationSmoothDampVelocity, speed);
            if (speed != 0)
            {
                transform.rotation = Quaternion.Euler(0, angle, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, targetAngle, 0);
            }
        }

        private void ApplyGravity()
        {
            Velocity.y += Configuration.Gravity * Time.fixedDeltaTime;
            if (Velocity.y < 0 && Grounded)
            {
                Velocity.y = -1;
            }
        }

        private void ApplyDrag(float drag)
        {
            var velocity = Velocity;
            velocity = Vector3.MoveTowards(velocity, Vector3.zero, drag * Time.deltaTime);

            Velocity.x = velocity.x;
            Velocity.z = velocity.z;
        }
        public void SimpleMove(Vector3 motion)
        {
            _characterController.Move(motion);
        }

        public void SetPosition(Vector3 newPosition)
        {
            _characterController.Move(newPosition - transform.position);
        }

        public void SetPositionViaTransform(Vector3 newPosition)
        {
            _characterController.enabled = false;
            transform.position = newPosition;
            _characterController.enabled = true;
        }

        public void AlignRotationInstant(GameObject targetRotation)
        {
            transform.forward = targetRotation.transform.forward;
        }

        public void SetPositionViaTransform(GameObject newPosition)
        {
            _characterController.enabled = false;
            transform.position = newPosition.transform.position;
            _characterController.enabled = true;
        }

        private bool IsGrounded()
        {
            return Physics.BoxCast(
                transform.position,
                GroundCheckBoxSize * 0.5f,
                Vector3.down,
                Quaternion.identity,
                GroundCheckDistance,
                GroundLayer
                );
        }
    }
}
