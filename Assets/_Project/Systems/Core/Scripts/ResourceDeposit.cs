using System;
using SpaceGame.Utility;
using SpaceGame.Utility.SaveSystem;
using UnityEngine;

namespace SpaceGame.Core
{
    public class ResourceDeposit : MonoBehaviour, IPersistable
    {
        [SerializeField] private ResourceType _type;
        [SerializeField] private uint _amount = 1;

        public event Action<ResourceDeposit> OnDestroy;

        public ResourceType Type => _type;
        public uint Amount => _amount;

        public float MaxHealth => _type != null ? _type.Hardness * _amount : 0f;
        
        public IReadonlyObservable<float> Health => _health;
        private readonly Observable<float> _health = new Observable<float>();

        private void Awake() => _health.Value = MaxHealth;

        public bool Damage(float damage) {
            if (_health.Value.IsZero()) {
                Debug.Log("Asteroid already dead!", this);
                Debug.Break();
                return true;  // Guard against already dead
            }

            _health.Value = Mathf.Max(0, _health.Value - damage);
            if (_health.Value > 0f) return false;

            OnDestroy?.Invoke(this);
            gameObject.SetActive(false); //Destroy(gameObject);
            return true;
        }

        #region IPersistable
        [Serializable]
        public class PersistentData
        {
            public readonly float Health;
            public PersistentData(float health) => Health = health;
        }

        public object CaptureState() => new PersistentData(_health.Value);

        public void RestoreState(object state)
        {
            var persistentData = (PersistentData) state;
            _health.Value = persistentData.Health;
            
            gameObject.SetActive(!_health.Value.IsZero());
            
        }
        #endregion
    }
}
