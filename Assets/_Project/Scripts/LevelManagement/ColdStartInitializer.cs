using Sirenix.OdinInspector;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceGame.LevelManagement
{
    public class ColdStartInitializer : MonoBehaviour
    {
        [InlineButton("LoadGlobalScene", "Load")]
        [InlineProperty]
        [SerializeField] private SceneAsset _globalScene = new SceneAsset();
        [SerializeField] private LevelEvent _loadLevelRequest;
        [SerializeField] private Level _thisLevel; 

#if UNITY_EDITOR
        private void LoadGlobalScene() => EditorSceneManager.OpenScene(_globalScene.ScenePath, OpenSceneMode.Additive);
#endif

        private void Start()
        {
            if (_globalScene.Scene.isLoaded) {
                _loadLevelRequest.Broadcast(_thisLevel);
            } else {
                SceneManager.LoadSceneAsync(_globalScene.ScenePath, LoadSceneMode.Additive).completed +=
                    operation => _loadLevelRequest.Broadcast(_thisLevel);
            }
        }

#if UNITY_EDITOR
        private void Reset()
        {
            gameObject.name = GetType().Name;
            gameObject.tag = "EditorOnly";
        }
        
        [UnityEditor.MenuItem("GameObject/ColdStartInitializer", false, 10)]
        private static void CreateShuttleConfigurationInjector(UnityEditor.MenuCommand menuCommand)
        {
            var go = new GameObject();
            UnityEditor.GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            go.AddComponent<ColdStartInitializer>();
            UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            UnityEditor.Selection.activeObject = go;
        }
        #endif
    }
}
