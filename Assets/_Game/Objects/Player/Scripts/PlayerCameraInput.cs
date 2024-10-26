using UnityEngine;

namespace MP.Game.Objects.Player.Scripts
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerCameraInput : MonoBehaviour
    {
        [SerializeField] private Transform _lookTarget;

        [Range(0, 45)] public float VerticalCameraRange = 45;

        private PlayerInput _playerInput;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
        }

        private void Update()
        {
            var lookInput = _playerInput.InputMap.Gameplay.Look.ReadValue<Vector2>();

            var quaternion = _lookTarget.rotation;
            quaternion *= Quaternion.AngleAxis(lookInput.x * Time.deltaTime, Vector3.up);
            quaternion *= Quaternion.AngleAxis(-lookInput.y * Time.deltaTime, Vector3.right);

            quaternion = Quaternion.Euler(quaternion.eulerAngles.x, quaternion.eulerAngles.y, 0);

            //clamp up/down rotation
            var limitInRadians = Mathf.Deg2Rad * VerticalCameraRange;
            quaternion.x = Mathf.Clamp(quaternion.x, -limitInRadians, limitInRadians);
            _lookTarget.rotation = quaternion;
        }
    }
}
