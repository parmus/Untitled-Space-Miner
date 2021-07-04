using SpaceGame.Interactables;
using SpaceGame.PlayerInput;
using UnityEngine;

namespace SpaceGame
{
    public class Terminal : MonoBehaviour, IInteractable
    {
        [SerializeField] private Canvas _terminalCanvas = default;
        [SerializeField] private string _prompt = "Open terminal";
        [SerializeField] private InputReader _inputReader;

        public bool IsOpen => _terminalCanvas.enabled;

        private void Start() => _terminalCanvas.enabled = false;

        private void OnEnable() => _inputReader.OnCloseInventory += Dismiss;

        private void OnDisable() => _inputReader.OnCloseInventory -= Dismiss;

        private void Dismiss()
        {
            if (!IsOpen) return;

            _terminalCanvas.enabled = false;
            _inputReader.EnableRobot();
        }
        
        public void Interact()
        {
            if (IsOpen) return;

            _terminalCanvas.enabled = true;
            _inputReader.EnableUI();
        }

        public string Prompt => _prompt;
    }
}
