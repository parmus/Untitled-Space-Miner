using SpaceGame.Core;
using SpaceGame.Utility;
using SpaceGame.Utility.SaveSystem;
using UnityEngine;

namespace SpaceGame.ShuttleSystems.Thrusters {

    [AddComponentMenu("Shuttle Systems/Thrusters")]
    public class Thrusters : MonoBehaviour, IPersistable {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private DefaultThrusters _defaultThrusters = new DefaultThrusters();

        #region Properties
        public readonly Utility.IObservable<ThrusterUpgrade> Upgrade = new Observable<ThrusterUpgrade>();
        public IReadonlyObservable<int> Velocity => _velocity;
        #endregion

        #region Private properties
        private bool Boost => _shuttle.ShuttleControls.Boost;
        private float ThrustPower => Boost ? _currentThruster.BoostThrustPower : _currentThruster.NormalThrustPower;
        private float PowerConsumption => Boost ? _currentThruster.BoostPowerConsumption : _currentThruster.NormalPowerConsumption;
        #endregion

        #region Private variables
        private IThrusterUpgrade _currentThruster;
        private Vector3 _thrustDirection = Vector3.zero;
        private Shuttle _shuttle;
        private readonly Observable<int> _velocity = new Observable<int>();
        #endregion

        private void Awake() => _shuttle = GetComponent<Shuttle>();
        private void OnEnable() => Upgrade.Subscribe(OnUpgradeChange);
        private void OnDisable() => Upgrade.Unsubscribe(OnUpgradeChange);

        private void OnUpgradeChange(ThrusterUpgrade upgrade) => _currentThruster = upgrade ? (IThrusterUpgrade) upgrade : _defaultThrusters;

        private void FixedUpdate() {
            _thrustDirection.Set(_shuttle.ShuttleControls.Thrust.x, 0f, _shuttle.ShuttleControls.Thrust.y);

            if (_thrustDirection == Vector3.zero) return;
            if (_shuttle.PowerSystem.IsEmpty) return;

            _rigidbody.AddRelativeForce(_thrustDirection * (ThrustPower * Time.fixedDeltaTime));
            _shuttle.PowerSystem.Consume(PowerConsumption * Time.fixedDeltaTime);
            
            _velocity.Value = Mathf.RoundToInt(_rigidbody.velocity.magnitude);
        }

        private void Reset() => _rigidbody = GetComponent<Rigidbody>();

        #region IPersistable
        [System.Serializable]
        public class PersistentData
        {
            public readonly string ThrusterUpgradeName;

            public ThrusterUpgrade ThrusterUpgrade =>
                ItemType.GetByName<ThrusterUpgrade>(ThrusterUpgradeName);

            public PersistentData(ThrusterUpgrade thrusterUpgrade) =>
                ThrusterUpgradeName = thrusterUpgrade != null ? thrusterUpgrade.Name : null;
        }

        public object CaptureState() => new PersistentData(Upgrade.Value);

        public void RestoreState(object state)
        {
            var persistentData = (PersistentData) state;
            Upgrade.Set(persistentData.ThrusterUpgrade);
        }
        #endregion
        
        [System.Serializable]
        private class DefaultThrusters: IThrusterUpgrade {
            [SerializeField] private float _defaultThrustPower = 500f;
            [SerializeField] private float _defaultPowerConsumption = 0.1f;

            public float NormalThrustPower => _defaultThrustPower;
            public float NormalPowerConsumption => _defaultPowerConsumption;
            public float BoostThrustPower => _defaultThrustPower;
            public float BoostPowerConsumption => _defaultPowerConsumption;

        }
    }
}