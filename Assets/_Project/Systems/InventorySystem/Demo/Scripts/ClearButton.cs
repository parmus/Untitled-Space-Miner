using UnityEngine;

namespace SpaceGame.InventorySystem.Demo.Scripts {
    [RequireComponent(typeof(UnityEngine.UI.Button))]
    public class ClearButton : MonoBehaviour {
        public IInventory Inventory { get; set; } = null;

        private void Awake() {
            var button = GetComponent<UnityEngine.UI.Button>();
            button.onClick.AddListener(() => Inventory?.Clear());
        }
    }
}
