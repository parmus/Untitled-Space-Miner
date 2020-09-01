using System;
using System.Collections.Generic;
using SpaceGame.Core;

namespace SpaceGame.InventorySystem
{
    public interface IInventory: IReadOnlyList<IInventoryStack>
    {
        event Action OnChange;
        event Action OnResize;

        void Resize(uint slots, uint stackMultiplier);
        void Resize(uint slots);

        uint Add(ItemType itemType, uint amount = 1);

        bool Remove(ItemType itemType, uint amount = 1);
        bool RemoveFrom(int index, uint amount = 1);
        void ClearFrom(int index);

        void Move(IInventoryStack stack, bool clearSource=true);


        uint Contains(ItemType itemType);

        bool Has(ItemType itemType, uint amount = 1);

        bool CanAdd(ItemType itemType, uint amount = 1);

        void Clear();
    }
}