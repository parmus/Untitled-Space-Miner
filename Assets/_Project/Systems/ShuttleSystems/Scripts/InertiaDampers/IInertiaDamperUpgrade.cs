namespace SpaceGame.ShuttleSystems.InertiaDampers
{
    public interface IInertiaDamperUpgrade
    {
        float Drag { get;  }
        float AngularDrag { get; }
    }
}