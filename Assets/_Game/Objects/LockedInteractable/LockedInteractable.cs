using MP.Game.Objects.Interactable;
using MP.Game.Objects.Player.Scripts.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MP.Game.Objects.LockedInteractable
{
    public class LockedInteractable : Interactable.Interactable
    {
        public InventoryItem RequiredItem;

        public override void Interact(GameObject sender)
        {
            if (PlayerInventory.Instance.HasItem(RequiredItem))
            {
                PlayerInventory.Instance.UseNormalItem(RequiredItem);
                base.Interact(sender);
            }
        }
    }
}
