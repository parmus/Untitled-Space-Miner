using System;

namespace SpaceGame.Utility
{
    public interface IObservable<T>
    {
        event Action<T> OnChange;
        
        T Value { get; set; }
        void Set(T value);
        void Subscribe(Action<T> subscriber);
        void Unsubscribe(Action<T> subscriber);
    }
}