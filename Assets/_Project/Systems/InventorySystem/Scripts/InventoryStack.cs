using System;
using SpaceGame.Core;
using UnityEngine;

namespace SpaceGame.InventorySystem
{
    [Serializable]
    public class InventoryStack: IInventoryStack
    {
        [SerializeField] private ItemType _type = null;
        [SerializeField] private uint _amount = 0;
        
        public ItemType Type { get => _type; set => _type = value; }
        public uint Amount { get => _amount; private set => _amount = value; }

        public float PercentageFull => Type == null ? 0 : (float) Amount / StackSize;
        public uint Free => Type == null ? 0 : StackSize - Amount;
        public uint StackSize {
            get {
                if (Type == null) return 0;
                
                return Type.CanStack ? Type.StackSize * _stackMultiplier : 1;
            }
        }
        
        public uint StackMultiplier
        {
            get => _stackMultiplier;
            set
            {
                _stackMultiplier = value;
                if (Amount <= StackSize) return;
                Amount = StackSize;
                _onChangeCallback?.Invoke();
            }
        }

        private Action _onChangeCallback;
        private uint _stackMultiplier;

        public InventoryStack(uint stackMultiplier, Action onChangeCallback) {
            _stackMultiplier = stackMultiplier;
            _onChangeCallback = onChangeCallback;
        }

        public uint TryAdd(ItemType type, uint amount) {
            if (Type != null && Type != type) return 0;
            Type = type;
            return TryAdd(amount);
        }

        public uint TryAdd(uint amount) {
            var addedToStack = Math.Min(StackSize - Amount, amount);
            Amount += addedToStack;
            if (addedToStack > 0) _onChangeCallback?.Invoke();
            return addedToStack;
        }

        public uint TryRemove(uint amount) {
            var removedFromStack = Math.Min(Amount, amount);
            Amount -= removedFromStack;
            if (Amount == 0) Type = null;
            if (removedFromStack > 0) _onChangeCallback?.Invoke();
            return removedFromStack;
        }

        public void Clear() {
            if (Type == null) return;
            Type = null;
            Amount = 0;
            _onChangeCallback?.Invoke();
        }
    }
}