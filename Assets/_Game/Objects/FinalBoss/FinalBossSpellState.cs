using MP.Game.Objects.Enemy.Scripts;
using UnityEngine;

namespace MP.Game.Objects.FinalBoss
{
    public class FinalBossSpellState : FinalBossState
    {
        [SerializeField] private float _waitTime;
        [SerializeField] private TeleportState _teleportState;
        private float _waitTimer;

        public override void OnEnter()
        {
            _waitTimer = _waitTime;
        }

        protected override void OnStateUpdate()
        {
            if(_waitTimer > 0)
            {
                _waitTimer -= Time.deltaTime;
                if(_waitTimer <= 0)
                {
                    StateMachine.ChangeState(_teleportState);
                }
            }
        }
    }
}
