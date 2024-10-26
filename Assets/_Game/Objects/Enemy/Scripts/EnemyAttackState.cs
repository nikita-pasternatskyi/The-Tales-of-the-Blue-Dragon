using UnityEngine;

namespace MP.Game.Objects.Enemy.Scripts
{
    public class EnemyAttackState : EnemyState
    {
        [SerializeField] private EnemyAttack _attack;
        [SerializeField] private EnemyApproachToWaitState _approachToWaitState;
        private Vector3 _targetRotation;
        private float _timer;
        private bool _hit;

        public override void OnEnter()
        {
            _targetRotation = Enemy.Player.transform.position - transform.position;
            _hit = false;

            Enemy.Agent.updateRotation = false;
            _timer = _attack.AttackDurationTime;
            Enemy.RaiseDanger(_attack);
        }

        public override void OnExit()
        {
            Enemy.ResetCombatState();
        }

        protected override void OnStateUpdate()
        {
            Enemy.AlignToVector(_targetRotation.normalized, 0.1f);
            _timer -= Time.deltaTime;
            if (_timer <= _attack.DangerWarningTime && _hit == false)
            {
                var difference = _attack.AttackDistance - Enemy.DistanceToPlayer();
                if (difference >= _attack.ExtraDodgeDistance)
                {
                    Hit();
                }
                _hit = true;
            }

            if (_timer <= 0)
            {
                Enemy.ResetCombatState();
                StateMachine.ChangeState(_approachToWaitState);
            }
        }

        public void Hit()
        {
            Enemy.Player.GetComponent<Health.Health>().TakeDamage(_attack.Damage);
        }
    }
}
