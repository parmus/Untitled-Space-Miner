using UnityEngine;
using UnityEngine.Rendering;

namespace SpaceGame.ShuttleSystems.VFX {
    [AddComponentMenu("Shuttle Systems/VFX/Low Power Volume Effect")]
    public class LowPowerVolumeEffect : MonoBehaviour {
        [SerializeField] private Volume _volume = default;
        [SerializeField] private float _lowPowerThreshold = 15f;

        private PowerSystem.PowerSystem _powerSystem = default;

        public void SetPowerSystem(PowerSystem.PowerSystem powerSystem) {
            if (_powerSystem != null) _powerSystem.Charge.Unsubscribe(OnChargeChange);
            _powerSystem = powerSystem;
            if (_powerSystem != null) _powerSystem.Charge.Subscribe(OnChargeChange);
        }

        private void OnChargeChange(float change) => _volume.weight = 1 - Mathf.InverseLerp(0f, _lowPowerThreshold, change);

        private void OnDestroy()
        {
            _volume.weight = 0;
            if (_powerSystem != null) _powerSystem.Charge.Unsubscribe(OnChargeChange);
        }

        private void Reset() => _volume = GetComponent<Volume>();
    }
}