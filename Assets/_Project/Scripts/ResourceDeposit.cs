using System;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;

namespace SpaceGame
{
    public class ResourceDeposit : MonoBehaviour
    {
        [SerializeField] private ResourceType _type = default;
        [SerializeField] private uint _amount = 1;
        [SerializeField] private float _maxHealth = 100f;

        [SerializeField] private string _id = "";

        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying) return;
            if (PrefabStageUtility.GetCurrentPrefabStage() != null) return;
            
            var so = new SerializedObject(this);
            var prop = so.FindProperty("_id");
            
            if (!string.IsNullOrEmpty(prop.stringValue)) return;
            prop.stringValue = Guid.NewGuid().ToString();
            so.ApplyModifiedProperties();
        }
        #endif

        public event Action<ResourceDeposit> OnDestroy;
        public event Action<float> OnDamaged;


        public ResourceType Type => _type;
        public uint Amount => _amount;

        public float MaxHealth => _maxHealth;
        public float Health { get; private set; }

        private void Awake() => Health = _maxHealth;

        public bool Damage(float damage) {
            if (Health < Mathf.Epsilon) {
                Debug.Log("Asteroid already dead!", this);
                Debug.Break();
                return true;  // Guard against already dead
            }

            Health = Mathf.Max(0, Health - damage);
            OnDamaged?.Invoke(Health);
            if (Health > 0f) return false;

            OnDestroy?.Invoke(this);
            Destroy(gameObject);
            return true;
        }
    }
}
