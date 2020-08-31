using UnityEngine;

namespace SpaceGame.ShuttleSystems.ResourceScanner {
    [CreateAssetMenu(fileName = "New Resource Scanner Configuration", menuName = "Game Data/Resource Scanner Configuration", order = 1)]
    public class Configuration : ShuttleUpgrade {
        [SerializeField] private float _range = 100f;

        public override string Name => name;
        public override string Description => $"{base.Description}\n<#00ff00>• Range: {_range}m</color>";
        public float Range => _range;
    }
}