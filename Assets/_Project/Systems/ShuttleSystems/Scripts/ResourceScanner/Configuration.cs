using UnityEngine;

namespace SpaceGame.ShuttleSystems.ResourceScanner {
    [CreateAssetMenu(fileName = "New Resource Scanner Configuration", menuName = "Game Data/Resource Scanner Configuration", order = 1)]
    public class Configuration : ShuttleUpgrade<Configuration> {
        [SerializeField] private float _range = 100f;

        public override string Name => name;
        public float Range => _range;
    }
}