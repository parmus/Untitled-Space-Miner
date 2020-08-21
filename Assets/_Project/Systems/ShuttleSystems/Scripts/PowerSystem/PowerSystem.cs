using System;
using SpaceGame.Utility;
using UnityEngine;

namespace SpaceGame.ShuttleSystems.PowerSystem
{
    public class PowerSystem : MonoBehaviour, IPersistable
    {
        [SerializeField] private DefaultConfiguration _defaultConfiguration = new DefaultConfiguration();

        #region Observables
        public readonly Utility.IObservable<PowerSystemUpgrade> Upgrade = new Observable<PowerSystemUpgrade>();
        public IReadonlyObservable<float> Charge => _charge;
        public IReadonlyObservable<float> Capacity => _capacity;
        #endregion

        #region Private variables
        private readonly Observable<float> _charge = new Observable<float>();
        private readonly Observable<float> _capacity = new Observable<float>();
        #endregion

        #region Public methods
        public void Consume(float amount) => _charge.Value = Mathf.Clamp(_charge.Value - amount, 0f, _capacity.Value);
        public void Recharge(float amount) => _charge.Value = Mathf.Clamp(_charge.Value + amount, 0f, _capacity.Value);
        public float ChargePercentage => _charge.Value / _capacity.Value;
        public bool IsEmpty => _charge.Value.IsZero();
        #endregion
        
        private void Awake()
        {
            Upgrade.OnChange += upgrade => _capacity.Set(upgrade ? upgrade.Capacity : _defaultConfiguration.Capacity);
            _capacity.OnChange += capacity => _charge.Set(Mathf.Clamp(_charge.Value, 0f, capacity));
        }

        private void Start()
        {
            _capacity.Set(_defaultConfiguration.Capacity);
            _charge.Set(_defaultConfiguration.Capacity);
        }

        [Serializable]
        private class DefaultConfiguration : IPowerSystemConfiguration
        {
            [SerializeField] private float _capacity = 100f;
            public float Capacity => _capacity;
        }
        
        
        #region IPersistable
        [Serializable]
        public class PersistentData
        {
            public readonly string PowerSystemUpgradeName;
            public readonly float Charge;

            public PowerSystemUpgrade PowerSystemUpgrade => PowerSystemUpgrade.GetByName(PowerSystemUpgradeName);

            public PersistentData(PowerSystemUpgrade powerSystemUpgrade, float charge)
            {
                PowerSystemUpgradeName = powerSystemUpgrade != null ? powerSystemUpgrade.name : null;
                Charge = charge;
            }
        }

        public object CaptureState() => new PersistentData(Upgrade.Value, _charge.Value);

        public void RestoreState(object state)
        {
            var persistentData = (PersistentData) state;
            Upgrade.Set(persistentData.PowerSystemUpgrade);
            _charge.Set(persistentData.Charge);
        }
        #endregion
       
    }
}