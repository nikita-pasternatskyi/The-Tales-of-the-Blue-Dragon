using MP.Game.Utils;
using UnityEngine;

namespace MP.Game.Objects.Enemy.Scripts
{
    public class EnemyApproachToWaitState : EnemyState
    {
        [SerializeField] private RandomSelectFromArray<EnemyWaitState> _waitStates;

        public override void OnEnter()
        {
            Enemy.Agent.isStopped = false;
            Enemy.Agent.updateRotation = true;
            var direction = Enemy.Player.transform.position - transform.position;
            direction = -direction.normalized;
            Enemy.Agent.SetDestination(transform.position + direction * Enemy.TargetDistanceFromPlayer);
        }

        public override void OnExit()
        {
            Enemy.Agent.isStopped = true;
        }

        protected override void OnStateUpdate()
        {
            if (Enemy.Agent.remainingDistance <= 0.5f)
            {
                StateMachine.ChangeState(_waitStates.GetRandom());
            }
        }
    }
}
