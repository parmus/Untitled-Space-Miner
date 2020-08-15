using UnityEngine;
using UnityEngine.Events;

namespace SpaceGame.ShuttleSystems.UI {
    [AddComponentMenu("Shuttle Systems/Hull HUD")]
    public class HullHUD: MonoBehaviour {
        [SerializeField] private UnityEvent<float> _onHullIntegrityChange = default;
        private Hull.Hull _hull = default;

        public void SetHull(Hull.Hull hull) {
            if (_hull) _hull.OnIntegrityChange -= OnHullIntegrityChange;
            _hull = hull;
            if (!_hull) return;
            _hull.OnIntegrityChange += OnHullIntegrityChange;
            OnHullIntegrityChange(_hull.Integrity);
        }

        private void OnDestroy() {
            if (_hull) _hull.OnIntegrityChange -= OnHullIntegrityChange;
        }

        private void OnHullIntegrityChange(float integrity) {
            _onHullIntegrityChange.Invoke(integrity);
        }
    }
}
