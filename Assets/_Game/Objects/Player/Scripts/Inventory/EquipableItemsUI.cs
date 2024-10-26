using UnityEngine;

namespace MP.Game.Objects.Player.Scripts.Inventory
{
    public class EquipableItemsUI : MonoBehaviour 
    {
        [SerializeField] private EquipableItemSlot[] _slots;
        [SerializeField] private Transform _viewParent;
        [SerializeField] private EquipableItemSlotUI _prefab;

        private void Start()
        {
            foreach(var item in _slots)
            {
                var prefab = Instantiate(_prefab, _viewParent);
                prefab.SubscribeToSlot(item);
            }
        }
    }
}
