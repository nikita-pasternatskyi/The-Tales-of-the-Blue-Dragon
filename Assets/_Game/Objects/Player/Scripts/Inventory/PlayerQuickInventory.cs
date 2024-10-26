using MP.Game.Assets.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace MP.Game.Objects.Player.Scripts.Inventory
{
    public class PlayerQuickInventory : UnitySingleton<PlayerQuickInventory>
    {
        [SerializeField] private InventoryEvent _removedItemEvent;
        private Dictionary<InventoryItem, EquipableItemSlot> _equipedSlots = new Dictionary<InventoryItem, EquipableItemSlot>();
        private List<EquipableItemSlot> _slots = new List<EquipableItemSlot>();

        public void RegisterItemSlot(EquipableItemSlot slot)
        {
            _slots.Add(slot);
        }

        private void OnEnable()
        {
            _removedItemEvent.GenericEvent += OnItemRemoved;
        }

        private void OnDisable()
        {
            _removedItemEvent.GenericEvent -= OnItemRemoved;
        }

        private void OnItemRemoved(InventoryItem item)
        {
            if(_equipedSlots.TryGetValue(item, out EquipableItemSlot slot))
            {
                slot.UnequipItem();
                _equipedSlots.Remove(item);
            }
        }

        private void Update()
        {
            if (PlayerInventory.Instance.SelectedItem == null)
                return;
            if (PlayerInventory.Instance.SelectedItem is not InventoryItem)
                return;
            var item = PlayerInventory.Instance.SelectedItem;
            if (PlayerInventory.Instance.GetItemCount(item) == 0)
                return;
            foreach (var slot in _slots)
            {
                var input = slot.ActivationButton;
                if (input.action.WasPressedThisFrame())
                {
                    if (slot.AllowedTypes.Contains(item.GetType()))
                    {
                        if (_equipedSlots.ContainsKey(item))
                        {
                            _equipedSlots[item].UnequipItem();
                            _equipedSlots[item] = slot;
                        }
                        else
                        {
                            _equipedSlots.Add(item, slot);
                        }
                        slot.EquipItem(item);
                    }
                    continue;
                }
            }
        }
    }
}
