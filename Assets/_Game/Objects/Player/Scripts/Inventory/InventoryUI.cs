using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MP.Game.Objects.Player.Scripts.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private PlayerInventory _inventory;
        [SerializeField] private RectTransform _inventorySlotsParent;
        [SerializeField] private InventorySlotUI _inventorySlotPrefab;

        [Header("Preview")]
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private Image _previewImage;
        [SerializeField] private Sprite _defaultImage;
        [SerializeField] private Color _defaultColor;

        private Dictionary<InventoryItem, InventorySlotUI> _itemSlots = new Dictionary<InventoryItem, InventorySlotUI>();

        private void OnEnable()
        {
            _inventory.ItemAdded.AddListener(AddedItem);
            _inventory.ItemRemoved.AddListener(RemovedItem);
            _inventory.ItemUsed.AddListener(ItemUsed);
        }

        private void OnDisable()
        {
            _inventory.ItemAdded.RemoveListener(AddedItem);
            _inventory.ItemRemoved.RemoveListener(RemovedItem);
            _inventory.ItemUsed.RemoveListener(ItemUsed);
        }

        private void ItemUsed(InventoryItem item)
        {
            _itemSlots[item].UpdateUI(item);
        }

        private void AddedItem(InventoryItem item)
        {
            if(_itemSlots.ContainsKey(item))
            {
                _itemSlots[item].UpdateUI(item);
                return;
            }
            var itemUI = Instantiate(_inventorySlotPrefab, _inventorySlotsParent);
            itemUI.PointerEntered.AddListener(SlotSelected);
            itemUI.PointerExited.AddListener(SlotDeselected);
            itemUI.UpdateUI(item);
            _itemSlots.Add(item, itemUI);
        }

        private void SlotSelected(InventorySlotUI item)
        {
            PlayerInventory.Instance.ChangeSelection(item.CurrentItem);
            _previewImage.color = item.CurrentItem.Color;
            _previewImage.sprite = item.CurrentItem.Image;
            _nameText.text = item.CurrentItem.Name;
            _descriptionText.text = item.CurrentItem.Description;
        }
        private void SlotDeselected(InventorySlotUI item)
        {
            _previewImage.sprite = _defaultImage;
            _previewImage.color = _defaultColor;
            _nameText.text = string.Empty;
            _descriptionText.text = string.Empty;
        }

        private void RemovedItem(InventoryItem item) 
        {
            _itemSlots[item].PointerEntered.RemoveListener(SlotSelected);
            _itemSlots[item].PointerExited.RemoveListener(SlotDeselected);
            Destroy(_itemSlots[item].gameObject);
            _itemSlots.Remove(item);
        }
    }
}
