using System.Collections.Generic;
using SpaceGame.Core;
using UnityEngine;

namespace SpaceGame.AsteroidSystem
{
    public class AsteroidSpawner : MonoBehaviour {
        [Header("Spawn Area")]
        [SerializeField] private float _radius = 5;

        [Header("Asteroid spawning")]
        [SerializeField] private List<GameObject> _asteroidPrefabs = new List<GameObject>();
        [SerializeField][Range(1, 100000)] private int _asteroidCount = 10;
        [SerializeField] private float _minDistance = 2f;
        [SerializeField] private int _asteroidRandomSeed = 0;

        [Header("Resource spawning")]
        [SerializeField] private List<ResourceDeposit> _resourceTypes = new List<ResourceDeposit>();
        [SerializeField] private int _resourceRandomSeed = 0;

        [SerializeField] [Range(1, 100)] private int _resourceCount = 10;

        [Header("Advanced")]
        [SerializeField] private int _maxPlacementAttempts = 10;


        #region Properties
        public float Radius => _radius;
        public IReadOnlyList<GameObject> AsteroidPrefabs => _asteroidPrefabs;
        public int AsteroidCount => _asteroidCount;
        public float MinDistance => _minDistance;
        public int AsteroidRandomSeed => _asteroidRandomSeed;
        public IReadOnlyList<ResourceDeposit> ResourceTypes => _resourceTypes;
        public int ResourceRandomSeed => _resourceRandomSeed;
        public int ResourceCount => _resourceCount;
        public int MaxPlacementAttempts => _maxPlacementAttempts;
        #endregion
        
#if UNITY_EDITOR
        private void OnDrawGizmosSelected() {
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
#endif
    }
}