using SpaceGame.ShuttleSystems.Storage;
using SpaceGame.Utility;

namespace SpaceGame.ShuttleSystems.UI.UpgradeSlot
{
    public class StorageUpgradeSlot : ShuttleUpgradeSlot<StorageUpgrade>
    {
        protected override IObservable<StorageUpgrade> UpgradeFromShuttle(Shuttle shuttle) => shuttle == null ? null : shuttle.Storage.Upgrade;
    }
}