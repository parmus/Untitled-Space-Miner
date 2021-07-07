using UnityEngine;
using UnityEngine.Events;

namespace SpaceGame.Utility
{
    public class ObservableListener<T> : MonoBehaviour
    {
        [SerializeField] protected ObservableSO<T> _observable;
        [SerializeField] protected UnityEvent<T> _onChange;

        private void OnEnable() => _observable.Value.Subscribe(_onChange.Invoke);
        private void OnDisable() => _observable.Value.Unsubscribe(_onChange.Invoke);
    }
}
