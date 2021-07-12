using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace SpaceGame.LevelManagement
{
    public class LocationManager : MonoBehaviour
    {
        [SerializeField] private LocationEvent _loadLocationRequest;
        
        [Header("Fading")]
        [SerializeField] private FadeRequest _fadeRequest;
        [SerializeField] private float _fadeOutDuration;
        [SerializeField] private float _fadeInDuration;
        
        
        private bool _isLoading;
        private Location _currentLocation;

        private void OnEnable() => _loadLocationRequest.OnEvent += LoadLocation;

        private void OnDisable() => _loadLocationRequest.OnEvent -= LoadLocation;

        private void LoadLocation(Location location)
        {
            if (_isLoading) return;
            _isLoading = true;
            StartCoroutine(CO_LoadLevel(location));
        }

        private IEnumerator CO_LoadLevel(Location location)
        {
            if (_fadeRequest != null)
            {
                _fadeRequest.FadeOut(_fadeOutDuration);
                yield return new WaitForSeconds(_fadeOutDuration);
            }

            var levelsToLoad = _currentLocation != null
                ? location.Scenes
                : location.Scenes.Where(sceneAsset => !sceneAsset.Scene.isLoaded);

            yield return new LoadGroup(levelsToLoad, location.ActiveScene).WaitForCompleted();
            if (_currentLocation != null)
                yield return new UnloadGroup(_currentLocation.Scenes).WaitForCompleted();
            
            _currentLocation = location;

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
            go.AddComponent<LocationManager>();
            UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            UnityEditor.Selection.activeObject = go;
        }
        #endif

    }
}
