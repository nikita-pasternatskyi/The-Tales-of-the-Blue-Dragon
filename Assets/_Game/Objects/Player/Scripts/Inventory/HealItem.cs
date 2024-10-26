using MP.Game.Objects.Health;
using UnityEngine;

namespace MP.Game.Objects.Player.Scripts.Inventory
{
    [CreateAssetMenu(menuName = "MP/Inventory/Heal Item")]
    public class HealItem : InventoryItem, IUsableItem
    {
        [SerializeField] private ChangeHealthEvent _changeHealthEvent;
        [SerializeField] private float _healthToHeal;

        public void Use()
        {
            _changeHealthEvent.Invoke(_healthToHeal, HealthAction.Heal);
        }
    }

}
