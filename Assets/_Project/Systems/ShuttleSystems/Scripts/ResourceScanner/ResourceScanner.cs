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
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private Transform _origin;
        #endregion

        #region Public variables
        public event Action<ResourceDeposit> OnEnter
        {
            add => _scanner.OnEnter += value;
            remove => _scanner.OnEnter -= value;
        }
        public event Action<ResourceDeposit> OnLeave
        {
            add => _scanner.OnLeave += value;
            remove => _scanner.OnLeave -= value;
        }
        
        public readonly Utility.IObservable<Configuration> Configuration = new Observable<Configuration>();
        #endregion
        
        #region Properties
        public Transform Origin => _origin;
        public float Range => Configuration.Value ? Configuration.Value.Range : 0;
        public IReadOnlyCollection<ResourceDeposit> InRange => _scanner.InRange;
        #endregion

        #region Private variables
        private readonly Scanner<ResourceDeposit> _scanner = new Scanner<ResourceDeposit>();
        #endregion

        private void Awake() {
            Configuration.OnChange += configuration =>
            {
                if (configuration) Clear();
            };
        }

        private void FixedUpdate() {
            if (!Configuration.Value) return;
            _scanner.Scan(_origin, Configuration.Value.Range, _layerMask);
        }

        private void Clear() => _scanner.Clear();

        private void OnDestroy() => Clear();

        private void Reset() => _origin = transform;
        
        
        #region IPersistable
        [Serializable]
        public class PersistentData: ShuttleUpgrade.PersistentData<Configuration>
        {
            public PersistentData(Configuration configuration): base(configuration) { }
        }

        public object CaptureState() => new PersistentData(Configuration.Value);

        public void RestoreState(object state)
        {
            var persistentData = (PersistentData) state;
            Configuration.Set(persistentData.Upgrade);
        }
        #endregion
    }
}
