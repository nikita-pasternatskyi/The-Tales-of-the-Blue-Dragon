using UnityEngine;

namespace MP.Game.Objects.Player.Scripts.Inventory
{
    [CreateAssetMenu(menuName = "MP/Inventory/Inventory Item")]
    public class InventoryItem : ScriptableObject
    {
        public Sprite Image;
        public Color Color = Color.white;
        public string Name;
        public string Description;
    }

}
