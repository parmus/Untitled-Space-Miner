using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceGame.LevelManagement
{
    public class DebugLevelLoader : MonoBehaviour
    {
        [SerializeField] private LevelManager LevelManager;
        [SerializeField] private Level Level;
        [SerializeField] private InputAction Action;

        private void Awake() => Action.performed += OnActionOnperformed;

        private void OnActionOnperformed(InputAction.CallbackContext ctx)
        {
            if (ctx.phase != InputActionPhase.Performed) return;
            LevelManager.LoadLevel(Level);
        }

        private void OnEnable() => Action.Enable();

        private void OnDisable() => Action.Disable();
    }
}
