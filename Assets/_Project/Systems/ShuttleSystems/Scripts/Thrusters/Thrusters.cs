using SpaceGame.Utility;
using UnityEngine;

namespace SpaceGame.ShuttleSystems.Thrusters {

    [AddComponentMenu("Shuttle Systems/Thrusters")]
    public class Thrusters : MonoBehaviour {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private DefaultThrusters _defaultThrusters = new DefaultThrusters();


        #region Properties
        public ThrusterUpgrade Upgrade { get; set; } = default;
        public IReadonlyObservable<int> Velocity => _velocity;
        #endregion


        #region Private properties
        private IThrusterUpgrade CurrentThruster => (IThrusterUpgrade) Upgrade ?? _defaultThrusters;
        private bool Boost => _shuttle.ShuttleControls.Boost;
        private float ThrustPower => Boost ? CurrentThruster.BoostThrustPower : CurrentThruster.NormalThrustPower;
        private float PowerConsumption => Boost ? CurrentThruster.BoostPowerConsumption : CurrentThruster.NormalPowerConsumption;
        
        private readonly Observable<int> _velocity = new Observable<int>();

        #endregion

        #region Private variables
        private Vector3 _thrustDirection = Vector3.zero;
        private Shuttle _shuttle;
        #endregion

        private void Awake() {
            _shuttle = GetComponent<Shuttle>();
        }

        private void FixedUpdate() {
            _thrustDirection.Set(_shuttle.ShuttleControls.Thrust.x, 0f, _shuttle.ShuttleControls.Thrust.y);

            if (_thrustDirection == Vector3.zero) return;
            if (_shuttle.PowerSystem.IsEmpty) return;

            _rigidbody.AddRelativeForce(_thrustDirection * (ThrustPower * Time.fixedDeltaTime));
            _shuttle.PowerSystem.Charge -= PowerConsumption * Time.fixedDeltaTime;
            
            _velocity.Value = Mathf.RoundToInt(_rigidbody.velocity.magnitude);
        }

        private void Reset() {
            _rigidbody = GetComponent<Rigidbody>();
        }

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