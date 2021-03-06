﻿using UnityEngine;
using UnityEngine.Events;

namespace SpaceGame.Interactables
{
    public class InteractableSwitch : MonoBehaviour, IInteractable
    {
        [SerializeField] private bool _enabled;
        [SerializeField] private UnityEvent<bool> _onChange;
        [SerializeField] private UnityEvent _onEnable;
        [SerializeField] private UnityEvent _onDisable;
        [SerializeField] private bool _triggerOnStart = true;
        [SerializeField] private string _enabledPrompt;
        [SerializeField] private string _disabledPrompt;
        
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
