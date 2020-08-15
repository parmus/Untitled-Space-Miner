using SpaceGame.ShuttleSystems.ResourceScanner;

namespace SpaceGame.ShuttleSystems.UI
{
    public class ResourceScannerConfigurationSlot : ShuttleUpgradeSlot<Configuration>
    {
        protected override void Set(Configuration upgrade) => ShuttleConfigurationManager.ResourceScannerConfiguration = upgrade;

        protected override Configuration Get() => ShuttleConfigurationManager.ResourceScannerConfiguration;
    }
}