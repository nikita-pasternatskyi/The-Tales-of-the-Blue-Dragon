using MP.Game.Utils;
using UnityEngine;

namespace MP.Game.Objects.Enemy.Scripts
{

    public class EnemyNormalState : EnemyState
    {
        [SerializeField] private EnemyApproachToWaitState _enemyApproachToWaitState;
        [SerializeField] protected LayerMask _playerLayer;
        [SerializeField] protected float _playerEntranceDetectionRadius = 5;
        [SerializeField] protected float _closePlayerRadius = 5;
        [SerializeField] protected float _visionRange = 45;
        [SerializeField] protected MinMax _targetDistanceAwayFromPlayer;

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, _playerEntranceDetectionRadius);
            Gizmos.DrawWireSphere(transform.position, _closePlayerRadius);
        }

        protected override void OnStateUpdate()
        {
            var colliders = Physics.OverlapSphere(transform.position, _playerEntranceDetectionRadius, _playerLayer);
            if (colliders == null || colliders.Length == 0)
                return;
            Player.Scripts.Player player = colliders[0].GetComponent<Player.Scripts.Player>();
            if (player == null)
                return;
            var direction = player.transform.position - transform.position;
            var playerClose = Vector3.Distance(transform.position, player.transform.position) < _closePlayerRadius;
            if (playerClose || Vector3.Dot(direction.normalized, transform.forward) > Mathf.Cos(Mathf.Deg2Rad * _visionRange))
            {
                Enemy.FoundPlayer(player);
                Enemy.AddEnemyToCombat();
                StateMachine.ChangeState(_enemyApproachToWaitState);
                Enemy.TargetDistanceFromPlayer = _targetDistanceAwayFromPlayer.GetRandom();

            }
        }
    }
}
