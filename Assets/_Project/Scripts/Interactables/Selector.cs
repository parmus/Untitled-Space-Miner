using System;
using SpaceGame.PlayerInput;
using SpaceGame.Utility;
using UnityEngine;

namespace SpaceGame.Interactables
{
    public class Selector : MonoBehaviour
    {
        [SerializeField] private Transform _origin = default;
        [SerializeField] private float _range = 3f;
        [SerializeField] private LayerMask _layerMask = default;

        public IReadonlyObservable<IInteractable> CurrentInteractable => _raycaster.CurrentTarget;
        
        private Raycaster<IInteractable> _raycaster;
        private Controls _controls;

        private void Awake()
        {
            _raycaster = new Raycaster<IInteractable>(_origin, _range, _layerMask);
            _controls = new Controls();
            _controls.Robot.Interact.performed += context => _raycaster.CurrentTarget.Value?.Interact();
        }

        private void Update() => _raycaster.Update();

        private void OnEnable() => _controls.Robot.Enable();
        private void OnDisable() => _controls.Robot.Disable();
        private void Reset() => _origin = transform;

        private void OnDrawGizmosSelected()
        {
            var pos = _origin.position;
            Gizmos.DrawLine(pos, pos + (_origin.forward * _range));
        }
    }
}