using SpaceGame.ShuttleSystems.InertiaDampers;
using SpaceGame.Utility;

namespace SpaceGame.ShuttleSystems.UI.UpgradeSlot
{
    public class InertiaDamperUpgradeSlot : ShuttleUpgradeSlot<InertiaDamperUpgrade>
    {
        protected override IObservable<InertiaDamperUpgrade> UpgradeFromShuttle(Shuttle shuttle) => shuttle == null ? null : shuttle.InertiaDampers.Upgrade;
    }
}