using System;

namespace SpaceGame.Utility
{
    public interface IReadonlyObservable<out T>
    {
        event Action<T> OnChange;

        T Value { get; }
        void Subscribe(Action<T> subscriber);
        void Unsubscribe(Action<T> subscriber);
    }
}