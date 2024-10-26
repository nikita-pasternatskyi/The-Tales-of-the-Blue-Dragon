using MP.Game.Objects.StateMachine;
using UnityEngine;

namespace MP.Game.Objects.Enemy.Scripts
{
    public abstract class EnemyState : State
    {
        [SerializeField] private bool _playerCanRunAway;
        protected Enemy Enemy { get; private set; }
        protected sealed override void OnInit(StateMachine.StateMachine stateMachine)
        {
            Enemy = StateMachine.GetComponent<Enemy>();
        }
        protected virtual void Initted(StateMachine.StateMachine stateMachine) { }
        public sealed override void StateUpdate()
        {
            if (Enemy.PlayerRanAway() && _playerCanRunAway)
                StateMachine.ChangeState<EnemyNormalState>();
            OnStateUpdate();
        }

        protected virtual void OnStateUpdate() { }
    }
}
