using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

namespace SpaceGame
{
    public static class SceneLoader
    {
        public static int[] CaptureLoadedScenes()
        {
            var loadedSceneIds = new int[SceneManager.sceneCount];
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                loadedSceneIds[i] = SceneManager.GetSceneAt(i).buildIndex;
            }
            return loadedSceneIds;
        }

        public static IEnumerator RestoreScenes(int[] sceneIds)
        {
            var loadedScenes = new List<Scene>(SceneManager.sceneCount);
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                loadedScenes.Add(SceneManager.GetSceneAt(i));
            }

            foreach (var sceneBuildIndex in sceneIds)
            {
                if (loadedScenes.Any(scene => scene.buildIndex == sceneBuildIndex)) continue;
                yield return SceneManager.LoadSceneAsync(sceneBuildIndex, LoadSceneMode.Additive);
            }

            foreach (var scene in loadedScenes)
            {
                if (sceneIds.Contains(scene.buildIndex)) continue;
                yield return SceneManager.UnloadSceneAsync(scene.buildIndex);
            }
        }
    }
}