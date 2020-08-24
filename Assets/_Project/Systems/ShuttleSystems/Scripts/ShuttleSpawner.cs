using System.Collections;
using SpaceGame.Utility;
using UnityEngine;

namespace SpaceGame.ShuttleSystems {
    [AddComponentMenu("Shuttle Systems/Shuttle Spawner")]
    public class ShuttleSpawner : MonoBehaviour {
        [SerializeField] private Shuttle _shuttlePrefab = default;
        [SerializeField] private float _respawnDelay = 3.0f;
        
        public static IReadonlyObservable<Shuttle> CurrentShuttle => _currentShuttle;
        private static readonly Observable<Shuttle> _currentShuttle = new Observable<Shuttle>();
        
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
            yield return new WaitForSeconds(_respawnDelay);
            SpawnNewShuttle();
        }

        private Shuttle SpawnNewShuttle(Vector3 position, Quaternion rotation) {
            var newShuttle = Instantiate(_shuttlePrefab, position, rotation);
            _currentShuttle.Value = newShuttle;

            return newShuttle;
        }
        
    }
}