﻿using SpaceGame.InventorySystem;
using SpaceGame.InventorySystem.UI;
using SpaceGame.PlayerInput;
using SpaceGame.ShuttleSystems;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceGame
{
    public class SessionManagerHUD : MonoBehaviour
    {
        [SerializeField] private UnityEvent<IInventory> _onInventoryChange = default;
        [SerializeField] private Canvas _HUD = default;

        private Controls _controls;

        private void Awake() {
            _controls = new Controls();
            _controls.UI.ToggleInventory.performed += ctx =>
            {
                _HUD.gameObject.SetActive(!_HUD.gameObject.activeSelf);
                SetCursorEnabled(_HUD.gameObject.activeSelf);

                var shuttle = ShuttleSpawner.CurrentShuttle.Value;
                if (shuttle != null) shuttle.ShuttleControls.enabled = !_HUD.gameObject.activeSelf; 
            };
        }

        private void OnDestroy() => _controls.Disable();

        public static void Quit()
        {
            #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        } 

        private static void SetCursorEnabled(bool enabled) {
            Cursor.lockState = enabled ? CursorLockMode.Confined : CursorLockMode.Locked;
            Cursor.visible = enabled;
        }

        private void Start() {
            _onInventoryChange.Invoke(SessionManager.Instance.Inventory);
            _HUD.gameObject.SetActive(false);
            SetCursorEnabled(false);
        }

        private void OnEnable() {
            _controls.UI.Enable();
        }

        private void OnDisable() {
            _controls.UI.Disable();
        }
    }
}
