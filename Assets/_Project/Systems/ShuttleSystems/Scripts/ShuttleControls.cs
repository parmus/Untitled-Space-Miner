using UnityEngine;

namespace SpaceGame.ShuttleSystems {
    public class ShuttleControls: MonoBehaviour {
        #region Properties
        public bool Boost { get; private set; } = false;
        public bool Fire { get; private set; } = false;
        public Vector2 Thrust { get; private set; } = Vector2.zero;
        public Vector2 OnLook { get; private set; } = Vector2.zero;
        #endregion


        #region Setup and tear down
        private void Awake() {
            _controls = new Controls();
            _controls.Flight.Boost.performed += ctx => Boost = true;
            _controls.Flight.Boost.canceled += ctx => Boost = false;
            _controls.Flight.Fire.performed += ctx => Fire = true;
            _controls.Flight.Fire.canceled += ctx => Fire = false;
            _controls.Flight.LookAround.performed += ctx => OnLook = ctx.ReadValue<Vector2>();
            _controls.Flight.LookAround.canceled += ctx => OnLook = Vector2.zero;
            _controls.Flight.Thrust.performed += ctx => Thrust = ctx.ReadValue<Vector2>();
            _controls.Flight.Thrust.canceled += ctx => Thrust = Vector2.zero;
        }

        private void OnEnable() {
            _controls.Flight.Enable();
        }

        private void OnDisable() {
            _controls.Flight.Disable();
            Boost = false;
            Fire = false;
            Thrust = Vector2.zero;
            OnLook = Vector2.zero;
        }
        #endregion

        private Controls _controls;
   }
}