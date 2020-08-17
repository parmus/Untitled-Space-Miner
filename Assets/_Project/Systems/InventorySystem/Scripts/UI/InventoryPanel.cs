using System.Collections.Generic;
using SpaceGame.Utility;
using UnityEngine;

namespace SpaceGame.InventorySystem.UI {
    public class InventoryPanel : MonoBehaviour {
        [SerializeField] private RectTransform _content = default;
        [SerializeField] private InventorySlot _slotPrefab = default;

        private void Awake()
        {
            if (_inventory == null) _content.DestroyAllChildren();
        }

        private IInventory _inventory = null;
        public IInventory Inventory {
            get => _inventory;
            set {
                if (_inventory != null)
                {
                    _inventory.OnChange -= OnInventoryChange;
                    _inventory.OnResize -= OnInventoryResize;
                }
                _content.DestroyAllChildren();
                _slots.Clear();
                _inventory = value;
                if (_inventory == null) return;
                _inventory.OnChange += OnInventoryChange;
                _inventory.OnResize += OnInventoryResize;
                OnInventoryResize();
            }
        }

        private void OnInventoryChange() => _slots.ForEach(slot => slot.OnChange());

        private void OnInventoryResize()
        {
            while (_inventory.Count > _slots.Count)
            {
                var slot = Instantiate(_slotPrefab, _content);
                slot.Stack = _inventory[_slots.Count];
                _slots.Add(slot);
            }
            while (_inventory.Count < _slots.Count)
            {
                Destroy(_slots[_slots.Count-1].gameObject);
                _slots.RemoveAt(_slots.Count-1);
            }
        }

        private readonly List<InventorySlot> _slots = new List<InventorySlot>();
    }
}
