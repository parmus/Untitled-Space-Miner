using UnityEngine;

namespace SpaceGame.ShuttleSystems.PowerSystem
{
    [CreateAssetMenu(fileName = "New Power System Upgrade", menuName = "Game Data/Power System Upgrade", order = 1)]

    public class PowerSystemUpgrade: ShuttleUpgrade, IPowerSystemConfiguration
    {
        [SerializeField] private float _capacity = 100f;
        public override string Name => name;
        public override string Description => $"{base.Description}\n<#00ff00>• Capacity: {_capacity}</color>";
        public float Capacity => _capacity;
    }
}