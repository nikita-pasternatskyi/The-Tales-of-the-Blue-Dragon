using MP.Game.Objects.Player.Scripts.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MP.Game.Objects.Player.Scripts.Inventory
{
    public class InventorySlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _preview;
        [SerializeField] private TextMeshProUGUI _itemCount;
        public InventoryItem CurrentItem { get; protected set; }

        public UnityEvent<InventorySlotUI> PointerEntered;
        public UnityEvent<InventorySlotUI> PointerExited;
        private Color _normalColor;

        private void Awake()
        {
            _normalColor = _preview.color;
        }

        public void UpdateUI(InventoryItem item)
        {
            if (item == null)
            {
                _preview.sprite = null;
                _itemCount.text = "0";
                CurrentItem = null;
                _preview.color = _normalColor;
                return;
            }
            _preview.color = item.Color;
            _preview.sprite = item.Image;
            _itemCount.text = PlayerInventory.Instance.GetItemCount(item).ToString();
            CurrentItem = item;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PointerEntered?.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PointerExited?.Invoke(this);
        }
    }
}
