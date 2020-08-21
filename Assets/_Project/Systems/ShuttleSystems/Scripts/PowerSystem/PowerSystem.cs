using System;
using SpaceGame.Utility;
using UnityEngine;

namespace SpaceGame.ShuttleSystems.PowerSystem
{
    public class PowerSystem : MonoBehaviour
    {
        [SerializeField] private DefaultConfiguration _defaultConfiguration = new DefaultConfiguration();

        public readonly Utility.IObservable<PowerSystemUpgrade> Upgrade = new Observable<PowerSystemUpgrade>();
        
        private void Awake()
        {
            Capacity = _defaultConfiguration.Capacity;
            Charge = _defaultConfiguration.Capacity;
            Upgrade.OnChange += upgrade => Capacity = upgrade ? upgrade.Capacity : _defaultConfiguration.Capacity;
        }
        
        [Serializable]
        private class DefaultConfiguration : IPowerSystemConfiguration
        {
            [SerializeField] private float _capacity = 100f;
            public float Capacity => _capacity;
        }

        public event Action<float> OnChargeChange;
        public event Action<float> OnCapacityChange;

        public float Capacity {
            get => _capacity;
            set {
                _capacity = Mathf.Max(value, 0f);
                _charge = Mathf.Clamp(_charge, 0f, Capacity);
                OnCapacityChange?.Invoke(_capacity);
                OnChargeChange?.Invoke(Charge);
            }
        }

        public float Charge {
            get => _charge;
            set {
                var prevCharge = _charge;
                _charge = Mathf.Clamp(value, 0f, Capacity);
                if (Mathf.Approximately(_charge, prevCharge)) return;
                OnChargeChange?.Invoke(Charge);
            }
        }

        public float ChargePercentage => _charge / _capacity;
        public bool IsEmpty => Mathf.Approximately(_charge, 0f);

        private float _charge;
        private float _capacity;
    }
}