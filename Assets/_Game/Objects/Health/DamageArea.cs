using System.Collections.Generic;
using UnityEngine;

namespace MP.Game.Objects.Health
{
    public class DamageArea : MonoBehaviour
    {
        public float Damage;
        public bool CanDamageOnlyOnce;

        private HashSet<Collider> _hitColliders = new HashSet<Collider>(4);

        private void OnTriggerEnter(Collider other)
        {
            if (!enabled)
                return;
            if (Damage == 0)
                return;
            if (CanDamageOnlyOnce)
            {
                if (_hitColliders.Contains(other))
                {
                    return;
                }
            }
            if (other.TryGetComponent(out Health health))
            {
                health.TakeDamage(Damage);
                _hitColliders.Add(other);
            }
        }

        public void ResetColliders() => _hitColliders.Clear();
    }
}
