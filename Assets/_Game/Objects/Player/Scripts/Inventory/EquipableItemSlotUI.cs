using MP.Game.Objects.UI;
using UnityEngine;

namespace MP.Game.Objects.Player.Scripts.Inventory
{
    public class EquipableItemSlotUI : InventorySlotUI
    {
        [SerializeField] private ButtonHint _buttonHint;
        private EquipableItemSlot _slotToFollow;

        private void OnEnable()
        {
            SubscribeToSlot(_slotToFollow);
            PlayerInventory.Instance.ItemAdded.AddListener(UpdateSlotUI);
            PlayerInventory.Instance.ItemUsed.AddListener(UpdateSlotUI);
            PlayerInventory.Instance.ItemRemoved.AddListener(ItemRemoved);
        }

        private void OnDisable()
        {
            UnsubscribeFromSlot(_slotToFollow);
            PlayerInventory.Instance.ItemAdded.RemoveListener(UpdateSlotUI);
            PlayerInventory.Instance.ItemUsed.RemoveListener(UpdateSlotUI);
            PlayerInventory.Instance.ItemRemoved.RemoveListener(ItemRemoved);
        }

        public void SubscribeToSlot(EquipableItemSlot slot)
        {
            _slotToFollow = slot;
            if (_slotToFollow != null)
            {
                _slotToFollow.ItemEquipped.AddListener(OnItemEquiped);
                _slotToFollow.ItemUnequipped.AddListener(OnItemUnequiped);
                _buttonHint.Button = _slotToFollow.ActivationButton;
            }

        }
        public void UnsubscribeFromSlot(EquipableItemSlot slot)
        {
            if (_slotToFollow != null)
            {
                _slotToFollow.ItemEquipped.RemoveListener(OnItemEquiped);
                _slotToFollow.ItemUnequipped.RemoveListener(OnItemUnequiped);
                _slotToFollow = slot;
            }
        }

        private void OnItemEquiped(InventoryItem item)
        {
            UpdateUI(item);
        }
        private void OnItemUnequiped()
        {
            UpdateUI(null);
        }


        private void UpdateSlotUI(InventoryItem item)
        {
            if (item == CurrentItem)
            {
                UpdateUI(item);
            }
        }

        private void ItemRemoved(InventoryItem item)
        {
            if (item == CurrentItem)
            {
                UpdateUI(null);
            }
        }
    }
}
