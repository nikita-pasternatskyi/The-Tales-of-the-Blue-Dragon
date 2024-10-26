using MP.Game.Utils;
using UnityEngine;

namespace MP.Game.Objects.Player.Scripts.Inventory
{
    [CreateAssetMenu(menuName ="MP/Events/Inventory Event")]    

    public class InventoryEvent : GenericScriptableEvent<InventoryItem>
    { }
}
