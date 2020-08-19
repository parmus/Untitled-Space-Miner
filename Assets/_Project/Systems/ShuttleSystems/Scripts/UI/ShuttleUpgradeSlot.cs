using System;
using JetBrains.Annotations;
using SpaceGame.Core;
using SpaceGame.InventorySystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SpaceGame.ShuttleSystems.UI {
    public abstract class ShuttleUpgradeSlot<T>: MonoBehaviour, IDropHandler, IStackProvider where T: ShuttleUpgrade
    {
        [SerializeField] private Image _image = default;
        [SerializeField] private Image _background = default;
        [SerializeField] private TMPro.TextMeshProUGUI _label = default;
        
        private FakeStack _stack;
        public IInventoryStack Stack => _stack;

        private void Awake()
        {
            _stack = new FakeStack(Set, Get);
        }

        private void OnEnable()
        {
            ShuttleConfigurationManager.OnChange += OnConfigurationChange;
            OnConfigurationChange();
        }

        private void OnDisable() => ShuttleConfigurationManager.OnChange -= OnConfigurationChange;

        protected abstract void Set([CanBeNull] T upgrade);
        protected abstract T Get();

        private void OnConfigurationChange()
        {
            var upgrade = Get();
            if (upgrade)
            {
                _background.enabled = false;
                _image.sprite = upgrade.Thumbnail;
                _image.enabled = true;
                _label.text = upgrade.Name;
                _label.enabled = true;
            }
            else
            {
                _background.enabled = true;
                _image.sprite = null;
                _image.enabled = false;
                _label.enabled = false;
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
    }
}
