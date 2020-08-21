using UnityEngine;

namespace SpaceGame.ShuttleSystems.PowerSystem
{
    [CreateAssetMenu(fileName = "New Power System Upgrade", menuName = "Game Data/Power System Upgrade", order = 1)]

    public class PowerSystemUpgrade: ShuttleUpgrade<PowerSystemUpgrade>, IPowerSystemConfiguration
    {
        [SerializeField] private float _capacity = 100f;
        public override string Name => name;
        public float Capacity => _capacity;
    }
}