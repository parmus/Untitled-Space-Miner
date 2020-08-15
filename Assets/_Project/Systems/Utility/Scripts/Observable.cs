using System;
using System.Collections.Generic;

namespace SpaceGame.Utility
{
    public class Observable<T>: IObservable<T>, IReadonlyObservable<T> {
        public event Action<T> OnChange = default;

        public T Value {
            get => _value;
            set
            {
                if (EqualityComparer<T>.Default.Equals(_value, value)) return;
                _value = value;
                OnChange?.Invoke(_value);
            }
        }

        public void Set(T value) => Value = value;

        public Observable() => _value = default;
        public Observable(T initialValue) => _value = initialValue;
        public Observable(Action<T> onChange) => OnChange += onChange;
        public Observable(T initialValue, Action<T> onChange, bool trigger = false) {
            _value = initialValue;
            OnChange = onChange;
            if (trigger) OnChange?.Invoke(_value);
        }
        public static implicit operator T(Observable<T> o) => o.Value;

        private T _value;
    }
}