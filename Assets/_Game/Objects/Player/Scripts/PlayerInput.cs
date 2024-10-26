using MP.Game.Settings.Input;
using UnityEngine;

namespace MP.Game.Objects.Player.Scripts
{

    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        private UnityEngine.InputSystem.PlayerInput _playerInput;

        public InputMap InputMap { get; private set; }
        public Vector3 RelativeMovementInput { get; private set; }
        public Vector2 AbsoluteMovementInput { get; private set; }

        private void Awake()
        {
            _playerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        }

        private void OnEnable()
        {
            InputMap ??= new InputMap();
            InputMap.Enable();
        }

        private void OnDisable()
        {
            InputMap.Disable();
        }

        private void Update()
        {
            AbsoluteMovementInput = InputMap.Gameplay.Move.ReadValue<Vector2>();

            var forward = _camera.transform.forward;
            var right = _camera.transform.right;

            forward.y = right.y = 0;

            forward.Normalize();
            right.Normalize();

            RelativeMovementInput = forward * AbsoluteMovementInput.y + right * AbsoluteMovementInput.x;
            RelativeMovementInput.Normalize();
        }
    }
}
