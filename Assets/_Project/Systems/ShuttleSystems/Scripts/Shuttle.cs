using SpaceGame.Utility;
using UnityEngine;

namespace SpaceGame.ShuttleSystems {
    [AddComponentMenu("Shuttle Systems/Shuttle")]
    [RequireComponent(typeof(CameraControl))]
    [RequireComponent(typeof(Hull.Hull))]
    [RequireComponent(typeof(InertiaDampers.InertiaDampers))]
    [RequireComponent(typeof(MiningTool.MiningTool))]
    [RequireComponent(typeof(PowerSystem.PowerSystem))]
    [RequireComponent(typeof(ResourceScanner.ResourceScanner))]
    [RequireComponent(typeof(ShuttleControls))]
    [RequireComponent(typeof(Storage.Storage))]
    [RequireComponent(typeof(Thrusters.Thrusters))]
    public class Shuttle : MonoBehaviour, IPersistable {
        #region Shuttle Components
        public CameraControl CameraControl { get; private set; }
        public Hull.Hull Hull { get; private set; }
        public InertiaDampers.InertiaDampers InertiaDampers { get; private set; }
        public MiningTool.MiningTool MiningTool { get; private set; }
        public PowerSystem.PowerSystem PowerSystem { get; private set; }
        public ResourceScanner.ResourceScanner ResourceScanner { get; private set; }
        public ShuttleControls ShuttleControls { get; private set; }
        public Storage.Storage Storage { get; private set; }
        public Thrusters.Thrusters Thrusters { get; private set; }
        #endregion


        public Transform LandingPad { get; set; } = null;
        public IReadonlyObservable<ShuttleStates.FSM.State> CurrentState => _fsm.CurrentState;

        private ShuttleStates.FSM _fsm;

        private void Awake() {
            CameraControl = GetComponent<CameraControl>();
            Hull = GetComponent<Hull.Hull>();
            InertiaDampers = GetComponent<InertiaDampers.InertiaDampers>();
            MiningTool = GetComponent<MiningTool.MiningTool>();
            PowerSystem = GetComponent<PowerSystem.PowerSystem>();
            ResourceScanner = GetComponent<ResourceScanner.ResourceScanner>();
            ShuttleControls = GetComponent<ShuttleControls>();
            Storage = GetComponent<Storage.Storage>();
            Thrusters = GetComponent<Thrusters.Thrusters>();

            _fsm = new ShuttleStates.FSM(this);
        }

        private void Update() => _fsm.Tick();
        
        
        
        #region IPersistable
        [System.Serializable]
        public class PersistentData
        {
            public readonly Vector3 Position;
            public readonly Quaternion Rotation;

            public PersistentData(Transform transform)
            {
                Position = transform.position;
                Rotation = transform.rotation;
            }
        }

        public object CaptureState() => new PersistentData(transform);

        public void RestoreState(object state)
        {
            var persistentData = (PersistentData) state;
            var t = transform;
            t.position = persistentData.Position;
            t.rotation = persistentData.Rotation;
            var rigidBody = GetComponent<Rigidbody>();
            rigidBody.velocity = Vector3.zero;
            rigidBody.angularVelocity = Vector3.zero;
        }
        #endregion
        
    }
}