using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace SpaceGame.LevelManagement
{
    public class DebugLocationLoader : MonoBehaviour
    {
        [SerializeField] private LocationEvent _loadLocationRequest;
        [SerializeField] private Location _location;
        [SerializeField] private InputAction _action;

        private void Awake() => _action.performed += OnActionPerformed;

        private void OnActionPerformed(InputAction.CallbackContext ctx)
        {
            if (ctx.phase != InputActionPhase.Performed) return;
            Assert.IsNotNull(_loadLocationRequest);
            _loadLocationRequest.Broadcast(_location);
        }

        private void OnEnable() => _action.Enable();

        private void OnDisable() => _action.Disable();
    }
}
