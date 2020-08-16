using System;
using SpaceGame.Utility;
using UnityEngine;

namespace SpaceGame
{
    public class ResourceDeposit : MonoBehaviour, IPersistable
    {
        [SerializeField] private ResourceType _type = default;
        [SerializeField] private uint _amount = 1;
        [SerializeField] private float _maxHealth = 100f;

        public event Action<ResourceDeposit> OnDestroy;

        public ResourceType Type => _type;
        public uint Amount => _amount;

        public float MaxHealth => _maxHealth;
        
        public IReadonlyObservable<float> Health => _health;
        private readonly Observable<float> _health = new Observable<float>();

        private void Awake() => _health.Value = _maxHealth;

        public bool Damage(float damage) {
            if (_health.Value < Mathf.Epsilon) {
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

        #region Persistence
        [Serializable]
        public class PersistentData
        {
            public float _maxHealth;
            public float _health;

            public PersistentData(float maxHealth, float health)
            {
                _maxHealth = maxHealth;
                _health = health;
            }
        }

        public object CaptureState() => new PersistentData(MaxHealth, _health.Value);

        public void RestoreState(object state)
        {
            var persistentData = (PersistentData) state;
            _maxHealth = persistentData._maxHealth;
            _health.Value = persistentData._health;
            
            gameObject.SetActive(_health.Value > Mathf.Epsilon);
        }
        #endregion
    }
}
