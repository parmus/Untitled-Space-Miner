using SpaceGame.Core;
using SpaceGame.Utility;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Assertions;

namespace SpaceGame.AsteroidSystem.Editor
{
    [CustomEditor(typeof(AsteroidSpawner))]
    public class AsteroidSpawnerEditor : UnityEditor.Editor
    {
        private AsteroidSpawner _spawner;
       
        private void OnEnable()
        {
            _spawner = target as AsteroidSpawner;
            Assert.IsNotNull(_spawner);
        }

        public override void OnInspectorGUI(){
            DrawDefaultInspector();

            if (GUILayout.Button("Regenerate Asteroids")) {
                RegenerateAsteroids();
            }
            if (GUILayout.Button("Regenerate Resources")) {
                RegenerateResources();
            }
            if (GUILayout.Button("Clear Resources")) {
                ClearResources();
            }
            if (GUILayout.Button("Clear all")) {
                Clear();
            }
        }

        private void Clear() {
            _spawner.transform.DestroyAllChildrenImmediate();
            EditorSceneManager.MarkSceneDirty(_spawner.gameObject.scene);
        }

        private void ClearResources() {
            _spawner.transform.DestroyAllChildrenImmediate<ResourceDeposit>();
            EditorSceneManager.MarkSceneDirty(_spawner.gameObject.scene);
        }

        private void RegenerateAsteroids() {
            // Clean up
            Clear();

            // Reset randomness
            Random.InitState(_spawner.AsteroidRandomSeed);

            for (var i = 0; i < _spawner.AsteroidCount; i++){
                // Spawn asteroid
                var spawnPosition = RandomPlacement();
                if (!spawnPosition.HasValue) {
                    Debug.Log($"Skipping asteroid {i}");
                    continue;
                }

                var asteroid = PrefabUtility.InstantiatePrefab(_spawner.AsteroidPrefabs.PickRandom()) as GameObject;
                Assert.IsNotNull(asteroid);
                asteroid.transform.position = spawnPosition.Value;
                asteroid.transform.rotation = Random.rotation;
                asteroid.transform.parent = _spawner.transform;
                asteroid.name = $"Asteroid {i}";
            }
            EditorSceneManager.MarkSceneDirty(_spawner.gameObject.scene);
        }

        private void RegenerateResources() {
            // Clean up
            ClearResources();

            // Reset randomness
            Random.InitState(_spawner.ResourceRandomSeed);
            foreach(var asteroid in _spawner.GetComponentsInChildren<Asteroid>()) {
                // Spawn resources on the asteroid
                var maxAxis = asteroid.GetComponentInChildren<MeshFilter>().sharedMesh.bounds.extents.magnitude;
                for(var j = 0; j < _spawner.ResourceCount; j++) {
                    var point = Random.onUnitSphere;
                    Physics.Raycast(asteroid.transform.position + point * maxAxis, -point, out var hit, maxAxis);

                    var resource = PrefabUtility.InstantiatePrefab(_spawner.ResourceTypes.PickRandom()) as ResourceDeposit;
                    Assert.IsNotNull(resource);
                    resource.transform.SetPositionAndRotation(
                        hit.point,
                        Quaternion.LookRotation(Vector3.up, hit.normal)
                    );
                    resource.transform.SetParent(asteroid.transform);
                    resource.gameObject.name = $"Resource {j}";
                }
            }
            EditorSceneManager.MarkSceneDirty(_spawner.gameObject.scene);
        }

        private Vector3? RandomPlacement() {
            var overlaps = new Collider[2];
            for (var i = 0; i < _spawner.MaxPlacementAttempts; i++) {
                var spawnPosition = _spawner.transform.position + Random.insideUnitSphere *  _spawner.Radius;

                var discard = false;
                var hits = Physics.OverlapSphereNonAlloc(spawnPosition, _spawner.MinDistance, overlaps);
                for(var x = 0; x < hits; x++)
                {
                    if (overlaps[x].gameObject.transform.parent != _spawner.transform) continue;
                    discard = true;
                    break;
                }
                if (!discard) return spawnPosition;
            }
            return null;
        }
    }
}
