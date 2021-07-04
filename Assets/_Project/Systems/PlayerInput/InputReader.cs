using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceGame.PlayerInput
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "InputReader", order = 0)]
    public class InputReader : ScriptableObject, Controls.IFlightActions, Controls.IUIActions, Controls.IRobotActions 
    {
        
        #region Delegates
        public event Action<bool> OnFlightBoost;
        public event Action<bool> OnFlightFire;
        public event Action<Vector2> OnFlightThrust;
        public event Action<Vector2> OnFlightLook;

        public event Action OnOpenInventory;
        public event Action OnCloseInventory;
        
        public event Action<bool> OnRobotRun;
        public event Action OnRobotInteract;
        public event Action<Vector2> OnRobotMovement;
        public event Action<Vector2> OnRobotLook;
        #endregion

        
        private Controls _controls;

        #region 
        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.Flight.SetCallbacks(this);
                _controls.Robot.SetCallbacks(this);
                _controls.UI.SetCallbacks(this);
            }
        }

        private void OnDisable() => DisableAll();

        public void EnableFlight()
        {
            _controls.Robot.Disable();
            _controls.UI.Disable();
            _controls.Flight.Enable();
            SetCursorEnabled(false);
        }

        public void EnableRobot()
        {
            _controls.Flight.Disable();
            _controls.UI.Disable();
            _controls.Robot.Enable();
            SetCursorEnabled(false);
        }

        public void EnableUI()
        {
            _controls.Robot.Disable();
            _controls.Flight.Disable();
            _controls.UI.Enable();
            SetCursorEnabled(true);
        }

        public void DisableAll()
        {
            _controls.Flight.Disable();
            _controls.UI.Disable();
            _controls.Robot.Disable();
            SetCursorEnabled(true);
        }
        #endregion

        #region Flight Callbacks
        void Controls.IFlightActions.OnLookAround(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    OnFlightLook?.Invoke(context.ReadValue<Vector2>());
                    break;
                case InputActionPhase.Canceled:
                    OnFlightLook?.Invoke(Vector2.zero);
                    break;
            }
        }

        void Controls.IFlightActions.OnThrust(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    OnFlightThrust?.Invoke(context.ReadValue<Vector2>());
                    break;
                case InputActionPhase.Canceled:
                    OnFlightThrust?.Invoke(Vector2.zero);
                    break;
            }
        }

        void Controls.IFlightActions.OnFire(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    OnFlightFire?.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    OnFlightFire?.Invoke(false);
                    break;
            }
        }

        void Controls.IFlightActions.OnBoost(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    OnFlightBoost?.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    OnFlightBoost?.Invoke(false);
                    break;
            }
        }

        void Controls.IFlightActions.OnOpenInventory(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed) OnOpenInventory?.Invoke();
        }

        #endregion

        #region Robot Callbacks
        void Controls.IRobotActions.OnMovement(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    OnRobotMovement?.Invoke(context.ReadValue<Vector2>());
                    break;
                case InputActionPhase.Canceled:
                    OnRobotMovement?.Invoke(Vector2.zero);
                    break;
            }
        }

        void Controls.IRobotActions.OnLookAround(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    OnRobotLook?.Invoke(context.ReadValue<Vector2>());
                    break;
                case InputActionPhase.Canceled:
                    OnRobotLook?.Invoke(Vector2.zero);
                    break;
            }
        }

        void Controls.IRobotActions.OnRun(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    OnRobotRun?.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    OnRobotRun?.Invoke(false);
                    break;
            }
        }

        void Controls.IRobotActions.OnInteract(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed) OnRobotInteract?.Invoke();
        }

        #endregion


        #region UI Callbacks
        void Controls.IUIActions.OnClose(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed) OnCloseInventory?.Invoke();
        }
        #endregion
        
        private static void SetCursorEnabled(bool enabled) {
            Cursor.lockState = enabled ? CursorLockMode.Confined : CursorLockMode.Locked;
            Cursor.visible = enabled;
        }
    }
}
