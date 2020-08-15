using SpaceGame.ShuttleSystems.MiningTool;

namespace SpaceGame.ShuttleSystems.UI
{
    public class MiningToolUpgradeSlot : ShuttleUpgradeSlot<MiningToolUpgrade>
    {
        protected override void Set(MiningToolUpgrade upgrade) => ShuttleConfigurationManager.MiningToolUpgrade = upgrade;
        protected override MiningToolUpgrade Get() => ShuttleConfigurationManager.MiningToolUpgrade;
    }
}