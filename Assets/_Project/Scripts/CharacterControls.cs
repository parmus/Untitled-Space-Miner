using SpaceGame.PlayerInput;
using UnityEngine;

namespace SpaceGame
{
    public class CharacterControls : MonoBehaviour, ICharacterControls
    {
        #region Properties
        public bool InRunning { get; private set; } = false;
        public Vector2 Movement { get; private set; } = Vector2.zero;
        public Vector2 OnLook { get; private set; } = Vector2.zero;
        #endregion


        #region Setup and tear down
        private void Awake() {
            _controls = new Controls();
            _controls.Robot.Run.performed += ctx => InRunning = true;
            _controls.Robot.Run.canceled += ctx => InRunning = false;
            _controls.Robot.LookAround.performed += ctx => OnLook = ctx.ReadValue<Vector2>();
            _controls.Robot.LookAround.canceled += ctx => OnLook = Vector2.zero;
            _controls.Robot.Movement.performed += ctx => Movement = ctx.ReadValue<Vector2>();
            _controls.Robot.Movement.canceled += ctx => Movement = Vector2.zero;
        }

        private void OnEnable()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            _controls.Robot.Enable();
        }

        private void OnDisable()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            _controls.Robot.Disable();
            InRunning = false;
            Movement = Vector2.zero;
            OnLook = Vector2.zero;
        }
        #endregion

        private Controls _controls;
        
    }
}