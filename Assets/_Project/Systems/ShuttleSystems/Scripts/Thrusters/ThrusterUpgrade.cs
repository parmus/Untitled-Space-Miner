using System.Text;
using SpaceGame.Utility;
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
        public override string Description
        {
            get
            {
                var sb = new StringBuilder();
                sb.AppendLine(base.Description);
                sb.Append("<#00ff00>");
                sb.AppendLine($"• Normal Thrust: {_normalThrustPower}");
                sb.AppendLine($"• Normal Power Consumption: {_normalPowerConsumption}");
                if (!_normalThrustPower.Approximate(_boostThrustPower))
                {
                    sb.AppendLine($"• Boost Thrust: {_boostThrustPower}");
                    sb.AppendLine($"• Boost Power Consumption: {_boostPowerConsumption}");
                }
                sb.Append("</color>");
                return sb.ToString();
            }
        }

        public float NormalThrustPower => _normalThrustPower;
        public float NormalPowerConsumption => _normalPowerConsumption;
        public float BoostThrustPower => _boostThrustPower;
        public float BoostPowerConsumption => _boostPowerConsumption;
        #endregion

   }
}