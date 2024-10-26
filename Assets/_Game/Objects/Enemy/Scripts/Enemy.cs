using MP.Game.Objects.Player.Scripts;
using MP.Game.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace MP.Game.Objects.Enemy.Scripts
{

    [RequireComponent(typeof(StateMachine.StateMachine))]
    [RequireComponent(typeof(Health.Health))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private ScriptableEvent _addEnemyInCombatEvent;
        [SerializeField] private ScriptableEvent _removeEnemyFromCombatEvent;
        [SerializeField] private LayerMask _playerLayer;
        [SerializeField] private float _playerExitDetectionRadius = 10;
        [SerializeField] private PlayerDangerEvent _danger;
        [SerializeField] private EnemyStunState _stunState;

        public float TargetDistanceFromPlayer;
        private StateMachine.StateMachine _stateMachine;
        public float StunTime { get; private set; }
        public NavMeshAgent Agent { get; private set; }
        public Player.Scripts.Player Player { get; private set; }
        public PlayerCombatState CombatState { get; private set; }
        private Health.Health _health;

        private float _rotationSmoothDampVelocity;

        private void Awake()
        {
            _health = GetComponent<Health.Health>();
            Agent = GetComponent<NavMeshAgent>();
            _stateMachine = GetComponent<StateMachine.StateMachine>();
        }

        private void Update()
        {
            if (Player != null)
            {
                if (DistanceToPlayer() > 30)
                {
                    RemoveEnemyFromCombat();
                    Player = null;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _playerExitDetectionRadius);
        }

        public void FoundPlayer(Player.Scripts.Player player)
        {
            Player = player;
            CombatState = player.GetComponent<PlayerCombatState>();
        }

        public bool PlayerRanAway()
        {
            if (Player == null)
                return true;
            if (Vector3.Distance(Player.transform.position, transform.position) > _playerExitDetectionRadius)
            {
                return true;
            }
            return false;
        }

        public bool IsTheAttackingEnemy()
        {
            if (CombatState == null)
                return false;
            return CombatState.AttackingEnemy;
        }

        public void TakeDamage(float dmg) => _health.TakeDamage(dmg);

        public void SetCombatState()
        {
            if (CombatState == null || Player == null)
                return;
            CombatState.AttackingEnemy = gameObject;
            CombatState.CanBeAttacked = false;
        }

        public void ResetCombatState()
        {
            if (CombatState == null || Player == null)
                return;
            CombatState.AttackingEnemy = null;
            CombatState.CanBeAttacked = true;
        }

        public Vector3 DirectionToPlayer()
        {
            return Player.transform.position - transform.position;
        }

        public float DistanceToPlayer()
        {
            return Vector3.Distance(transform.position, Player.transform.position);
        }

        public void AlignToVector(Vector3 vector, float smoothness)
        {
            if (vector == Vector3.zero) return;
            float targetAngle = Mathf.Atan2(vector.x, vector.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _rotationSmoothDampVelocity, smoothness);
            if (smoothness != 0)
            {
                transform.rotation = Quaternion.Euler(0, angle, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, targetAngle, 0);
            }
        }

        public void RaiseDanger(EnemyAttack attack)
        {
            _danger.Invoke(gameObject, attack);
        }

        private void OnDestroy()
        {
            RemoveEnemyFromCombat();
        }

        public void AddEnemyToCombat()
        {
            _addEnemyInCombatEvent?.Invoke();
        }

        public void RemoveEnemyFromCombat()
        {
            if (CombatState == null || Player == null)
                return;
            if (CombatState.AttackingEnemy == gameObject)
            {
                ResetCombatState();
            }
            _removeEnemyFromCombatEvent?.Invoke();
        }

        public void StunEnemy(float stunDuration)
        {
            StunTime = stunDuration;
            _stateMachine.ChangeState(_stunState);    
        }
    }
}
