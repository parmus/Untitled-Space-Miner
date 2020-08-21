using SpaceGame.ShuttleSystems.MiningTool.VFX;
using UnityEngine;

namespace SpaceGame.ShuttleSystems.MiningTool {
    [CreateAssetMenu(fileName = "New Mining Tool Configuration", menuName = "Game Data/Mining Tool Configuration", order = 1)]
    public class MiningToolUpgrade: ShuttleUpgrade<MiningToolUpgrade>, IMiningToolConfiguration {
        [Header("Laser parameters")]
        [SerializeField] private float _strength = 10f;
        [SerializeField] private float _range = 50f;
        [SerializeField] private float _powerConsumption = 2f;

        [Header("VFX")]
        [SerializeField] private MiningToolVFX _vfxPrefab = default;

        public override string Name => name;
        public float Strength => _strength;
        public float Range => _range;
        public float PowerConsumption => _powerConsumption;
        public MiningToolVFX VFXPrefab => _vfxPrefab;
    }
}
