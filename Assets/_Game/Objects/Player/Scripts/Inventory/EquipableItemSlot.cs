using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace MP.Game.Objects.Player.Scripts.Inventory
{
    public class EquipableItemSlot : SerializedMonoBehaviour
    {
        public HashSet<Type> AllowedTypes = new HashSet<Type>();
        [SerializeField] private InventoryEvent _equipItemEvent;
        [SerializeField] private InventoryEvent _unequipItemEvent;
        [field: SerializeField] public InputActionReference ActivationButton { get; private set; }
        public UnityEvent<InventoryItem> ItemEquipped; 
        public UnityEvent ItemUnequipped;
        protected InventoryItem item { get; private set; }
        public InventoryItem Item => item;

        private void Start()
        {
            PlayerQuickInventory.Instance.RegisterItemSlot(this);
        }

        public void EquipItem(InventoryItem item)
        {
            ItemEquipped?.Invoke(item);
            this.item = item;
            OnEquip(item);
        }

        public void UnequipItem()
        {
            this.item = null;
            ItemUnequipped?.Invoke();
            return;
        }

        private void OnEnable()
        {
            if(_equipItemEvent)
                _equipItemEvent.GenericEvent += OnItemEquipReguested;
            if(_unequipItemEvent)
                _unequipItemEvent.Event += OnItemUnequipReguested;
            SubscribeToInput();
        }

        private void OnDisable()
        {
            if (_equipItemEvent) 
                _equipItemEvent.GenericEvent -= OnItemEquipReguested;
            if (_unequipItemEvent) 
                _unequipItemEvent.Event -= OnItemUnequipReguested;
            UnsubscribeFromInput();
        }

        private void OnItemUnequipReguested()
        {
            UnequipItem();
        }

        private void OnItemEquipReguested(InventoryItem obj)
        {
            if (PlayerInventory.Instance.HasItem(obj))
            {
                EquipItem(obj);
            }
        }

        protected virtual void OnActivationButtonCanceled(InputAction.CallbackContext obj)
        {
        }

        protected virtual void OnActivationButtonPressed(InputAction.CallbackContext obj)
        {
            if (Game.Instance.Paused)
                return;
            if (item == null)
                return;
            if (item is IUsableItem)
            {
                PlayerInventory.Instance.TryUseItem(item as IUsableItem);
            }
        }

        protected void SubscribeToInput()
        {
            ActivationButton.action.performed += OnActivationButtonPressed;
            ActivationButton.action.canceled += OnActivationButtonCanceled;
        }
        protected void UnsubscribeFromInput()
        {
            ActivationButton.action.performed -= OnActivationButtonPressed;
            ActivationButton.action.canceled -= OnActivationButtonCanceled;
        }

        protected virtual void OnEquip(InventoryItem item)
        {

        }
    }
}
