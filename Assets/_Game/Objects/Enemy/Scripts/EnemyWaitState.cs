using MP.Game.Utils;
using UnityEngine;

namespace MP.Game.Objects.Enemy.Scripts
{
    public class EnemyWaitState : EnemyState
    {
        [SerializeField] protected MinMax WaitTime;
        [SerializeField] private EnemyApproachToAttackState _approachState;
        [SerializeField] private float _rotationSmoothness;

        private float _timer;
        private Vector3 _direction;

        public override void OnEnter()
        {
            _timer = WaitTime.GetRandom();
        }

        protected override void OnStateUpdate()
        {
            _direction = Enemy.DirectionToPlayer();
            Enemy.AlignToVector(_direction, _rotationSmoothness);
            _timer -= Time.deltaTime;
            if (_timer <= 0 && Enemy.CombatState.CanBeAttacked)
            {
                StateMachine.ChangeState(_approachState);
            }
        }
    }
}
