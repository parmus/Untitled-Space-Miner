using SpaceGame.Utility;
using UnityEngine;

namespace SpaceGame.ShuttleSystems.InertiaDampers {
    [AddComponentMenu("Shuttle Systems/Inertia Dampers")]
    public class InertiaDampers : MonoBehaviour {
        [SerializeField] private Rigidbody _rigidbody;

        [Header("Configuration when disabled")]
        [SerializeField] private DefaultInertiaDampers _defaultInertiaDampers = new DefaultInertiaDampers();

        public readonly IObservable<InertiaDamperUpgrade> Upgrade = new Observable<InertiaDamperUpgrade>();

        private void Awake()
        {
            Upgrade.OnChange += upgrade =>
            {
                if (enabled && upgrade)
                {
                    EnableDampers();
                }
                else
                {
                    DisableDampers();
                }
            };
        }

        private void Reset() {
            _rigidbody = GetComponentInChildren<Rigidbody>();
        }

        private void EnableDampers() {
            if (!Upgrade.Value) return;
            _rigidbody.drag = Upgrade.Value.Drag;
            _rigidbody.angularDrag = Upgrade.Value.AngularDrag;
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