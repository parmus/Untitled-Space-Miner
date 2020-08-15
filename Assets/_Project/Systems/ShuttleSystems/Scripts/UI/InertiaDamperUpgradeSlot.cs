using SpaceGame.ShuttleSystems.InertiaDampers;

namespace SpaceGame.ShuttleSystems.UI
{
    public class InertiaDamperUpgradeSlot : ShuttleUpgradeSlot<InertiaDamperUpgrade>
    {
        protected override void Set(InertiaDamperUpgrade upgrade) => ShuttleConfigurationManager.InertiaDamperUpgrade = upgrade;
        protected override InertiaDamperUpgrade Get() => ShuttleConfigurationManager.InertiaDamperUpgrade;
    }
}