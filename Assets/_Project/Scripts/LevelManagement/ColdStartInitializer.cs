using Sirenix.OdinInspector;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace SpaceGame.LevelManagement
{
    public class ColdStartInitializer : MonoBehaviour
    {
        [InlineButton("LoadGlobalScene", "Load")]
        [InlineProperty]
        [SerializeField] private SceneAsset _globalScene = new SceneAsset();
        [SerializeField] private LocationEvent _loadLocationRequest;
        [SerializeField] private Location _thisLocation; 

#if UNITY_EDITOR
        private void LoadGlobalScene() => EditorSceneManager.OpenScene(_globalScene.ScenePath, OpenSceneMode.Additive);
#endif

        private void Start()
        {
            if (_globalScene.Scene.isLoaded) {
                _loadLocationRequest.Broadcast(_thisLocation);
            } else {
                SceneManager.LoadSceneAsync(_globalScene.ScenePath, LoadSceneMode.Additive).completed +=
                    operation => _loadLocationRequest.Broadcast(_thisLocation);
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
