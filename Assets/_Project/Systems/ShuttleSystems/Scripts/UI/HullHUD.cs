using UnityEngine;
using UnityEngine.Events;

namespace SpaceGame.ShuttleSystems.UI {
    [AddComponentMenu("Shuttle Systems/Hull HUD")]
    public class HullHUD: MonoBehaviour {
        [SerializeField] private UnityEvent<float> _onHullIntegrityChange = default;
        private Hull.Hull _hull = default;

        public void SetHull(Hull.Hull hull) {
            if (_hull) _hull.Integrity.Unsubscribe(OnHullIntegrityChange);
            _hull = hull;
            if (_hull) _hull.Integrity.Subscribe(OnHullIntegrityChange);
        }

        private void OnDestroy() {
            if (_hull) _hull.Integrity.Unsubscribe(OnHullIntegrityChange);
        }

        private void OnHullIntegrityChange(float integrity) => _onHullIntegrityChange.Invoke(integrity);
    }
}
