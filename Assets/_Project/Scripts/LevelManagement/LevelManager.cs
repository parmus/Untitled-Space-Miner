using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SpaceGame.Utility.GameEvents;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace SpaceGame.LevelManagement
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelEvent _loadLevelRequest;
        
        [Header("Fading")]
        [SerializeField] private FadeRequest _fadeRequest;
        [SerializeField] private float _fadeOutDuration;
        [SerializeField] private float _fadeInDuration;
        
        
        private bool _isLoading;
        private Level _currentLevel;

        private void OnEnable() => _loadLevelRequest.OnEvent += LoadLevel;

        private void OnDisable() => _loadLevelRequest.OnEvent -= LoadLevel;

        private void LoadLevel(Level level)
        {
            if (_isLoading) return;
            _isLoading = true;
            StartCoroutine(CO_LoadLevel(level));
        }

        private IEnumerator CO_LoadLevel(Level level)
        {
            if (_fadeRequest != null)
            {
                _fadeRequest.FadeOut(_fadeOutDuration);
                yield return new WaitForSeconds(_fadeOutDuration);
            }

            var levelsToLoad = _currentLevel != null
                ? level.Scenes
                : level.Scenes.Where(sceneAsset => !sceneAsset.Scene.isLoaded);

            yield return new LoadGroup(levelsToLoad, level.ActiveScene).WaitForCompleted();
            if (_currentLevel != null)
                yield return new UnloadGroup(_currentLevel.Scenes).WaitForCompleted();
            
            _currentLevel = level;

            if (_fadeRequest != null)
            {
                _fadeRequest.FadeIn(_fadeInDuration);
                yield return new WaitForSeconds(_fadeInDuration);
            }
            _isLoading = false;
        }

        private class UnloadGroup
        {
            private readonly List<IEnumerator> _operations = new List<IEnumerator>();

            public UnloadGroup(IEnumerable<SceneAsset> sceneAssets)
            {
                foreach (var sceneAsset in sceneAssets)
                {
                    _operations.Add(DelayedUnload(sceneAsset.Scene));
                }
            }

            private static IEnumerator DelayedUnload(Scene scene)
            {
                while (SceneManager.sceneCount < 2)
                {
                    Debug.Log($"{scene.name} waiting to unload");
                    yield return null;
                }

                yield return SceneManager.UnloadSceneAsync(scene);
            }
            public IEnumerator WaitForCompleted() => _operations.GetEnumerator();

        }

        private class LoadGroup
        {
            private readonly SceneAsset _activeScene;
            private readonly List<AsyncOperation> _operations = new List<AsyncOperation>();

            public LoadGroup(IEnumerable<SceneAsset> sceneAssets, SceneAsset activeScene)
            {
                _activeScene = activeScene;
                foreach (var sceneAsset in sceneAssets)
                {
                    _operations.Add(SceneManager.LoadSceneAsync(sceneAsset.BuildIndex, LoadSceneMode.Additive));
                }
            }
            
            public IEnumerator WaitForCompleted()
            {
                yield return _operations.GetEnumerator();
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(_activeScene.BuildIndex));
            }
        }
        
        #if UNITY_EDITOR
        [UnityEditor.MenuItem("GameObject/Level Manager", false, 10)]
        private static void CreateShuttleConfigurationInjector(UnityEditor.MenuCommand menuCommand)
        {
            var go = new GameObject("Level Manager");
            UnityEditor.GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            go.AddComponent<LevelManager>();
            UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            UnityEditor.Selection.activeObject = go;
        }
        #endif

    }
}
