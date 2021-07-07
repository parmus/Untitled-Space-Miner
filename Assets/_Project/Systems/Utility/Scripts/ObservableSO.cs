using UnityEngine;

namespace SpaceGame.Utility
{
    public class ObservableSO<T> : ScriptableObject
    {
        public readonly Observable<T> Value = new Observable<T>();
    }
}
