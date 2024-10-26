using UnityEngine;
using UnityEngine.Events;

namespace MP.Game.Objects.Player.Scripts
{
    public class PlayerDangerEventListener : MonoBehaviour
    {
        [SerializeField] private PlayerDangerEvent _dangerEventToListenTo;
        public UnityEvent Event;
        public UnityEvent DangerEvaded;

        private float _timer;
        private float _dangerDistance;
        private Vector3 _attackPosition;

        private void OnEvent(GameObject arg1, Enemy.Scripts.EnemyAttack arg2)
        {
            Event?.Invoke();
            _attackPosition = arg1.transform.position;
            _timer = arg2.AttackDurationTime;
        }

        private void Update()
        {
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
                if (Vector3.Distance(transform.position, _attackPosition) >= _dangerDistance)
                {
                    _timer = 0;
                    DangerEvaded?.Invoke();
                }
                if (_timer <= 0)
                    DangerEvaded?.Invoke();
            }
        }

        private void OnEnable()
        {
            _dangerEventToListenTo.GenericEvent += OnEvent;
        }

        private void OnDisable()
        {
            _dangerEventToListenTo.GenericEvent -= OnEvent;
        }
    }
}
