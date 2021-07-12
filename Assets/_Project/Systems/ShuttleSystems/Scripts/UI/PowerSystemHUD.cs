using System;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceGame.ShuttleSystems.UI {
    [AddComponentMenu("Shuttle Systems/Power System HUD")]
    public class PowerSystemHUD : MonoBehaviour {
        [SerializeField] private UnityEvent<float> _onChargeChange;
        [SerializeField] private UnityEvent<float> _onCapacityChange;
        
        [Header("Power Level Messages")]
        [SerializeField] private float _lowPowerThreshold = 15f;
        [SerializeField] private UnityEvent _onPowerLevelNormal;
        [SerializeField] private UnityEvent _onPowerLevelLow;
        [SerializeField] private UnityEvent _onPowerLevelLost;

        private PowerLevel _powerLevel = PowerLevel.Lost;

        private PowerSystem.PowerSystem _powerSystem;

        public void SetPowerSystem(PowerSystem.PowerSystem powerSystem) {
            if (_powerSystem) {
                _powerSystem.Capacity.Unsubscribe(OnCapacityChange);
                _powerSystem.Charge.Unsubscribe(OnChargeChange);
            }
            _powerSystem = powerSystem;
            if (_powerSystem) {
                _powerSystem.Charge.Subscribe(OnChargeChange);
                _powerSystem.Capacity.Subscribe(OnCapacityChange);
            } else {
                OnCapacityChange(0f);
                OnChargeChange(0f);
            }
        }

        private void OnDestroy()
        {
            if (!_powerSystem) return;
            _powerSystem.Charge.Unsubscribe(OnChargeChange);
            _powerSystem.Capacity.Unsubscribe(OnCapacityChange);
        }

        private void OnCapacityChange(float capacity) {
            _onCapacityChange.Invoke(capacity);
            UpdatePowerLevel();
        }

        private void OnChargeChange(float charge) {
            _onChargeChange.Invoke(charge);
            UpdatePowerLevel();
        }

        private void UpdatePowerLevel() {
            var newPowerLevel = !_powerSystem || _powerSystem.Charge.Value > _lowPowerThreshold ?
                PowerLevel.Normal :
                _powerSystem.IsEmpty ? PowerLevel.Lost : PowerLevel.Low;
            if (newPowerLevel == _powerLevel) return;
            _powerLevel = newPowerLevel;
            switch(_powerLevel) {
                case PowerLevel.Normal:
                    _onPowerLevelNormal.Invoke();
                    break;
                case PowerLevel.Low:
                    _onPowerLevelLow.Invoke();
                    break;
                case PowerLevel.Lost:
                    _onPowerLevelLost.Invoke();
                    break;

                default:
                    Debug.LogError("It looks like you forgot an enum in the switch-block");
                    throw new ArgumentOutOfRangeException();
            }
        }

        private enum PowerLevel {
            Normal,
            Low,
            Lost
        }
    }
}
