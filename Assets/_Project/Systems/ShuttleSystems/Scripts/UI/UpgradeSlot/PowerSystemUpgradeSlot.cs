using SpaceGame.ShuttleSystems.PowerSystem;
using SpaceGame.Utility;

namespace SpaceGame.ShuttleSystems.UI.UpgradeSlot
{
    public class PowerSystemUpgradeSlot : ShuttleUpgradeSlot<PowerSystemUpgrade>
    {
        protected override IObservable<PowerSystemUpgrade> UpgradeFromShuttle(Shuttle shuttle) => shuttle == null ? null : shuttle.PowerSystem.Upgrade;
    }
}