using SpaceGame.ShuttleSystems.Thrusters;
using SpaceGame.Utility;

namespace SpaceGame.ShuttleSystems.UI.UpgradeSlot
{
    public class ThrusterUpgradeSlot : ShuttleUpgradeSlot<ThrusterUpgrade>
    {
        protected override IObservable<ThrusterUpgrade> UpgradeFromShuttle(Shuttle shuttle) => shuttle == null ? null : shuttle.Thrusters.Upgrade;
    }
}