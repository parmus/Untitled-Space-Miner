using System.Collections.Generic;
using SpaceGame.InventorySystem;
using SpaceGame.InventorySystem.Utils;
using SpaceGame.Utility.SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceGame
{
    [System.Serializable]
    public class Session: IPersistable {
        [SerializeField] private Inventory _inventory = new Inventory(40, 10);
        public IInventory Inventory => _inventory;
            
        #region IPersistable
        [System.Serializable]
        public class PersistentData
        {
            public readonly SerializableInventory Inventory;
            public readonly List<int> Scenes = new List<int>();
            public readonly int ActiveScene;
                
            public void RestoreInventory(IInventory inventory) => Inventory.RestoreInventory(inventory);

            public void RestoreScenes()
            {
                foreach (var sceneBuildIndex in Scenes)
                {
                    if (!SceneIsLoaded(sceneBuildIndex)) SceneManager.LoadScene(sceneBuildIndex);
                }

                for (var i = 0; i < SceneManager.sceneCount; i++)
                {
                    var scene = SceneManager.GetSceneAt(i);
                    if (Scenes.Contains(scene.buildIndex))
                    {
                        if (scene.buildIndex == ActiveScene) SceneManager.SetActiveScene(scene);
                    }
                    else
                    {
                        SceneManager.UnloadSceneAsync(scene.buildIndex);
                    }
                }
            }

            private static bool SceneIsLoaded(int buildIndex)
            {
                for (var i = 0; i < SceneManager.sceneCount; i++)
                {
                    if (SceneManager.GetSceneAt(i).buildIndex == buildIndex) return true;
                }
                return false;
            }

            public PersistentData(IInventory inventory)
            {
                Inventory = new SerializableInventory(inventory);
                ActiveScene = SceneManager.GetActiveScene().buildIndex;

                for (var i = 0; i < SceneManager.sceneCount; i++)
                {
                    var scene = SceneManager.GetSceneAt(i);
                    if (scene.isLoaded) Scenes.Add(scene.buildIndex);
                }
            }
        }

        public object CaptureState() => new PersistentData(Inventory);

        public void RestoreState(object state)
        {
            var persistentData = (PersistentData) state;
            persistentData.RestoreInventory(Inventory);
            persistentData.RestoreScenes();
        }
        #endregion
    }
}