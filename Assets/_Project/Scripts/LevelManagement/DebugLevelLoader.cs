using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace SpaceGame.LevelManagement
{
    public class DebugLevelLoader : MonoBehaviour
    {
        [SerializeField] private LevelEvent _loadLevelRequest;
        [FormerlySerializedAs("Level")] [SerializeField] private Level _level;
        [FormerlySerializedAs("Action")] [SerializeField] private InputAction _action;

        private void Awake() => _action.performed += OnActionPerformed;

        private void OnActionPerformed(InputAction.CallbackContext ctx)
        {
            if (ctx.phase != InputActionPhase.Performed) return;
            Assert.IsNotNull(_loadLevelRequest);
            _loadLevelRequest.Broadcast(_level);
        }

        private void OnEnable() => _action.Enable();

        private void OnDisable() => _action.Disable();
    }
}
