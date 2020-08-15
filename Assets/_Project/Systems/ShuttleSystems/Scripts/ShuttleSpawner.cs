using System.Collections;
using UnityEngine;

namespace SpaceGame.ShuttleSystems {
    [AddComponentMenu("Shuttle Systems/Shuttle Spawner")]
    public class ShuttleSpawner : MonoBehaviour {
        [SerializeField] private float _respawnDelay = 3.0f;

        private Transform _spawnPoint;

        private void Awake() => _spawnPoint = transform;

        private void Start() => SpawnNewShuttle();

        private void SpawnNewShuttle()
        {
            var shuttle = ShuttleConfigurationManager.Instance.SpawnNewShuttle(_spawnPoint.position, _spawnPoint.rotation);
            shuttle.CurrentState.OnChange += state => {
                if (state is ShuttleStates.ShutdownState) StartCoroutine(_onShuttleLost());
            };
        }

        private IEnumerator _onShuttleLost() {
            ShuttleConfigurationManager.Clear();
            yield return new WaitForSeconds(_respawnDelay);
            SpawnNewShuttle();
        }
    }
}