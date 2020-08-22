using System.Collections.Generic;
using SpaceGame.Core;
using UnityEngine.Assertions;

namespace SpaceGame.InventorySystem.Utils
{
    [System.Serializable]
    public class SerializableInventory
    {
        [System.Serializable]
        public class InventorySlot
        {
            public readonly string TypeName;
            public readonly uint Amount;
            public ItemType Type => ItemType.GetByName<ItemType>(TypeName);

            public InventorySlot(ItemType type, uint amount)
            {
                TypeName = type != null ? type.Name : null;
                Amount = amount;
            }
        }

        public readonly List<InventorySlot> InventorySlots = new List<InventorySlot>();

        public void RestoreInventory(IInventory inventory)
        {
            Assert.IsTrue(inventory.Count == InventorySlots.Count);
            inventory.Resize((uint) InventorySlots.Count);
            inventory.Clear();
            for (var i = 0; i < InventorySlots.Count; i++)
            {
                inventory[i].TryAdd(InventorySlots[i].Type, InventorySlots[i].Amount);
            }
        }

        public SerializableInventory(IInventory inventory)
        {
            foreach (var stack in inventory)
            {
                InventorySlots.Add(new InventorySlot(stack.Type, stack.Amount));
            }
        }
    }
}