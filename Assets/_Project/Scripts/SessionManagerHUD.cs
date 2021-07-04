using SpaceGame.InventorySystem;
using SpaceGame.PlayerInput;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceGame
{
    public class SessionManagerHUD : MonoBehaviour
    {
        [SerializeField] private InputReader _inputReader;
        [SerializeField] private UnityEvent<IInventory> _onInventoryChange = default;
        [SerializeField] private Canvas _HUD = default;

        private void Start() {
            _onInventoryChange.Invoke(SessionManager.Instance.Inventory);
            _HUD.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _inputReader.OnOpenInventory += OnOpenInventory;
            _inputReader.OnCloseInventory += OnCloseInventory;
        }

        private void OnDisable()
        {
            _inputReader.OnOpenInventory -= OnOpenInventory;
            _inputReader.OnCloseInventory -= OnCloseInventory;
        }

        private void OnCloseInventory()
        {
            _HUD.gameObject.SetActive(false);
        }

        private void OnOpenInventory()
        {
            _HUD.gameObject.SetActive(true);
            _inputReader.EnableUI();
        }
    }
}
