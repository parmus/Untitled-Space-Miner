using SpaceGame.ShuttleSystems.Storage;

namespace SpaceGame.ShuttleSystems.UI.UpgradeSlot
{
    public class StorageUpgradeSlot : ShuttleUpgradeSlot<StorageUpgrade>
    {
        protected override void Set(StorageUpgrade upgrade) => ShuttleConfigurationManager.StorageUpgrade = upgrade;

        protected override StorageUpgrade Get() => ShuttleConfigurationManager.StorageUpgrade;
    }
}