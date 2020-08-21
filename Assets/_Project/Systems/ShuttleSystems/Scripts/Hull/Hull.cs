using System;
using SpaceGame.Utility;
using UnityEngine;

namespace SpaceGame.ShuttleSystems.Hull {
    [AddComponentMenu("Shuttle Systems/Hull")]
    public class Hull : MonoBehaviour, IPersistable {
        [SerializeField] private DefaultHull _defaultHull = new DefaultHull();

        #region Events
        public event Action<float> OnImpact;
        public event Action OnDie;
        #endregion

        #region Properties
        public readonly Utility.IObservable<HullUpgrade> Upgrade = new Observable<HullUpgrade>();
        public IReadonlyObservable<float> Integrity => _integrity;
        #endregion

        #region Private variables
        private readonly Observable<float> _integrity = new Observable<float>(1f); 
        private IHullConfiguration _currentHull;
        #endregion

        private void OnEnable()
        {
            Upgrade.Subscribe(OnUpgradeChange);
            _integrity.Subscribe(OnIntegrityChange);
        }

        private void OnDisable()
        {
            _integrity.Unsubscribe(OnIntegrityChange);
            Upgrade.Unsubscribe(OnUpgradeChange);
        }

        private void OnUpgradeChange(HullUpgrade upgrade) => _currentHull = upgrade ? (IHullConfiguration) upgrade : _defaultHull;

        private void OnIntegrityChange(float integrity)
        {
            if (integrity.IsZero()) {
                OnDie?.Invoke();
            }
        }


        private void OnCollisionEnter(Collision other) {
            var impact = other.relativeVelocity.magnitude / _currentHull.HullStrength;
            OnImpact?.Invoke(impact);
            _integrity.Set(Mathf.Clamp01(_integrity.Value - impact / 100f));
        }

        [Serializable]
        private class DefaultHull: IHullConfiguration
        {
            [SerializeField] private float _defaultHullStrength = 1f;
            public float HullStrength => _defaultHullStrength;
        }


        #region IPersistable
        [Serializable]
        public class PersistentData
        {
            public readonly string HullUpgradeName;
            public readonly float Integrity;

            public HullUpgrade HullUpgrade => HullUpgrade.GetByName(HullUpgradeName);

            public PersistentData(HullUpgrade hullUpgrade, float integrity)
            {
                HullUpgradeName = hullUpgrade != null ? hullUpgrade.name : null;
                Integrity = integrity;
            }
        }

        public object CaptureState() => new PersistentData(Upgrade.Value, _integrity.Value);

        public void RestoreState(object state)
        {
            var persistentData = (PersistentData) state;
            Upgrade.Set(persistentData.HullUpgrade);
            _integrity.Set(persistentData.Integrity);
        }
        #endregion
    }
}