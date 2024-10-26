using MP.Game.Objects.Health;
using UnityEngine;

namespace MP.Game.Objects.Player.Scripts.Inventory
{
    [CreateAssetMenu(menuName = "MP/Inventory/Weapon Item")]
    public class WeaponItem : InventoryItem
    {
        public GameObject AnimationWeaponModel;
        public GameObject CombatWeaponPrefab;
        public AnimatorOverrideController AnimatorOverrideController;
        public PlayerAttack[] Attacks;
        public PlayerAttack RunningAttack;
        public PlayerAttack ParryAttack;
    }
}
