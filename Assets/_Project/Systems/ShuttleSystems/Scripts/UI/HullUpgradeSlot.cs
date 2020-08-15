using SpaceGame.ShuttleSystems.Hull;

namespace SpaceGame.ShuttleSystems.UI
{
    public class HullUpgradeSlot : ShuttleUpgradeSlot<HullUpgrade>
    {
        protected override void Set(HullUpgrade upgrade) => ShuttleConfigurationManager.HullUpgrade = upgrade;
        protected override HullUpgrade Get() => ShuttleConfigurationManager.HullUpgrade;
    }
}