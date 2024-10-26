using UnityEngine;

namespace MP.Game.Objects.Enemy.Scripts
{
    public class EnemyStunState : EnemyState
    {
        [SerializeField] private EnemyApproachToWaitState _approachToWaitState;
        private float _timer;

        public override void OnEnter()
        {
            if (Enemy.IsTheAttackingEnemy())
                Enemy.ResetCombatState();
            _timer = Enemy.StunTime;
        }

        protected override void OnStateUpdate()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                StateMachine.ChangeState(_approachToWaitState);
            }
        }
    }
}
