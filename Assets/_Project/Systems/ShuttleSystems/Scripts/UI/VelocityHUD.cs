using UnityEngine;
using UnityEngine.Events;

namespace SpaceGame.ShuttleSystems.UI {
    [AddComponentMenu("Shuttle Systems/UI/Velocity HUD")]
    public class VelocityHUD : MonoBehaviour {
        [SerializeField] private UnityEvent<float> _onChange = default;

        private Thrusters.Thrusters _thrusters = default;

        public void SetThrusters(Thrusters.Thrusters thrusters) {
            if (_thrusters) _thrusters.Velocity.OnChange -= OnVelocityChange;
            _thrusters = thrusters;
            if (_thrusters) {
                _thrusters.Velocity.OnChange += OnVelocityChange;
                OnVelocityChange(_thrusters.Velocity.Value);
            } else {
                OnVelocityChange(0);
            }
        }

        private void OnDestroy() {
            if (_thrusters) _thrusters.Velocity.OnChange -= OnVelocityChange;
        }

        private void OnVelocityChange(int velocity) => _onChange.Invoke(velocity);
    }
}