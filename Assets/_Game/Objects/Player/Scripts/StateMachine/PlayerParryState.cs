using MP.Game.Objects.Enemy.Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace MP.Game.Objects.Player.Scripts.StateMachine
{
    public class PlayerParryState : PlayerState
    {
        private enum ParryStates
        {
            SuccesfullParry,
            UnSuccesfullParry,
            ParryAttack,
        }

        [Header("Parry")]
        [SerializeField] private PlayerDangerEvent _parryDanger;
        [SerializeField] private float _parryDuration = 0.5f;

        [Header("Parry Attack")]
        [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private float _timeBeforeCanAttack = 0.2f;
        [SerializeField] private PlayerAttack _parryThrustAttack;

        [Header("Events")]
        public UnityEvent SuccesfullParry;
        public UnityEvent UnSuccesfullParry;
        public UnityEvent ParryAttack;

        private ParryStates _currentState;
        private GameObject _enemyToParry;
        private float _timer;
        private float _waitingTimer;
        private Vector3 _facingDirection;

        private PlayerCombatState _combatState;
        private PlayerWeapon _weapon;

        protected override void StateInited()
        {
            _combatState = StateMachine.GetComponent<PlayerCombatState>();
            _weapon = StateMachine.GetComponent<PlayerWeapon>();
        }

        public override bool CanEnter()
        {
            return _enemyToParry != null && _weapon.HasWeapon();
        }

        public override void OnEnter()
        {
            _weapon.EquipWeapon();
            _currentState = ParryStates.SuccesfullParry;
            switch (_currentState)
            {
                case ParryStates.SuccesfullParry:
                    SuccesfullParry?.Invoke();
                    _enemyToParry.GetComponent<Health.Health>().TakeDamage(1f);
                    _enemyToParry.GetComponent<Enemy.Scripts.Enemy>().StunEnemy(_parryDuration);
                    _facingDirection = (_enemyToParry.transform.position - Character.transform.position).normalized;
                    break;
                case ParryStates.UnSuccesfullParry:
                    break;
            }

            Character.Velocity = Vector3.zero;
            _timer = _parryDuration;
        }

        public override void StateUpdate()
        {
            Character.AlignToVector(_facingDirection, 0.1f);
            switch (_currentState)
            {
                case ParryStates.SuccesfullParry:
                    if (_timer <= 0)
                    {
                        StateMachine.ChangeState<PlayerIdleState>();
                        break;
                    }
                    if (_timer <= _parryDuration - _timeBeforeCanAttack)
                    {
                        if (PlayerInput.InputMap.Gameplay.SwordAttack.triggered)
                        {
                            _timer = _parryThrustAttack.ReturnDelay;
                            ParryAttack?.Invoke();
                            _currentState = ParryStates.ParryAttack;
                            var hitEnemies = Physics.OverlapSphere(transform.position + transform.forward * _parryThrustAttack.Distance, _parryThrustAttack.Radius, _enemyLayer);
                            foreach (var enemy in hitEnemies)
                            {
                                var c = enemy.GetComponent<Enemy.Scripts.Enemy>();
                                if (c)
                                {
                                    c.TakeDamage(_parryThrustAttack.Damage);
                                }
                            }
                            //_weapon.SetDamage(_parryThrustAttack.Damage);
                            Character.Jump(_parryThrustAttack.JumpForce.y, _parryThrustAttack.JumpForce.x, transform.forward);
                            break;
                        }
                    }
                    break;

                case ParryStates.UnSuccesfullParry:
                    _timer -= Time.deltaTime;
                    if (_timer <= 0)
                    {
                        StateMachine.ChangeState<PlayerIdleState>();
                        break;
                    }
                    break;

                case ParryStates.ParryAttack:
                    _timer -= Time.deltaTime;
                    if (_timer <= 0)
                    {
                        StateMachine.ChangeState<PlayerIdleState>();
                    }
                    break;
            }
        }

        public override void OnExit()
        {
            _weapon.UnequipWeapon();
            _combatState.CanBeAttacked = true;
            ResetParry();
        }

        #region ParryDangerEvent

        private void OnEnable()
        {
            _parryDanger.GenericEvent += OnParryDanger;
        }

        private void OnDisable()
        {
            _parryDanger.GenericEvent -= OnParryDanger;
        }

        private void OnParryDanger(GameObject arg1, EnemyAttack arg2)
        {
            _timer = arg2.DangerWarningTime;
            _enemyToParry = arg1;
        }
        #endregion

        #region ResetParry

        private void Update()
        {
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0)
                    ResetParry();
            }
        }

        private void ResetParry()
        {
            _timer = 0;
            _enemyToParry = null;
        }
        #endregion
    }
}
