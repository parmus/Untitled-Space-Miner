using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame.InventorySystem {
    [Serializable]
    public class Inventory: IInventory
    {
        public event Action OnChange;
        public event Action OnResize;

        public Inventory(uint slots, uint stackMultiplier = 1) {
            _stackMultiplier = stackMultiplier;
            _content = new InventoryStack[slots];
            for(var i=0; i<slots; i++) {
                var stack = new InventoryStack(stackMultiplier);
                stack.OnChange += () => OnChange?.Invoke();
                _content[i] = stack;
            }
        }

        public void Resize(uint slots)
        {
            if (_content.Length == slots) return;
            Array.Resize(ref _content, (int) slots);
            for(var i=0; i<slots; i++) {
                if (_content[i] != null) continue;
                var stack = new InventoryStack(_stackMultiplier);
                stack.OnChange += () => OnChange?.Invoke();
                _content[i] = stack;
            }
            OnResize?.Invoke();
        }

        public uint Add(ItemType itemType, uint amount = 1) {
            var remaining = amount;
            foreach(var slot in _content)
            {
                if (slot.Type != itemType && slot.Type != null) continue;
                slot.Type = itemType;
                remaining -= slot.TryAdd(remaining);
                if (remaining == 0) break;
            }
            return amount - remaining;
        }

        public void Move(IInventoryStack stack, bool clearSource = true) {
            var moved = Add(stack.Type, stack.Amount);
            if (clearSource) {
                stack.Clear();
            } else {
                stack.TryRemove(moved);
            }
        }

        public bool Remove(ItemType itemType, uint amount = 1) {
            if (!Has(itemType, amount)) return false;

            var remaining = amount;
            for(var i = _content.Length-1; i>=0; i--) {
                var slot = _content[i];
                if (slot.Type != itemType) continue;
                remaining -= slot.TryRemove(remaining);
                if (slot.Amount == 0) slot.Clear();
                if (remaining == 0) break;
            }
            return true;
        }

        public bool RemoveFrom(int index, uint amount = 1) {
            if (_content[index].Amount < amount) return false;
            _content[index].TryRemove(amount);
            return true;
        }

        public void ClearFrom(int index) => _content[index].Clear();

        public uint Contains(ItemType itemType) {
            uint found = 0;
            foreach(var slot in _content) {
                if (slot.Type == itemType) found += slot.Amount;
            }
            return found;
        }

        public bool Has(ItemType itemType, uint amount = 1) {
            uint found = 0;
            foreach(var slot in _content) {
                if (slot.Type == itemType) found += slot.Amount;
                if (found >= amount) return true;
            }
            return false;
        }

        public bool CanAdd(ItemType itemType, uint amount = 1) {
            uint remaining = 0;
            foreach(var slot in _content) {
                if (slot.Type != null && slot.Type != itemType) continue;
                var freeInStack = slot.Type == null ? slot.Free : itemType.CanStack ? itemType.StackSize * _stackMultiplier : 1;
                if (remaining <= freeInStack) return true;
                remaining -= freeInStack;
            }
            return false;
        }

        public void Clear() => Array.ForEach(_content, slot => slot.Clear());

        [SerializeField] private InventoryStack[] _content;
        [SerializeField] private uint _stackMultiplier;


        //========================================================
        // IReadOnlyList<IInventoryStack>
        //========================================================
        int IReadOnlyCollection<IInventoryStack>.Count => _content.Length;
        IInventoryStack IReadOnlyList<IInventoryStack>.this[int i] => _content[i];
        IEnumerator IEnumerable.GetEnumerator() => _content.GetEnumerator();
        IEnumerator<IInventoryStack> IEnumerable<IInventoryStack>.GetEnumerator() => ((IReadOnlyCollection<IInventoryStack>) _content).GetEnumerator();
    }
}