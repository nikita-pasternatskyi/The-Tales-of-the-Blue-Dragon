using MP.Game.Objects.Dialogue;
using MP.Game.Objects.Player.Scripts.StateMachine;
using MP.Game.Utils;
using UnityEngine;

namespace MP.Game.Objects.Player.Scripts
{
    [RequireComponent(typeof(PlayerCombatState))]

    public class PlayerCamera : MonoBehaviour
    {
        [System.Serializable]
        private struct CameraState
        {
            public float TargetFollowSpeed;
            public int ActivePriority;
            public Cinemachine.CinemachineVirtualCamera Camera;
        }
        [SerializeField] private RemoteTransform _followTarget;
        [Range(0, 45)] public float VerticalCameraRange = 45;

        [Header("States")]
        [SerializeField] private CameraState _normalCamera;
        [SerializeField] private CameraState _runningCamera;
        [SerializeField] private CameraState _combatCamera;
        [SerializeField] private CameraState _dialogueCamera;
        [SerializeField] private CameraState _horseCamera;
        [SerializeField] private float _waitUntilReturnFromCombat;


        private PlayerInput _playerInput;
        private Objects.StateMachine.StateMachine _stateMachine;
        private PlayerCombatState _playerCombatState;

        private void Awake()
        {
            _stateMachine = GetComponent<Objects.StateMachine.StateMachine>();
            _playerInput = GetComponent<PlayerInput>();
            _playerCombatState = GetComponent<PlayerCombatState>();
        }

        private void OnEnable()
        {
            var runState = _stateMachine.GetState<PlayerRunState>();
            runState.Entered.AddListener(OnRunStateEntered);
            runState.Exited.AddListener(OnRunStateExited);
            _playerCombatState.CombatBegin.AddListener(OnCombatBegin);
            _playerCombatState.CombatEnd.AddListener(OnCombatEnd);
        }

        private void OnDisable()
        {
            var runState = _stateMachine.GetState<PlayerRunState>();
            runState.Entered.RemoveListener(OnRunStateEntered);
            runState.Exited.RemoveListener(OnRunStateExited);

            _playerCombatState.CombatBegin.RemoveListener(OnCombatBegin);
            _playerCombatState.CombatEnd.RemoveListener(OnCombatEnd);

        }

        private void OnDialogueExited()
        {
            Debug.Log("hello");
            DeactivateCamera(_dialogueCamera);
            ActivateCamera(_normalCamera);
        }

        private void OnDialogueEntered()
        {
            ActivateCamera(_dialogueCamera);
        }

        private void Update()
        {
            var lookInput = _playerInput.InputMap.Gameplay.Look.ReadValue<Vector2>();

            var quaternion = _followTarget.transform.rotation;
            quaternion *= Quaternion.AngleAxis(lookInput.x * Time.deltaTime, Vector3.up);
            quaternion *= Quaternion.AngleAxis(-lookInput.y * Time.deltaTime, Vector3.right);

            quaternion = Quaternion.Euler(quaternion.eulerAngles.x, quaternion.eulerAngles.y, 0);

            //clamp up/down rotation
            var limitInRadians = Mathf.Deg2Rad * VerticalCameraRange;
            quaternion.x = Mathf.Clamp(quaternion.x, -limitInRadians, limitInRadians);
            _followTarget.transform.rotation = quaternion;
        }

        private void OnHorseMounted()
        {
            ActivateCamera(_horseCamera);
        }
        private void OnHorseDismounted()
        {
            DeactivateCamera(_horseCamera);
        }

        private void OnCombatBegin()
        {
            Debug.Log("combat begin");
            ActivateCamera(_combatCamera);
        }

        private void OnCombatEnd()
        {
            Invoke(nameof(CheckAndDeactivateCombatCamera), _waitUntilReturnFromCombat);
        }


        private void OnRunStateEntered()
        {
            ActivateCamera(_runningCamera);
        }

        private void OnRunStateExited()
        {
            DeactivateCamera(_runningCamera);
        }

        private void ActivateCamera(CameraState state)
        {
            _followTarget.PositionCopyMultiplier = state.TargetFollowSpeed;
            state.Camera.Priority = state.ActivePriority;
        }

        private void DeactivateCamera(CameraState state)
        {
            state.Camera.Priority = 0;
        }

        private void CheckAndDeactivateCombatCamera()
        {
            if (_playerCombatState.InCombat)
                return;
            DeactivateCamera(_combatCamera);
        }
    }
}
