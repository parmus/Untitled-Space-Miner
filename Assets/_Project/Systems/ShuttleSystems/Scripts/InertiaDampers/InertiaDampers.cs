using UnityEngine;

namespace SpaceGame.ShuttleSystems.InertiaDampers {
    [AddComponentMenu("Shuttle Systems/Inertia Dampers")]
    public class InertiaDampers : MonoBehaviour {
        [SerializeField] private Rigidbody _rigidbody;

        [Header("Configuration when disabled")]
        [SerializeField] private DefaultInertiaDampers _defaultInertiaDampers = new DefaultInertiaDampers();

        public InertiaDamperUpgrade Upgrade {
            get => _upgrade;
            set {
                _upgrade = value;
                if (enabled && _upgrade) {
                    EnableDampers();
                } else {
                    DisableDampers();
                }
            }
        }

        private InertiaDamperUpgrade _upgrade = default;

        private void Reset() {
            _rigidbody = GetComponentInChildren<Rigidbody>();
        }

        private void EnableDampers() {
            if (!_upgrade) return;
            _rigidbody.drag = _upgrade.Drag;
            _rigidbody.angularDrag = _upgrade.AngularDrag;
        }

        private void DisableDampers() {
            _rigidbody.drag = _defaultInertiaDampers.Drag;
            _rigidbody.angularDrag = _defaultInertiaDampers.AngularDrag;
        }

        private void OnEnable() => EnableDampers();
        private void OnDisable() => DisableDampers();

        [System.Serializable]
        private class DefaultInertiaDampers : IInertiaDamperUpgrade
        {
            [SerializeField] private float _drag = 0f;
            [SerializeField] private float _angularDrag = 0.3f;

            public float Drag => _drag;
            public float AngularDrag => _angularDrag;
        }
    }
}