using System;
using UnityEngine;

namespace SpaceGame.Utility.GameEvents
{
    public class GameEvent : ScriptableObject
    {
        public event Action OnEvent;
        public void Broadcast() => OnEvent?.Invoke();
    }
    
    public abstract class GameEvent<T> : ScriptableObject
    {
        public event Action<T> OnEvent;
        public void Broadcast(T value) => OnEvent?.Invoke(value);
    }
}
