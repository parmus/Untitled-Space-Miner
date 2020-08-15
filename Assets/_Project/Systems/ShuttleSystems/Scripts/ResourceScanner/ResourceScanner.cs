using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame.ShuttleSystems.ResourceScanner {
    [AddComponentMenu("Shuttle Systems/Resource Scanner")]
    public class ResourceScanner : MonoBehaviour {
        [SerializeField] private LayerMask _layerMask = default;
        [SerializeField] private Transform _origin = default;

        public Transform Origin => _origin;
        public float Range => _configuration ? _configuration.Range : 0;
        public event Action<IResourceScannerItem> OnEnter;
        public IEnumerable<IResourceScannerItem> InRange => _inRange.Values;
        public Configuration Configuration {
            get => _configuration;
            set {
                _configuration = value;
                if (value) Clear();
            }
        }

        private Configuration _configuration;
        private readonly Scanner<ResourceDeposit> _scanner = new Scanner<ResourceDeposit>();

        private readonly Dictionary<ResourceDeposit, ResourceScannerItem> _inRange = new Dictionary<ResourceDeposit, ResourceScannerItem>();

        private void Awake() {
            _scanner.OnEnter += OnResourceDepositEnter;
            _scanner.OnLeave += OnResourceDepositLeave;
        }

        private void OnResourceDepositEnter(ResourceDeposit r) {
            var item = new ResourceScannerItem(this, r);
            _inRange.Add(r, item);
            r.OnDestroy += OnResourceDepositLeave;
            OnEnter?.Invoke(item);
        }

        private void OnResourceDepositLeave(ResourceDeposit r) {
            if (!_inRange.TryGetValue(r, out var item)) return;
            _inRange.Remove(item.ResourceDeposit);
            item.ResourceDeposit.OnDestroy -= OnResourceDepositLeave;
            item.Destroy();
        }


        private void FixedUpdate() {
            if (!_configuration) return;

            foreach(var item in _inRange.Values) {
                item.UpdateDistance();
            }

            _scanner.Scan(_origin, _configuration.Range, _layerMask);
        }

        private void Clear() {
            foreach(var item in _inRange.Values) {
                item.ResourceDeposit.OnDestroy -= OnResourceDepositLeave;
            }
            _inRange.Clear();
        }

        private void OnDestroy() => Clear();

        private void Reset() {
            _origin = transform;
        }
    }
}