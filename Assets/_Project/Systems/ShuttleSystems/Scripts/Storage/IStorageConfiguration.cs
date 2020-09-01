namespace SpaceGame.ShuttleSystems.Storage
{
    public interface IStorageConfiguration
    {
        uint Slots { get;  }
        uint StackMultiplier { get;  }
    }
}