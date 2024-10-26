using MP.Game.Assets.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace MP.Game.Objects.Player.Scripts.Inventory
{

    public class PlayerInventory : UnitySingleton<PlayerInventory>
    {
        [SerializeField] private InventoryEvent _addItemEvent;
        [SerializeField] private InventoryEvent _removeItemEvent;

        [SerializeField] private InventoryEvent _addedItemEvent;
        [SerializeField] private InventoryEvent _removedItemEvent;

        private Dictionary<InventoryItem, int> _inventorySlots = new Dictionary<InventoryItem, int>();

        public UnityEvent<InventoryItem> ItemAdded;
        public UnityEvent<InventoryItem> ItemRemoved;
        public UnityEvent<InventoryItem> ItemUsed;

        public InventoryItem SelectedItem { get; private set; }

        private void OnEnable()
        {
            _addItemEvent.GenericEvent += AddItem;
            _removeItemEvent.GenericEvent += RemoveItem;
        }

        public void ChangeSelection(InventoryItem item)
        {
            SelectedItem = item;
        }

        public void Deselect()
        {
            SelectedItem = null;
        }

        private void OnDisable()
        {
            _addItemEvent.GenericEvent -= AddItem;
            _removeItemEvent.GenericEvent -= RemoveItem;
        }

        public int GetItemCount(InventoryItem item) 
        {
            if(_inventorySlots.TryGetValue(item, out int count)) return count;
            return 0;
        }

        public bool HasItem(InventoryItem item)
        {
            return _inventorySlots.ContainsKey(item);
        }

        public bool TryUseItem(IUsableItem item)
        {
            var itemToUse = item as InventoryItem;
            if(_inventorySlots.TryGetValue(itemToUse, out var count))
            {
                count--;
                item.Use();
                _inventorySlots[itemToUse] = count;
                ItemUsed?.Invoke(itemToUse);
                if (count == 0)
                {
                    RemoveItem(itemToUse);
                }
                return true;
            }
            return false;
        }

        public void RemoveItem(InventoryItem obj)
        {
            if (_inventorySlots.TryGetValue(obj, out var count))
            {
                _inventorySlots.Remove(obj);
            }
            ItemRemoved?.Invoke(obj);
            _removedItemEvent?.Invoke(obj);
        }

        public void UseNormalItem(InventoryItem obj)
        {
            if (_inventorySlots.TryGetValue(obj, out var count))
            {
                count--;
                _inventorySlots[obj] = count;
                ItemUsed?.Invoke(obj);
                if (count == 0)
                {
                    RemoveItem(obj);
                }
            }
        }

        public void AddItem(InventoryItem obj)
        {
            if (_inventorySlots.TryGetValue(obj, out var count))
            {
                count++;
                _inventorySlots[obj] = count;
            }
            else
            {
                _inventorySlots.Add(obj, 1);
            }
            _addedItemEvent?.Invoke(obj);
            ItemAdded.Invoke(obj);
        }

    }
}
