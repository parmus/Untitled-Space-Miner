﻿using System;
using JetBrains.Annotations;
using SpaceGame.Core;
using SpaceGame.InventorySystem;
using SpaceGame.Utility.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SpaceGame.ShuttleSystems.UI.UpgradeSlot {
    [SelectionBase]
    public abstract class ShuttleUpgradeSlot<T>: MonoBehaviour, IDropHandler, ITooltipProvider, IStackProvider where T: ShuttleUpgrade
    {
        [SerializeField] private Image _image;
        [SerializeField] private Image _background;
        [SerializeField] private ShuttleAnchor _shuttleAnchor;
        
        private FakeStack _stack;
        public IInventoryStack Stack => _stack;

        private void Awake() => _stack = new FakeStack(Set, Get);


        private void OnShuttleChange(Shuttle shuttle)
        {
            _upgrade?.Unsubscribe(OnConfigurationChange);
            _upgrade = shuttle ? UpgradeFromShuttle(shuttle) : null;
            _upgrade?.Subscribe(OnConfigurationChange);
        }
        
        protected abstract Utility.IObservable<T> UpgradeFromShuttle(Shuttle shuttle);

        private Utility.IObservable<T> _upgrade;
        
        private void OnEnable() => _shuttleAnchor.Subscribe(OnShuttleChange);

        private void OnDisable() => _shuttleAnchor.Unsubscribe(OnShuttleChange);

        private void Set([CanBeNull] T upgrade)
        {
            if (_upgrade == null) return;
            _upgrade.Value = upgrade;
        }

        private T Get() => _upgrade?.Value;

        private void OnConfigurationChange(T upgrade)
        {
            if (upgrade)
            {
                _background.enabled = false;
                _image.sprite = upgrade.Thumbnail;
                _image.enabled = true;
            }
            else
            {
                _background.enabled = true;
                _image.sprite = null;
                _image.enabled = false;
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null) return;
            var stackProvider = eventData.pointerDrag.GetComponent<IStackProvider>();
            var stack = stackProvider?.Stack;
            if (!(stack?.Type is T)) return;

            var currentUpgrade = Get();
            Set(stack.Type as T);
            stack.TryRemove(1);
            if (currentUpgrade == null) return;
            stack.TryAdd(currentUpgrade, 1);
        }
        
        private class FakeStack : IInventoryStack
        {
            private readonly Action<T> _set;
            private readonly Func<T> _get;
        
            public FakeStack(Action<T> set, Func<T> get) {
                _set = set;
                _get = get;
            }

            public ItemType Type => _get();
            public uint Amount => _get() == null ? 0U : 1U;
            public float PercentageFull => Amount;
            public uint Free => 1 - Amount;
            public uint StackSize => Amount;
            public uint TryAdd(ItemType type, uint amount)
            {
                if (amount == 0) return 0;
                if (_get() != null) return 0;
                var value = type as T;
                if (value == null) return 0;
                _set(value);
                return 1;
            }

            public uint TryAdd(uint amount) => 0;

            public uint TryRemove(uint amount)
            {
                if (amount == 0 || !_get()) return 0;
                _set(null);
                return 1;
            }

            public void Clear() => TryRemove(1);
        }

        public string GetTooltip() => _upgrade.Value == null ? null : _upgrade.Value.Tooltip;
    }
}
