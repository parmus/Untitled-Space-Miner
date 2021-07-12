using SpaceGame.PlayerInput;
using SpaceGame.Utility;
using UnityEngine;

namespace SpaceGame.Interactables
{
    public class Selector : MonoBehaviour
    {
        [SerializeField] private Transform _origin;
        [SerializeField] private float _range = 3f;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private InputReader _inputReader;

        public IReadonlyObservable<IInteractable> CurrentInteractable => _raycaster.CurrentTarget;
        
        private Raycaster<IInteractable> _raycaster;

        private void Awake() => _raycaster = new Raycaster<IInteractable>(_origin, _range, _layerMask);

        private void OnInteract() => _raycaster.CurrentTarget.Value?.Interact();

        private void Update() => _raycaster.Update();

        private void OnEnable() => _inputReader.OnRobotInteract += OnInteract;

        private void OnDisable() => _inputReader.OnRobotInteract -= OnInteract;

        private void Reset() => _origin = transform;

        private void OnDrawGizmosSelected()
        {
            var pos = _origin.position;
            Gizmos.DrawLine(pos, pos + (_origin.forward * _range));
        }
    }
}
