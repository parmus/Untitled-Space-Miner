using System;

namespace SpaceGame.Utility
{
    public interface IReadonlyObservable<out T>
    {
        event Action<T> OnChange;

        T Value { get; }
    }
}