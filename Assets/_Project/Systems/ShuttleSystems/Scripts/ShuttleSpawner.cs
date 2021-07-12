using System;
using System.Collections;
using SpaceGame.PlayerInput;
using SpaceGame.Utility.GameEvents;
using UnityEngine;

namespace SpaceGame.ShuttleSystems {
    [AddComponentMenu("Shuttle Systems/Shuttle Spawner")]
    public class ShuttleSpawner : MonoBehaviour {
        [SerializeField] private Shuttle _shuttlePrefab;
        [SerializeField] private float _respawnDelay = 3.0f;
        [SerializeField] private InputReader _inputReader;
        [SerializeField] private ShuttleAnchor _shuttleAnchor;
        [SerializeField] private GameEvent _locationReadyEvent;
        
        private Transform _spawnPoint;

        private void Awake() => _spawnPoint = transform;

        private void Start() => _locationReadyEvent.OnEvent += SpawnNewShuttle;

        private void OnDestroy() => _locationReadyEvent.OnEvent -= SpawnNewShuttle;


        private void SpawnNewShuttle()
        {
            var shuttle = SpawnNewShuttle(_spawnPoint.position, _spawnPoint.rotation);
            shuttle.CurrentState.OnChange += state => {
                if (state is ShuttleStates.ShutdownState) StartCoroutine(_onShuttleLost());
            };
            _inputReader.EnableFlight();
        }

        private IEnumerator _onShuttleLost() {
            yield return new WaitForSeconds(_respawnDelay);
            SpawnNewShuttle();
        }

        private Shuttle SpawnNewShuttle(Vector3 position, Quaternion rotation) {
            var newShuttle = Instantiate(_shuttlePrefab, position, rotation);
            _shuttleAnchor.Set(newShuttle);

            return newShuttle;
        }

        private void OnEnable() => _inputReader.OnCloseInventory += OnCloseInventory;

        private void OnDisable() => _inputReader.OnCloseInventory -= OnCloseInventory;

        private void OnCloseInventory() => _inputReader.EnableFlight();
    }
}
