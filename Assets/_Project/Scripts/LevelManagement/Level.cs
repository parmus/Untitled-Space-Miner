using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using System.Linq;
using UnityEditor.SceneManagement;
#endif

namespace SpaceGame.LevelManagement
{
    [CreateAssetMenu(menuName = "Levels/Level")]
    public class Level : ScriptableObject
    {
        [SerializeField] private List<SceneAsset> _scenes;
        public IEnumerable<SceneAsset> Scenes => _scenes;
        public SceneAsset ActiveScene => _scenes[0];

#if UNITY_EDITOR
        [Button("Load scene")]
        private void LoadScene()
        {
            var openScenes = Enumerable.Range(0, EditorSceneManager.loadedSceneCount)
                .Select(EditorSceneManager.GetSceneAt)
                .Where(scene => _scenes.All(x => scene.buildIndex != x.BuildIndex))
                .ToArray();
            if (!EditorSceneManager.SaveModifiedScenesIfUserWantsTo(openScenes)) return;
            foreach (var sceneAsset in _scenes)
            {
                EditorSceneManager.OpenScene(sceneAsset.ScenePath, OpenSceneMode.Additive);
            }
            EditorSceneManager.SetActiveScene(EditorSceneManager.GetSceneByPath(ActiveScene.ScenePath));
            foreach (var openScene in openScenes)
            {
                EditorSceneManager.CloseScene(openScene, true);
            }
        }
#endif

    }
}
