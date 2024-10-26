using MP.Game.Assets._Game.SceneManagement;
using MP.Game.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace MP.Game.Objects.Player.Scripts
{

    public class PlayerCombatState : MonoBehaviour
    {
        [SerializeField] private LoadSceneEvent _sceneLoadEvent;
        [SerializeField] private ScriptableEvent _addedEnemyToTheFight;
        [SerializeField] private ScriptableEvent _removedEnemyFromTheFight;

        public UnityEvent CombatBegin;
        public UnityEvent CombatEnd;

        public bool InCombat => _enemies > 0;
        private int _enemies;

        [HideInInspector] public bool CanBeAttacked = true;
        [HideInInspector] public GameObject AttackingEnemy;

        private void OnEnemyRemovedFromFight()
        {
            _enemies--;
            if (_enemies == 0)
                CombatEnd?.Invoke();
        }

        private void OnEnemyAdded()
        {
            if (_enemies == 0)
                CombatBegin?.Invoke();
            _enemies++;
        }

        public void ResetEnemyCount()
        {
            _enemies = 0;
            CombatEnd?.Invoke();
        }

        private void OnEnable()
        {
            _sceneLoadEvent.Event += ResetEnemyCount;
            _addedEnemyToTheFight.Event += OnEnemyAdded;
            _removedEnemyFromTheFight.Event += OnEnemyRemovedFromFight;
        }

        private void OnDisable()
        {
            _sceneLoadEvent.Event -= ResetEnemyCount;
            _addedEnemyToTheFight.Event -= OnEnemyAdded;
            _removedEnemyFromTheFight.Event -= OnEnemyRemovedFromFight;
        }
    }
}
