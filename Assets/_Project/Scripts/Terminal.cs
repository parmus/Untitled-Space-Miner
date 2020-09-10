using SpaceGame.Interactables;
using UnityEngine;

namespace SpaceGame
{
    public class Terminal : MonoBehaviour, IInteractable
    {
        [SerializeField] private Canvas _terminalCanvas = default;
        [SerializeField] private string _prompt = "Open terminal";

        private CharacterControls _characterControls;

        private void Awake() => _characterControls = FindObjectOfType<CharacterControls>();

        public bool IsOpen => _terminalCanvas.enabled;

        private void Start() => _terminalCanvas.enabled = false;

        public void Dismiss()
        {
            if (!IsOpen) return;

            _terminalCanvas.enabled = false;
            _characterControls.enabled = true;
        }
        
        public void Interact()
        {
            if (IsOpen) return;

            _terminalCanvas.enabled = true;
            _characterControls.enabled = false;
        }

        public string Prompt => _prompt;
    }
}