using SpaceGame.ShuttleSystems.Hull;
using SpaceGame.Utility;

namespace SpaceGame.ShuttleSystems.UI.UpgradeSlot
{
    public class HullUpgradeSlot : ShuttleUpgradeSlot<HullUpgrade>
    {
        protected override IObservable<HullUpgrade> UpgradeFromShuttle(Shuttle shuttle) => shuttle == null ? null : shuttle.Hull.Upgrade;
    }
}