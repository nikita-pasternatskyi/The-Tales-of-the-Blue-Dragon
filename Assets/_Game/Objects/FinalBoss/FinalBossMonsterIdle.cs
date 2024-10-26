using UnityEngine;

namespace MP.Game.Objects.FinalBoss
{
    public class FinalBossMonsterIdle : FinalBossState
    {
        [SerializeField] private float _waitTime;
        private bool _breathFire;
        private float _waitTimer;

        public override void OnEnter()
        {
            _waitTimer = 0;
        }

        protected override void OnStateUpdate()
        {
            _waitTimer += Time.deltaTime;
            if (_waitTimer >= _waitTime)
            {
                _breathFire = !_breathFire;
                if(_breathFire)
                {
                    StateMachine.ChangeState<FinalBossFireBreathState>();
                }
                else
                {
                    StateMachine.ChangeState<FinalBossJumpState>();
                }
            }
        }

        public void SwitchToIdle()
        {
            StateMachine.ChangeState<FinalBossMonsterIdle>();
        }
    }
}
