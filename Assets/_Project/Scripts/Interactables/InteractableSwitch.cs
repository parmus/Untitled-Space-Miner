using UnityEngine;
using UnityEngine.Events;

namespace SpaceGame.Interactables
{
    public class InteractableSwitch : MonoBehaviour, IInteractable
    {
        [SerializeField] private bool _enabled;
        [SerializeField] private UnityEvent<bool> _onChange = default;
        [SerializeField] private UnityEvent _onEnable = default;
        [SerializeField] private UnityEvent _onDisable = default;
        [SerializeField] private bool _triggerOnStart = true;
        [SerializeField] private string _enabledPrompt = default;
        [SerializeField] private string _disabledPrompt = default;
        
        public string Prompt => _enabled ? _enabledPrompt : _disabledPrompt;
        
        public void Interact()
        {
            _enabled = !_enabled;
            TriggerCallbacks();
        }

        private void Start()
        {
            if (_triggerOnStart) TriggerCallbacks();
        }

        private void TriggerCallbacks()
        {
            _onChange.Invoke(_enabled);
            if (_enabled) {
                _onEnable.Invoke();
            } else {
                _onDisable.Invoke();
            }
        }
    }
}