using MP.Game.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace MP.Game.Objects.FinalBoss
{
    public class TeleportState : FinalBossState
    {
        [SerializeField] private RandomSelectFromArray<FinalBossSpellState> _spellStates;
        [SerializeField] private Transform[] _targetPositions;
        [SerializeField] private float _invisibilityTime;
        private float _timer = 0;
        private int _currentIdx = 0;

        [SerializeField] public UnityEvent BeginTeleport;

        public void Teleport()
        {
            BeginTeleport?.Invoke();
            _timer = _invisibilityTime;
        }

        protected override void OnStateUpdate()
        {
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
                if(_timer <= 0)
                {
                    transform.position = _targetPositions[_currentIdx].position;
                    transform.rotation = _targetPositions[_currentIdx].rotation;
                    if (++_currentIdx == _targetPositions.Length)
                        _currentIdx = 0;
                    StateMachine.ChangeState(_spellStates.GetRandom());
                    return;
                }
            }
        }
    }
}
