using SpaceGame.ShuttleSystems.MiningTool.VFX;
using UnityEngine;

namespace SpaceGame.ShuttleSystems.MiningTool {
    [CreateAssetMenu(fileName = "New Mining Tool Configuration", menuName = "Game Data/Mining Tool Configuration", order = 1)]
    public class MiningToolUpgrade: ShuttleUpgrade, IMiningToolConfiguration {
        [Header("Laser parameters")]
        [SerializeField] private float _strength = 10f;
        [SerializeField] private float _range = 50f;
        [SerializeField] private float _powerConsumption = 2f;

        [Header("VFX")]
        [SerializeField] private MiningToolVFX _vfxPrefab = default;

        public override string Name => name;
        public override string Description => $"{base.Description}\n<#00ff00>• Mining Strength: {_strength}\n• Range: {_range}m\n• Power Consumption: {_powerConsumption}</color>";
        
        public float Strength => _strength;
        public float Range => _range;
        public float PowerConsumption => _powerConsumption;
        public MiningToolVFX VFXPrefab => _vfxPrefab;
    }
}
