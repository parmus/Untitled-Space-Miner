using SpaceGame.Utility;
using UnityEngine;

namespace SpaceGame.Interactables
{
    public class Raycaster<T>
    {
        public IReadonlyObservable<T> CurrentTarget => _currentTarget;
        private readonly Observable<T> _currentTarget = new Observable<T>();
        private readonly IObservable<Collider> _currentTargetCollider = new Observable<Collider>();

        private RaycastHit _hit;
        private readonly Transform _origin;
        private readonly float _range;
        private readonly LayerMask _layerMask;


        public Raycaster(Transform origin, float range, LayerMask layerMask = default)
        {
            _origin = origin;
            _range = range;
            _layerMask = layerMask;
            _currentTargetCollider.OnChange += collider => _currentTarget.Set(collider == null ? default : collider.GetComponent<T>());
        }

        public void Update()
        {
            _currentTargetCollider.Value =
                Physics.Raycast(_origin.position, _origin.forward, out _hit, _range, _layerMask) ? _hit.collider : null;
        }
    }
}