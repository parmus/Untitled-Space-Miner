using System;
using System.Collections;
using UnityEngine;

namespace SpaceGame.ShuttleSystems {
    [AddComponentMenu("Shuttle Systems/Shuttle Spawner")]
    public class ShuttleSpawner : MonoBehaviour {
        [SerializeField] private Shuttle _shuttlePrefab = default;
        [SerializeField] private float _respawnDelay = 3.0f;

        public static event Action<Shuttle> OnNewShuttleSpawned;
        public static Shuttle CurrentShuttle { get; private set; }
        
        private Transform _spawnPoint;

        private void Awake() => _spawnPoint = transform;

        private void Start() => SpawnNewShuttle();

        private void SpawnNewShuttle()
        {
            var shuttle = SpawnNewShuttle(_spawnPoint.position, _spawnPoint.rotation);
            shuttle.CurrentState.OnChange += state => {
                if (state is ShuttleStates.ShutdownState) StartCoroutine(_onShuttleLost());
            };
        }

        private IEnumerator _onShuttleLost() {
            ShuttleConfigurationManager.Clear();
            yield return new WaitForSeconds(_respawnDelay);
            SpawnNewShuttle();
        }

        private Shuttle SpawnNewShuttle(Vector3 position, Quaternion rotation) {
            CurrentShuttle = Instantiate(_shuttlePrefab, position, rotation);
            OnNewShuttleSpawned?.Invoke(CurrentShuttle);

            return CurrentShuttle;
        }
        
    }
}