using MP.Game.Utils.Extensions;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace MP.Game.Objects.Player.Scripts.StateMachine
{
    public partial class PlayerFightState : PlayerState
    {
        public UnityEvent UnityRunningAttack;
        public UnityEvent UnityNormalAttack;

        [SerializeField] private PlayerMoveAndRotateAction _moveAndRotate;
        [SerializeField] private LayerMask _enemyLayer;

        [Header("Flying attack: attack when running")]
        [SerializeField] private float _runningAttackSpeedThreshold = 3.5f;
        private PlayerInput _playerInput;
        private PlayerWeapon _weapon;
        private Character.Character _character;
        private Vector3 _savedDirectionInput;
        private Enemy.Scripts.Enemy _targetEnemy;
        private float _cooldownTimer;
        private float _returnTimer;
        private float _hitTimer;
        private bool _hit;
        private PlayerAttack _currentAttack;

        public event Action RunningAttack;
        public event Action<int> NormalAttack;

        public int CurrentAttackID { get; private set; }

        public override void OnExit()
        {
            _weapon.UnequipWeapon();

            CurrentAttackID = -1;
            _savedDirectionInput = Vector3.zero;
            _cooldownTimer = _returnTimer = 0;
        }

        public override void OnEnter()
        {
            _weapon.EquipWeapon();
            if (_character.Velocity.XZ().magnitude >= _runningAttackSpeedThreshold && _playerInput.InputMap.Gameplay.Run.IsPressed())
            {
                SetAttackParameters(_weapon.CurrentWeaponItem.RunningAttack);
                RunningAttack?.Invoke();
                return;
            }
            UpdateAttack(0);
        }


        public override bool CanEnter()
        {
            return _weapon.HasWeapon();
        }

        public override void StateUpdate()
        {
            if (PlayerInput.InputMap.Gameplay.Parry.triggered)
                StateMachine.ChangeState<PlayerParryState>();

            _moveAndRotate.MoveAndRotate(_character, _savedDirectionInput);
            _cooldownTimer -= Time.deltaTime;
            _hitTimer -= Time.deltaTime;
            if(_hitTimer <= 0 && !_hit)//_targetEnemy && !_hit)
            {
                var hitEnemies = Physics.OverlapSphere(transform.position + transform.forward * _currentAttack.Distance, _currentAttack.Radius, _enemyLayer);
                foreach (var enemy in hitEnemies)
                {
                    var c = enemy.GetComponent<Enemy.Scripts.Enemy>();
                    if(c)
                    {
                        c.TakeDamage(_currentAttack.Damage);
                    }
                }
                //_targetEnemy.TakeDamage(_weapon.CurrentWeaponItem.Attacks[CurrentAttackID].Damage);
                _hit = true;
            }
            if (_cooldownTimer <= 0)
            {
                if (_playerInput.InputMap.Gameplay.SwordAttack.triggered)
                {
                    UpdateAttack(++CurrentAttackID);
                    return;
                }

                if (_playerInput.InputMap.Gameplay.Roll.triggered)
                {
                    StateMachine.ChangeState<PlayerRollState>();
                    return;
                }

                _returnTimer -= Time.deltaTime;
                if (_returnTimer <= 0)
                {
                    StateMachine.ChangeState<PlayerIdleState>();
                }
            }
        }

        protected override void StateInited()
        {
            _character = StateMachine.GetComponent<Character.Character>();
            _playerInput = StateMachine.GetComponent<PlayerInput>();
            _weapon = StateMachine.GetComponent<PlayerWeapon>();
            CurrentAttackID = -1;
            _weapon.UnequipWeapon();
        }

        private void UpdateAttack(int newIdx)
        {
            CurrentAttackID = Wrap(newIdx, 0, _weapon.CurrentWeaponItem.Attacks.Length - 1);
            SetAttackParameters(_weapon.CurrentWeaponItem.Attacks[CurrentAttackID]);
            NormalAttack?.Invoke(newIdx);
        }

        private void SetAttackParameters(PlayerAttack newAttack)
        {
            _currentAttack = newAttack;
            _hit = false;
            
            //if (Physics.SphereCast(transform.position, _enemyCheckRadius, PlayerInput.RelativeMovementInput, out RaycastHit hit, _enemyCheckDistance, _enemyLayer, QueryTriggerInteraction.UseGlobal))
            //{
            //    _targetEnemy = hit.collider.GetComponent<Enemy.Scripts.Enemy>();
            //}
            newAttack.Triggered?.Invoke();
            _savedDirectionInput = _playerInput.RelativeMovementInput;
            if (_playerInput.RelativeMovementInput == Vector3.zero)
                _savedDirectionInput = _character.transform.forward;
            _cooldownTimer = newAttack.Cooldown;
            _returnTimer = newAttack.ReturnDelay;
            _hitTimer = newAttack.HitDelay;
            if (newAttack.JumpForce != Vector2.zero)
                _character.Jump(newAttack.JumpForce.y, newAttack.JumpForce.x, _savedDirectionInput);
        }

        private int Wrap(int value, int min, int max)
        {
            if (value < min)
                return max - (min + 1 - value); //  -1..0..1 so that first step is 0, and not 1
            if (value > max)
                return min + (value - 1 - max); // 0..1..2 so that first step is 0, and not 1
            return value;
        }

        private void OnEnable()
        {
            RunningAttack += UnityRunningAttack.Invoke;
            NormalAttack += _ => UnityNormalAttack.Invoke();
        }

        private void OnDisable()
        {
            RunningAttack -= UnityRunningAttack.Invoke;
            NormalAttack -= _ => UnityNormalAttack.Invoke();
        }
    }
}
