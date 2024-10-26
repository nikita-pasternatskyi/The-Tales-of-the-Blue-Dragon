using MP.Game.Objects.Player.Scripts.Inventory;
using UnityEngine;
using UnityEngine.Events;

namespace MP.Game.Objects.Player.Scripts
{
    public class PlayerWeapon : MonoBehaviour
    {
        [SerializeField] private WeaponObject<Transform> _parents;
        [SerializeField] private EquipableItemSlot _weaponSlot;
        public WeaponItem CurrentWeaponItem => _currentWeapon.WeaponItem;
        public UnityEvent<WeaponItem> Equiped;
        public UnityEvent<WeaponItem> Unequiped;
        public bool HasWeapon() => CurrentWeaponItem != null;
        private WeaponObject<GameObject> _currentWeapon;

        [System.Serializable]
        private struct WeaponObject<T> where T : Object
        {
            public T Combat;
            public T Animated;
            [HideInInspector] public WeaponItem WeaponItem;

            public void Destroy()
            {
                GameObject.Destroy(Combat);
                GameObject.Destroy(Animated);
            }

            public void Create(T combatPrefab, Transform combatParent, T animatedPrefab, Transform animatedParent)
            {
                Combat = Instantiate(combatPrefab, combatParent);
                Animated = Instantiate(animatedPrefab, animatedParent);
            }

            public void EquipWeapon()
            {
                (Animated as GameObject).SetActive(false);
                (Combat as GameObject).SetActive(true);
            }
            public void UnequipWeapon()
            {
                (Animated as GameObject).SetActive(true);
                (Combat as GameObject).SetActive(false);
            }
        }

        private void OnEnable()
        {
            _weaponSlot.ItemEquipped.AddListener(WeaponChanged);
            _weaponSlot.ItemUnequipped.AddListener(WeaponUnequiped);
        }

        private void OnDisable()
        {
            _weaponSlot.ItemEquipped.RemoveListener(WeaponChanged);
            _weaponSlot.ItemUnequipped.RemoveListener(WeaponUnequiped);
        }

        public void EquipWeapon()
        {
            if (!HasWeapon())
                return;
            _currentWeapon.EquipWeapon();
            Equiped?.Invoke(_currentWeapon.WeaponItem);
        }

        public void UnequipWeapon()
        {
            if (!HasWeapon())
                return;
            _currentWeapon.UnequipWeapon();
            Unequiped?.Invoke(_currentWeapon.WeaponItem);
        }

        private void WeaponUnequiped()
        {
            _currentWeapon.Destroy();
        }

        private void WeaponChanged(InventoryItem item)
        {
            WeaponItem weaponItem = (WeaponItem)item;
            _currentWeapon.Destroy();
            _currentWeapon.WeaponItem = weaponItem;
            _currentWeapon.Create(weaponItem.CombatWeaponPrefab, _parents.Combat, weaponItem.AnimationWeaponModel, _parents.Animated);
            _currentWeapon.UnequipWeapon();
        }
    }
}
