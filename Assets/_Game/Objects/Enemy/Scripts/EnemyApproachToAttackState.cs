using UnityEngine;

namespace MP.Game.Objects.Enemy.Scripts
{
    public class EnemyApproachToAttackState : EnemyState
    {
        [SerializeField] private float _attackDistance;
        [SerializeField] private EnemyAttackState _attackState;

        public override void OnEnter()
        {
            Enemy.SetCombatState();
            Enemy.Agent.isStopped = false;
            Enemy.Agent.updateRotation = true;
        }

        public override void OnExit()
        {
            Enemy.Agent.isStopped = true;
        }

        protected override void OnStateUpdate()
        {
            Enemy.Agent.SetDestination(Enemy.Player.transform.position);
            if (Vector3.Distance(transform.position, Enemy.Player.transform.position) < _attackDistance)
            {
                StateMachine.ChangeState(_attackState);
            }
        }
    }
}
