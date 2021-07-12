using UnityEngine;
using UnityEngine.InputSystem;
#if UNITY_EDITOR
using UnityEditor;

namespace SpaceGame.Util
{
    public class DebugBreak : MonoBehaviour
    {
        [SerializeField] private InputAction _shortcut;
        
        private void Awake() => _shortcut.performed += ctx => EditorApplication.isPaused = true;

        private void OnEnable() => _shortcut.Enable();

        private void OnDisable() => _shortcut.Disable();

        private void Reset()
        {
            var go = gameObject;
            go.name = GetType().Name;
            go.tag = "EditorOnly";
        }
        
        [MenuItem("GameObject/Grumpy Bear/Space Game/Debug Break", false, 10)]
        private static void CreateDebugBreak(MenuCommand menuCommand)
        {
            var go = new GameObject();
            go.AddComponent<DebugBreak>();
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }
    }
}
#endif
