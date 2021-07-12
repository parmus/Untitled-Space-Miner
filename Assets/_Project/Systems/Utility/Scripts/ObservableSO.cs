using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame.Utility
{
    public class ObservableSO<T> : ScriptableObject, IObservable<T>, IReadonlyObservable<T>
    {
        public event Action<T> OnChange;

        public T Value {
            get => _value;
            set
            {
                if (EqualityComparer<T>.Default.Equals(_value, value)) return;
                _value = value;
                OnChange?.Invoke(_value);
            }
        }

        private void OnDisable() => Set(default);

        public void Set(T value) => Value = value;

        public void Subscribe(Action<T> subscriber)
        {
            OnChange += subscriber;
            subscriber(Value);
        }

        public void Unsubscribe(Action<T> subscriber) => OnChange -= subscriber;

        public static implicit operator T(ObservableSO<T> o) => o.Value;

        private T _value;
    }
}
