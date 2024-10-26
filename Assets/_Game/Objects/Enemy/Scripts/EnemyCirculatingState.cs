using UnityEngine;

namespace MP.Game.Objects.Enemy.Scripts
{
    public class EnemyCirculatingState : EnemyWaitState
    {
        [SerializeField] protected float Speed;

        public override void OnEnter()
        {
            base.OnEnter();
            Enemy.Agent.updateRotation = false;
            Enemy.Agent.isStopped = false;
        }

        public override void OnExit()
        {
            base.OnExit();
            Enemy.Agent.updateRotation = true;
            Enemy.Agent.isStopped = true;
        }

        protected override void OnStateUpdate()
        {
            base.OnStateUpdate();
            Enemy.Agent.SetDestination(transform.position + transform.right * Speed);
            var playerPosition = Enemy.Player.transform.position;
            playerPosition.y = transform.position.y; 
            Vector3 targetRotation = Enemy.Player.transform.position - transform.position;
            Enemy.AlignToVector(targetRotation, 0);
        }
    }
}
