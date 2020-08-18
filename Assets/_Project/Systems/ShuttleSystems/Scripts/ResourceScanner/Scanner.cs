using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame.ShuttleSystems.ResourceScanner
{
    public class Scanner<T> where T: MonoBehaviour
    {
        public event Action<T> OnEnter;
        public event Action<T> OnLeave;

        private readonly Dictionary<Collider, T> _colliderRuntimeSet = new Dictionary<Collider, T>();
        private readonly HashSet<Collider> _toDelete = new HashSet<Collider>();

        public void Scan(Transform origin, float range, LayerMask layerMask = default) {
            _toDelete.Clear();
            _toDelete.UnionWith(_colliderRuntimeSet.Keys);
            foreach(var collider in Physics.OverlapSphere(origin.position, range, layerMask)) {
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