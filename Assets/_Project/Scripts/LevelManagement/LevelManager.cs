using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;


#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif


namespace SpaceGame.LevelManagement
{
    [CreateAssetMenu(menuName = "Levels/Level Manager")]
    public class LevelManager : BetterScriptableObject
    {
        [InlineButton("LoadGlobalScene", "Load")]
        [InlineProperty]
        [SerializeField] private SceneAsset _globalScene = new SceneAsset();
        
        [SerializeField] private SceneFader _faderPrefab; 
        private SceneFader _fader;
        private bool _isLoading;

        protected override void OnInitialize()
        {
            if (_faderPrefab != null)
            {
                _fader = Instantiate(_faderPrefab);
                DontDestroyOnLoad(_fader);
            }

            if (SceneManager.GetSceneByBuildIndex(_globalScene.BuildIndex).isLoaded) return;
            SceneManager.LoadSceneAsync(_globalScene.BuildIndex, LoadSceneMode.Additive);
        }

        public void LoadLevel(Level level)
        {
            if (_isLoading) return;
            _isLoading = true;
            CoroutineRunner.Start_Coroutine(CO_LoadLevel(level));
        }

        private IEnumerator CO_LoadLevel(Level level)
        {
            var fadeout = (_fader != null) ? _fader.FadeOut() : null;

            var loader = new LoadGroup(level.Scenes, level.ActiveScene);

            yield return fadeout;
            yield return loader.WaitForLoaded();
            loader.ActivateAll();

            yield return loader.WaitForCompleted();
            var unloader = new UnloadGroup(Enumerable.Range(0, SceneManager.sceneCount)
                .Select(SceneManager.GetSceneAt)
                .Where(scene => level.Scenes.All(x => x.ScenePath != scene.path))
                .Where(scene => _globalScene.BuildIndex != scene.buildIndex)); 
            yield return unloader.WaitForCompleted();
        
            if (_fader != null) yield return _fader.FadeIn();
            _isLoading = false;
        }

        public abstract class SceneFader: MonoBehaviour
        {
            public abstract IEnumerator FadeOut();
            public abstract IEnumerator FadeIn();
        }
        
#if UNITY_EDITOR
        private void LoadGlobalScene()
        {
            EditorSceneManager.OpenScene(_globalScene.ScenePath, OpenSceneMode.Additive);
        }
#endif

        private class UnloadGroup
        {
            private readonly List<IEnumerator> _operations = new List<IEnumerator>();

            public UnloadGroup(IEnumerable<Scene> scenes)
            {
                foreach (var scene in scenes)
                {
                    _operations.Add(DelayedUnload(scene));
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
                    var ao = SceneManager.LoadSceneAsync(sceneAsset.BuildIndex, LoadSceneMode.Additive);
                    ao.allowSceneActivation = false;
                    _operations.Add(ao);
                }
            }
            
            public IEnumerator WaitForCompleted()
            {
                yield return _operations.GetEnumerator();
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(_activeScene.BuildIndex));
            }

            public IEnumerator WaitForLoaded()
            {
                while (_operations.Any(operation => operation.progress < 0.9f))
                {
                    yield return null;
                }
            }

            public void ActivateAll()
            {
                foreach (var operation in _operations)
                {
                    operation.allowSceneActivation = true;
                }
            }
        }
    }
}
