using SpaceGame.Core;

namespace SpaceGame.ShuttleSystems
{
    public abstract class ShuttleUpgrade<T>: ItemType<T> where T : ShuttleUpgrade<T> { }
}