using System;
using UnityEngine;

namespace SpaceGame.ShuttleSystems.ResourceScanner
{
    public class ResourceScannerItem: IResourceScannerItem {
        public ResourceDeposit ResourceDeposit { get; }
        public Transform Transform => ResourceDeposit.transform;
        public float Distance { get; private set; }
        public bool InRange => Distance <= _scanner.Range;
        public event Action OnDestroy;

        private readonly ResourceScanner _scanner;

        public ResourceScannerItem(ResourceScanner scanner, ResourceDeposit resourceDeposit) {
            _scanner = scanner;
            ResourceDeposit = resourceDeposit;
            UpdateDistance();
        }

        public void Destroy() => OnDestroy?.Invoke();

        public void UpdateDistance() {
            Distance = Vector3.Distance(Transform.position, _scanner.Origin.position);
        }
    }
}