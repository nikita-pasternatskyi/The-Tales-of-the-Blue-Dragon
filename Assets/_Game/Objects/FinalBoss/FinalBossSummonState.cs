using MP.Game.Utils;
using UnityEngine;

namespace MP.Game.Objects.FinalBoss
{
    public class FinalBossSummonState : FinalBossSpellState
    {
        [SerializeField] private Enemy.Scripts.Enemy _enemyPrefab;
        [SerializeField] private int _amount;
        [SerializeField] private RandomSelectFromArray<Transform> _spawnPositions;
        private float _waitTimer;

        public override void OnEnter()
        {
            base.OnEnter();
            for (int i = 0; i < _amount; i++)
            {
                Transform parent = _spawnPositions.GetRandom();
                var instance = Instantiate(_enemyPrefab, parent.position, parent.rotation, parent);
            }
        }
    }
}
