using SpaceGame.Core;
using UnityEngine;

namespace SpaceGame.InventorySystem.Demo {
    [RequireComponent(typeof(UnityEngine.UI.Button))]
    public class AddButton : MonoBehaviour {
        [SerializeField] private ItemType _itemType = default;
        [SerializeField] private uint _amount = 1;

        public IInventory Inventory { get; set; } = null;

        private void Awake() {
            var button = GetComponent<UnityEngine.UI.Button>();
            button.onClick.AddListener(() => Inventory?.Add(_itemType, _amount));
        }

        private void OnValidate() {
            if (_itemType == null) return;
            var label = GetComponentInChildren<UnityEngine.UI.Text>();
            if (label == null) return;
            label.text = "Add " + _amount + " " + _itemType.name;
            gameObject.name = "Add " + _amount + " " + _itemType.name + " button";
        }
    }
}
