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
        #endregion

        #region Private variables
        private Shuttle _shuttle;
        private float _thrustPower;
        private float _powerConsumption;
        private IThrusterUpgrade _currentThruster;
        private Vector3 _thrustDirection = Vector3.zero;
        private readonly Observable<int> _velocity = new Observable<int>();
        #endregion

        private void OnFlightBoost(bool value)
        {
            _thrustPower = value ? _currentThruster.BoostThrustPower : _currentThruster.NormalThrustPower;
            _powerConsumption = value ? _currentThruster.BoostPowerConsumption : _currentThruster.NormalPowerConsumption;
        }

        private void OnFlightThrust(Vector2 direction) => _thrustDirection.Set(direction.x, 0f, direction.y);

        private void Awake() => _shuttle = GetComponent<Shuttle>();
        private void OnEnable()
        {
            Upgrade.Subscribe(OnUpgradeChange);
            OnFlightBoost(false);
            _shuttle.InputReader.OnFlightBoost += OnFlightBoost;
            _shuttle.InputReader.OnFlightThrust += OnFlightThrust;
        }

        private void OnDisable()
        {
            _shuttle.InputReader.OnFlightThrust -= OnFlightThrust;
            _shuttle.InputReader.OnFlightBoost -= OnFlightBoost;
            OnFlightThrust(Vector2.zero);
            OnFlightBoost(false);
            
            Upgrade.Unsubscribe(OnUpgradeChange);
            _velocity.Value = 0;
        }

        private void OnUpgradeChange(ThrusterUpgrade upgrade) => _currentThruster = upgrade ? (IThrusterUpgrade) upgrade : _defaultThrusters;

        private void FixedUpdate() {
            _velocity.Value = Mathf.RoundToInt(_rigidbody.velocity.magnitude);

            if (_thrustDirection == Vector3.zero) return;
            if (_shuttle.PowerSystem.IsEmpty) return;

            _rigidbody.AddRelativeForce(_thrustDirection * (_thrustPower * Time.fixedDeltaTime));
            _shuttle.PowerSystem.Consume(_powerConsumption * Time.fixedDeltaTime);
        }

        private void Reset() => _rigidbody = GetComponent<Rigidbody>();

        #region IPersistable
        [System.Serializable]
        public class PersistentData: ShuttleUpgrade.PersistentData<ThrusterUpgrade>
        {
            public PersistentData(ThrusterUpgrade thrusterUpgrade) : base(thrusterUpgrade) { }
        }

        public object CaptureState() => new PersistentData(Upgrade.Value);

        public void RestoreState(object state)
        {
            var persistentData = (PersistentData) state;
            Upgrade.Set(persistentData.Upgrade);
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
