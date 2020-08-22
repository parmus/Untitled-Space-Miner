using System;
using System.Collections.Generic;
using SpaceGame.Core;
using SpaceGame.Utility;
using SpaceGame.Utility.SaveSystem;
using UnityEngine;

namespace SpaceGame.ShuttleSystems.ResourceScanner {
    [AddComponentMenu("Shuttle Systems/Resource Scanner")]
    public class ResourceScanner : MonoBehaviour, IPersistable {
        #region Serialized fields
        [SerializeField] private LayerMask _layerMask = default;
        [SerializeField] private Transform _origin = default;
        #endregion

        #region Public variables
        public event Action<IResourceScannerItem> OnEnter;
        public readonly Utility.IObservable<Configuration> Configuration = new Observable<Configuration>();
        #endregion
        
        #region Properties
        public Transform Origin => _origin;
        public float Range => Configuration.Value ? Configuration.Value.Range : 0;
        public IEnumerable<IResourceScannerItem> InRange => _inRange.Values;
        #endregion

        #region Private variables
        private readonly Scanner<ResourceDeposit> _scanner = new Scanner<ResourceDeposit>();
        private readonly Dictionary<ResourceDeposit, ResourceScannerItem> _inRange = new Dictionary<ResourceDeposit, ResourceScannerItem>();
        #endregion

        private void Awake() {
            _scanner.OnEnter += OnResourceDepositEnter;
            _scanner.OnLeave += OnResourceDepositLeave;

            Configuration.OnChange += configuration =>
            {
                if (configuration) Clear();
            };
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
            if (!Configuration.Value) return;

            foreach(var item in _inRange.Values) {
                item.UpdateDistance();
            }

            _scanner.Scan(_origin, Configuration.Value.Range, _layerMask);
        }

        private void Clear() {
            foreach(var item in _inRange.Values) {
                item.ResourceDeposit.OnDestroy -= OnResourceDepositLeave;
            }
            _inRange.Clear();
        }

        private void OnDestroy() => Clear();

        private void Reset() => _origin = transform;
        
        
        #region IPersistable
        public object CaptureState() => Configuration.Value != null ? Configuration.Value.Name : null;

        public void RestoreState(object state)
        {
            var configurationName = (string) state;
            Configuration.Set(ItemType.GetByName<Configuration>(configurationName));
        }
        #endregion
    }
}