//using Cinemachine;
using MP.Game.Objects.Player.Scripts.Inventory;
using MP.Game.Objects.Player.Scripts.StateMachine;
using MP.Game.Utils.Extensions;
using UnityEngine;

namespace MP.Game.Objects.Player.Scripts
{

    [RequireComponent(typeof(Player))]
    public class PlayerAnimator : MonoBehaviour
    {
        [field: SerializeField] public Animator Animator { get; private set; }
        [SerializeField] private float _movementInputCatchUpSpeed;

        private RuntimeAnimatorController _defaultAnimatorController;
        private PlayerInput _playerInput;
        private Character.Character _character;
        private PlayerWeapon _weapon;
        private Objects.StateMachine.StateMachine _stateMachine;
        private PlayerFightState _playerFightState;
        private Vector3 _movementDirectionAnimator = new Vector3(0, 0, 1);

        private int _movementDirectionXHash;
        private int _movementDirectionYHash;
        private int _jumpedTriggerHash;
        private int _fallingBooleanHash;
        private int _groundedBooleanHash;
        private int _moveSpeedHash;
        private int _rollBooleanHash;
        private int _attackIDHash;
        private int _isFightingBooleanHash;
        private int _runningAttackHash;
        private int _parryingHash;
        private int _parryingAttackHash;

        private void Awake()
        {
            _weapon = GetComponent<PlayerWeapon>();
            _stateMachine = GetComponent<Objects.StateMachine.StateMachine>();
            _playerFightState = _stateMachine.GetState<PlayerFightState>();
            _playerInput = GetComponent<PlayerInput>();
            _character = GetComponent<Character.Character>();
            _moveSpeedHash = Animator.StringToHash("MoveSpeed");
            _movementDirectionXHash = Animator.StringToHash("MoveDirectionX");
            _movementDirectionYHash = Animator.StringToHash("MoveDirectionY");
            _jumpedTriggerHash = Animator.StringToHash("Jumped");
            _fallingBooleanHash = Animator.StringToHash("Falling");
            _groundedBooleanHash = Animator.StringToHash("Grounded");
            _rollBooleanHash = Animator.StringToHash("Roll");
            _attackIDHash = Animator.StringToHash("AttackID");
            _isFightingBooleanHash = Animator.StringToHash("IsFighting");
            _runningAttackHash = Animator.StringToHash("RunningAttack");
            _parryingHash = Animator.StringToHash("Parry");
            _parryingAttackHash = Animator.StringToHash("ParryAttack");
            _defaultAnimatorController = Animator.runtimeAnimatorController;
        }

        private void OnEnable()
        {
            _stateMachine.StateChanged.AddListener(OnStateChanged);
            _playerFightState.RunningAttack += OnRunningAttack;
            _weapon.Equiped.AddListener(OnWeaponEquiped);
            _weapon.Unequiped.AddListener(OnWeaponUnequiped);
        }

        private void OnDisable()
        {
            _stateMachine.StateChanged.RemoveListener(OnStateChanged);
            _playerFightState.RunningAttack -= OnRunningAttack;
            _weapon.Equiped.RemoveListener(OnWeaponEquiped);
            _weapon.Unequiped.RemoveListener(OnWeaponUnequiped);
        }

        private void OnWeaponEquiped(WeaponItem weapon)
        {
            RuntimeAnimatorController animator = weapon.AnimatorOverrideController;
            if (weapon.AnimatorOverrideController == null)
                animator = _defaultAnimatorController;
            Animator.runtimeAnimatorController = animator;
        }
        private void OnWeaponUnequiped(WeaponItem weapon)
        {
            Animator.runtimeAnimatorController = _defaultAnimatorController;
        }

        private void OnRunningAttack()
        {
            Animator.SetTrigger(_runningAttackHash);
        }

        private void OnStateChanged(Objects.StateMachine.State state)
        {
            if (state is PlayerJumpState)
            {
                Animator.SetTrigger(_jumpedTriggerHash);
            }

            if (state is PlayerRollState)
            {
                Animator.SetBool(_rollBooleanHash, true);
            }
            if (state is not PlayerRollState)
            {
                Animator.SetBool(_rollBooleanHash, false);
            }

            if (state is PlayerFightState)
            {
                Animator.SetBool(_isFightingBooleanHash, true);
            }
            if (state is not PlayerFightState)
            {
                Animator.SetBool(_isFightingBooleanHash, false);
            }

            if (state is PlayerParryState)
            {
                Animator.SetTrigger(_parryingHash);
            }
        }


        private void Update()
        {
            Animator.SetInteger(_attackIDHash, _playerFightState.CurrentAttackID);
            Animator.SetBool(_groundedBooleanHash, _character.Grounded);

            var relativeInput = _playerInput.RelativeMovementInput;
            var localInput = transform.InverseTransformDirection(relativeInput);

            _movementDirectionAnimator = Vector3.Slerp(_movementDirectionAnimator, localInput, Time.deltaTime * _movementInputCatchUpSpeed);
            Animator.SetFloat(_movementDirectionXHash, _movementDirectionAnimator.x);
            Animator.SetFloat(_movementDirectionYHash, _movementDirectionAnimator.z);

            Animator.SetFloat(_moveSpeedHash, _character.Velocity.XZ().magnitude);

            if (_character.Velocity.y < 0 && !_character.Grounded)
                Animator.SetBool(_fallingBooleanHash, true);
            else
            {
                Animator.SetBool(_fallingBooleanHash, false);
            }
        }
    }
}
