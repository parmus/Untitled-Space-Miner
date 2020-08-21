using SpaceGame.ShuttleSystems.ResourceScanner;
using SpaceGame.Utility;

namespace SpaceGame.ShuttleSystems.UI.UpgradeSlot
{
    public class ResourceScannerConfigurationSlot : ShuttleUpgradeSlot<Configuration>
    {
        protected override IObservable<Configuration> UpgradeFromShuttle(Shuttle shuttle) => shuttle == null ? null : shuttle.ResourceScanner.Configuration;
    }
}