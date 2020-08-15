using System;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceGame.InventorySystem.Demo.Scripts {
    public class DemoInventory : MonoBehaviour {
        [SerializeField][Delayed] private uint _numberOfSlots = 5;
        [SerializeField] private uint _stackMultiplier = 1;
        [SerializeField] private UnityEvent<IInventory> _onChange = default;

        private IInventory _inventory = default;

        private void Start() {
            _inventory = new Inventory(_numberOfSlots, _stackMultiplier);
            _onChange.Invoke(_inventory);
        }

        private void OnValidate() => _inventory?.Resize(_numberOfSlots);
    }
}