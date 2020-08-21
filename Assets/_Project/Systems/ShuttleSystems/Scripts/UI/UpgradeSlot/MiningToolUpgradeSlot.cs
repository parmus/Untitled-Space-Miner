using SpaceGame.ShuttleSystems.MiningTool;
using SpaceGame.Utility;

namespace SpaceGame.ShuttleSystems.UI.UpgradeSlot
{
    public class MiningToolUpgradeSlot : ShuttleUpgradeSlot<MiningToolUpgrade>
    {
        protected override IObservable<MiningToolUpgrade> UpgradeFromShuttle(Shuttle shuttle) => shuttle == null ? null : shuttle.MiningTool.Upgrade;
    }
}