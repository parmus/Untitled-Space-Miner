using System;
using UnityEngine;

namespace SpaceGame.ShuttleSystems.Hull {
    [AddComponentMenu("Shuttle Systems/Hull")]
    public class Hull : MonoBehaviour {
        [SerializeField] private DefaultHull _defaultHull = new DefaultHull();

        #region Events
        public event Action<float> OnImpact;
        public event Action<float> OnIntegrityChange;
        public event Action OnDie;
        #endregion

        #region Properties
        public HullUpgrade Upgrade { get; set; } = default;
        public float Integrity {
            get => _integrity;
            private set {
                _integrity = Mathf.Clamp01(value);
                OnIntegrityChange?.Invoke(_integrity);
                if (_integrity < Mathf.Epsilon) {
                    OnDie?.Invoke();
                }
            }
        }
        #endregion

        #region Private properties and variables
        private IHullConfiguration CurrentHull => Upgrade ? (IHullConfiguration) Upgrade : _defaultHull;
        private float HullStrength => CurrentHull.HullStrength;
        private float _integrity = 1f;
        #endregion

        private void OnCollisionEnter(Collision other) {
            var impact = other.relativeVelocity.magnitude / HullStrength;
            OnImpact?.Invoke(impact);
            Integrity -= impact / 100f;
        }

        [Serializable]
        private class DefaultHull: IHullConfiguration
        {
            [SerializeField] private float _defaultHullStrength = 1f;
            public float HullStrength => _defaultHullStrength;
        }
    }
}