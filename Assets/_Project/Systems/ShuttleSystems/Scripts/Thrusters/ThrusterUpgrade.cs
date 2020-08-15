using UnityEngine;

namespace SpaceGame.ShuttleSystems.Thrusters {

    [CreateAssetMenu(fileName = "New Thruster Upgrade", menuName = "Game Data/Thruster Upgrade", order = 1)]
    public class ThrusterUpgrade : ShuttleUpgrade, IThrusterUpgrade {
        #region Serialized fields
        [Header("Normal flight")]
        [SerializeField] private float _normalThrustPower = 500f;
        [SerializeField] private float _normalPowerConsumption = 0.1f;

        [Header("Boost flight")]
        [SerializeField] private float _boostThrustPower = 2500f;
        [SerializeField] private float _boostPowerConsumption = 5f;
        #endregion

        #region Properties
        public override string Name => name;
        public float NormalThrustPower => _normalThrustPower;
        public float NormalPowerConsumption => _normalPowerConsumption;
        public float BoostThrustPower => _boostThrustPower;
        public float BoostPowerConsumption => _boostPowerConsumption;
        #endregion

   }
}