using SpaceGame.ShuttleSystems.PowerSystem;

namespace SpaceGame.ShuttleSystems.UI
{
    public class PowerSystemUpgradeSlot : ShuttleUpgradeSlot<PowerSystemUpgrade>
    {
        protected override void Set(PowerSystemUpgrade upgrade) => ShuttleConfigurationManager.PowerSystemUpgrade = upgrade;
        protected override PowerSystemUpgrade Get() => ShuttleConfigurationManager.PowerSystemUpgrade;
    }
}