using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;

namespace SpaceGame
{
    public sealed class PersistableEntity: MonoBehaviour
    {
        private static readonly HashSet<PersistableEntity> _runtimeSet = new HashSet<PersistableEntity>();

        [SerializeField] private string _id = "";
        public string ID => _id;

        public object CaptureState()
        {
            var state = new Dictionary<string, object>();
            foreach (var persistable in GetComponents<IPersistable>())
            {
                state[persistable.GetType().ToString()] = persistable.CaptureState();
            }

            return state;
        }

        public void RestoreState(object entityState)
        {
            var stateDict = (Dictionary<string, object>) entityState;
            foreach (var persistable in GetComponents<IPersistable>())
            {
                if (stateDict.TryGetValue(persistable.GetType().ToString(), out var state))
                {
                    persistable.RestoreState(state);
                }
            }
        }

        
        public static void SaveTo(Dictionary<string, object> state)
        {
            state.Clear();
            foreach (var persistableEntity in _runtimeSet)
            {
                state[persistableEntity.ID] = persistableEntity.CaptureState();
            }
        }

        public static void LoadFrom(Dictionary<string, object> state)
        {
            foreach (var persistableEntity in _runtimeSet)
            {
                if (state.TryGetValue(persistableEntity.ID, out var entityState))
                {
                    persistableEntity.RestoreState(entityState);
                }
            }
        }
        
        
        #region Unique ID validation
#if UNITY_EDITOR
        private static readonly Dictionary<string, PersistableEntity> _persistableEntities = new Dictionary<string, PersistableEntity>();
        private void OnValidate()
        {
            if (Application.isPlaying) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;
            if (PrefabStageUtility.GetCurrentPrefabStage() != null) return;
            
            var so = new SerializedObject(this);
            var prop = so.FindProperty("_id");

            if (!IsValidAndUnique(prop.stringValue))
            {
                prop.stringValue = Guid.NewGuid().ToString();
                so.ApplyModifiedProperties();
            }
            _persistableEntities[prop.stringValue] = this;
        }

        private bool IsValidAndUnique(string id)
        {
            if (string.IsNullOrEmpty(id)) return false;
            
            if (!_persistableEntities.TryGetValue(id, out var persistableEntity)) return true;
            switch (persistableEntity)
            {
                case PersistableEntity p when p == this:
                    return true;
                case null:   
                case PersistableEntity p when p.ID != id:
                    _persistableEntities.Remove(id);
                    return true;
                default:
                    return false;
            }
        }
#endif
        #endregion

        private void Awake() => _runtimeSet.Add(this);

        private void OnDestroy() => _runtimeSet.Remove(this);
    }
}