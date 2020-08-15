using SpaceGame.ShuttleSystems.MiningTool.VFX;

namespace SpaceGame.ShuttleSystems.MiningTool {
    public interface IMiningToolConfiguration {
        float Strength { get; }
        float Range { get; }
        float PowerConsumption { get; }
        MiningToolVFX VFXPrefab { get; }
    }
}
