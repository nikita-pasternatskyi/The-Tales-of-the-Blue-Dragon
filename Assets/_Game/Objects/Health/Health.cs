using UnityEngine;
using UnityEngine.Events;

namespace MP.Game.Objects.Health
{

    public class Health : MonoBehaviour
    {
        [field: SerializeField] public float MaxHealth { get; private set; }
        [SerializeField] private ChangeHealthEvent _changeHealthEvent;
        public bool IsInvincible;
        /// <summary>
        /// Fires when health was changed, the float is the Current Health
        /// </summary>
        public UnityEvent<float> HealthChangedAbsolute;

        /// <summary>
        /// Fires when health was changed, the float is the Current Health in Percentage relative to MaxHealth
        /// 1 = Max Health, 0 = No Health
        /// </summary>
        public UnityEvent<float> HealthChangedPercent;
        public UnityEvent TookDamage;
        public UnityEvent Healed;
        public UnityEvent Died;
        public float CurrentHealth { get; private set; }

        private void OnEnable()
        {
            if(_changeHealthEvent)
                _changeHealthEvent.GenericEvent += OnHealthChangeRequested;
        }
        private void OnDisable()
        {
            if (_changeHealthEvent)
                _changeHealthEvent.GenericEvent -= OnHealthChangeRequested;
        }

        public void TurnInvincible()
        {
            IsInvincible = true;
        }
        public void TurnNormal()
        {
            IsInvincible = false;
        }

        private void OnHealthChangeRequested(float amount, HealthAction action)
        {
            switch (action)
            {
                case HealthAction.Heal:
                    Heal(amount);
                    break;
                case HealthAction.Damage:
                    TakeDamage(amount);
                    break;
            }
        }

        private void Awake()
        {
            CurrentHealth = MaxHealth;
            NotifyHealthChange();
        }

        public void ChangeMaxHealth(float newMaxHealth, bool healToMax = false)
        {
            if (healToMax)
                Heal(newMaxHealth);
            MaxHealth = newMaxHealth;
            NotifyHealthChange();
        }

        public void TakeDamage(float damage)
        {
            if (IsInvincible || CurrentHealth == 0)
                return;
            CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, MaxHealth);
            if (CurrentHealth == 0)
            {
                Died?.Invoke();
            }
            TookDamage?.Invoke();
            NotifyHealthChange();
        }

        public void Heal(float amount)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);
            Healed?.Invoke();
            NotifyHealthChange();
        }

        private void NotifyHealthChange()
        {
            HealthChangedAbsolute?.Invoke(CurrentHealth);
            HealthChangedPercent?.Invoke(CurrentHealth / MaxHealth);
        }
    }
}
