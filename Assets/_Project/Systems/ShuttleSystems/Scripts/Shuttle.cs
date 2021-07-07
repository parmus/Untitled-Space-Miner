using SpaceGame.PlayerInput;
using SpaceGame.ShuttleSystems.ShuttleStates;
using SpaceGame.Utility;
using SpaceGame.Utility.SaveSystem;
using UnityEngine;

namespace SpaceGame.ShuttleSystems {
    [AddComponentMenu("Shuttle Systems/Shuttle")]
    [RequireComponent(typeof(CameraControl))]
    [RequireComponent(typeof(Hull.Hull))]
    [RequireComponent(typeof(InertiaDampers.InertiaDampers))]
    [RequireComponent(typeof(MiningTool.MiningTool))]
    [RequireComponent(typeof(PowerSystem.PowerSystem))]
    [RequireComponent(typeof(ResourceScanner.ResourceScanner))]
    [RequireComponent(typeof(Storage.Storage))]
    [RequireComponent(typeof(Thrusters.Thrusters))]
    public class Shuttle : MonoBehaviour, IPersistable {
        [SerializeField] private InputReader _inputReader;
        public InputReader InputReader => _inputReader;
        
        #region Shuttle Components
        public CameraControl CameraControl { get; private set; }
        public Hull.Hull Hull { get; private set; }
        public InertiaDampers.InertiaDampers InertiaDampers { get; private set; }
        public MiningTool.MiningTool MiningTool { get; private set; }
        public PowerSystem.PowerSystem PowerSystem { get; private set; }
        public ResourceScanner.ResourceScanner ResourceScanner { get; private set; }
        public Storage.Storage Storage { get; private set; }
        public Thrusters.Thrusters Thrusters { get; private set; }
        #endregion

        public PositionRotation LandingPad { get; private set; } = null;
        public IReadonlyObservable<ShuttleStateMachine.State> CurrentState => _shuttleStateMachine.CurrentState;
        private ShuttleStateMachine _shuttleStateMachine;

        public void Land(Transform landingPad)
        {
            LandingPad = PositionRotation.FromTransform(landingPad);
            _shuttleStateMachine.SetState<LandingState>();
        }

        private void Awake() {
            CameraControl = GetComponent<CameraControl>();
            Hull = GetComponent<Hull.Hull>();
            InertiaDampers = GetComponent<InertiaDampers.InertiaDampers>();
            MiningTool = GetComponent<MiningTool.MiningTool>();
            PowerSystem = GetComponent<PowerSystem.PowerSystem>();
            ResourceScanner = GetComponent<ResourceScanner.ResourceScanner>();
            Storage = GetComponent<Storage.Storage>();
            Thrusters = GetComponent<Thrusters.Thrusters>();

            _shuttleStateMachine = new ShuttleStateMachine(this);
            Hull.OnDie += () => _shuttleStateMachine.SetState<ShutdownState>();
        }

        private void OnDestroy() => _shuttleStateMachine = null;
        
        #region IPersistable
        [System.Serializable]
        public class PersistentData
        {
            public readonly object State;
            public readonly PositionRotation PositionRotation;
            public readonly PositionRotation LandingPad;

            public PersistentData(object state, Transform transform, PositionRotation landingPad)
            {
                State = state;
                PositionRotation = PositionRotation.FromTransform(transform);
                LandingPad = landingPad;
            }
        }

        public object CaptureState() => new PersistentData(_shuttleStateMachine.CaptureState(), transform, LandingPad);

        public void RestoreState(object state)
        {
            var rigidBody = GetComponent<Rigidbody>();
            var persistentData = (PersistentData) state;
            persistentData.PositionRotation.SetTransform(transform);
            LandingPad = persistentData.LandingPad;
            rigidBody.velocity = Vector3.zero;
            rigidBody.angularVelocity = Vector3.zero;
            
            _shuttleStateMachine.RestoreState(persistentData.State);
        }
        #endregion
        
    }
}
