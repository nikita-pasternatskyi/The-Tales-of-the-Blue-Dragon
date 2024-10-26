using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace MP.Game.Objects.Enemy.Scripts
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Wave[] _waves;
        [SerializeField] private bool _spawnOnAwake;
        private bool _started;
        private int _currentWave = 0;
        private Coroutine _spawner;
        private int _deathCounter;
        public UnityEvent Completed;
        public UnityEvent Began;
        [System.Serializable]
        struct Wave
        {
            public EnemyAmount[] Enemies;
            public float SpawnDelay;
            public int NeededForNextWave;
        }

        [System.Serializable]
        struct EnemyAmount
        {
            public Enemy EnemyPrefab;
            public int Amount;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            foreach(var tr in transform.GetComponentsInChildren<Transform>())
            {
                Gizmos.DrawWireSphere(tr.position, 3f);
            }
        }

        private void Awake()
        {
            if (_spawnOnAwake)
                Begin();
        }

        private void Update()
        {
            if (_started)
            {
                if (_deathCounter >= _waves[_currentWave].NeededForNextWave)
                {
                    if (_currentWave >= _waves.Length - 1)
                    {
                        Completed?.Invoke();
                        _started = false;
                        return;
                    }
                    StartNewWave(++_currentWave);
                }
            }
        }

        private IEnumerator StartNewWave(int wave)
        {
            _deathCounter = 0;
            var curWave = _waves[wave];
            for (int i = 0; i < curWave.Enemies.Length; i++)
            {
                EnemyAmount enemy = curWave.Enemies[i];
                for(int j = 0; j <= enemy.Amount; j++)
                {
                    int child = j % (transform.childCount-1);
                    Transform parent = transform.GetChild(child);
                    var instance = Instantiate(enemy.EnemyPrefab, parent.position, parent.rotation, parent);
                    instance.GetComponent<Health.Health>().Died.AddListener(OnEnemyDied);
                    yield return new WaitForSeconds(curWave.SpawnDelay);
                }
            }
        }

        private void OnEnemyDied()
        {
            _deathCounter++;
        }

        public void Begin()
        {
            Began?.Invoke();
            _started = true;
            _currentWave = 0;
            if (_spawner != null)
                StopCoroutine(_spawner);
            _spawner = StartCoroutine(StartNewWave(_currentWave));
        }    
    }
}
