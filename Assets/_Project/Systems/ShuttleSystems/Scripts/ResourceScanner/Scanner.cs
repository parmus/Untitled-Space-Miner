using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame.ShuttleSystems.ResourceScanner
{
    public class Scanner<T> where T: MonoBehaviour
    {
        public event Action<T> OnEnter;
        public event Action<T> OnLeave;
        public IReadOnlyCollection<T> InRange => _colliderRuntimeSet.Values;
        
        private readonly Dictionary<Collider, T> _colliderRuntimeSet = new Dictionary<Collider, T>();
        private readonly HashSet<Collider> _toDelete = new HashSet<Collider>();
        private readonly Collider[] _colliders;

        public Scanner(int maxColliders = 100)
        {
            _colliders = new Collider[maxColliders];
        }
        
        public void Scan(Transform origin, float range, LayerMask layerMask = default) {
            _toDelete.Clear();
            _toDelete.UnionWith(_colliderRuntimeSet.Keys);
            
            var numColliders = Physics.OverlapSphereNonAlloc(origin.position, range, _colliders, layerMask);

            for (var i = 0; i < numColliders; i++)
            {
                var collider = _colliders[i];
                _toDelete.Remove(collider);
                if (_colliderRuntimeSet.ContainsKey(collider)) continue;
                var component = collider.GetComponentInParent<T>();
                _colliderRuntimeSet.Add(collider, component);
                if (component) OnEnter?.Invoke(component);
            }


            foreach(var collider in _toDelete) {
                if (_colliderRuntimeSet.TryGetValue(collider, out var component))
                {
                    OnLeave?.Invoke(component);
                }
                _colliderRuntimeSet.Remove(collider);
            }
        }

        public void Clear() {
            foreach(var behaviour in _colliderRuntimeSet.Values) {
                if (behaviour != null) OnLeave?.Invoke(behaviour);
            }
            _colliderRuntimeSet.Clear();
        }
    }
}
