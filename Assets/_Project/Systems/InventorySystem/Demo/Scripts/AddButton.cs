using UnityEngine;

namespace SpaceGame.InventorySystem.Demo.Scripts {
    [RequireComponent(typeof(UnityEngine.UI.Button))]
    public class AddButton : MonoBehaviour {
        [SerializeField] private ResourceType _resourceType = default;
        [SerializeField] private uint _amount = 1;

        public IInventory Inventory { get; set; } = null;

        private void Awake() {
            var button = GetComponent<UnityEngine.UI.Button>();
            button.onClick.AddListener(() => Inventory?.Add(_resourceType, _amount));
        }

        private void OnValidate() {
            if (_resourceType == null) return;
            var label = GetComponentInChildren<UnityEngine.UI.Text>();
            if (label == null) return;
            label.text = "Add " + _amount + " " + _resourceType.name;
            gameObject.name = "Add " + _amount + " " + _resourceType.name + " button";
        }
    }
}
