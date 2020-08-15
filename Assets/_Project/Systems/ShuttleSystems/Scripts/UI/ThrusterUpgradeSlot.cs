using SpaceGame.ShuttleSystems.Thrusters;

namespace SpaceGame.ShuttleSystems.UI
{
    public class ThrusterUpgradeSlot : ShuttleUpgradeSlot<ThrusterUpgrade>
    {
        protected override void Set(ThrusterUpgrade upgrade) => ShuttleConfigurationManager.ThrusterUpgrade = upgrade;

        protected override ThrusterUpgrade Get() => ShuttleConfigurationManager.ThrusterUpgrade;
    }
}