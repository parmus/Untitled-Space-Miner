namespace SpaceGame.ShuttleSystems.Thrusters {
    public interface IThrusterUpgrade {
        float NormalThrustPower { get; }
        float NormalPowerConsumption { get; }
        float BoostThrustPower { get; }
        float BoostPowerConsumption { get; }
    }
}
