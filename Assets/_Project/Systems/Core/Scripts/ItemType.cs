using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;

namespace SpaceGame.Core
{
    public abstract class ItemType : ScriptableObject
    {
        private static readonly Dictionary<string, ItemType> _runtimeSet = new Dictionary<string, ItemType>();
        [SerializeField, HideInInspector] private string _guid;
        [SerializeField] protected Sprite _thumbnail;
        [SerializeField] protected uint _stackSize = 1;
        [SerializeField] protected bool _canStack = true;

        public static T GetByGUID<T>(string name) where T: ItemType => string.IsNullOrEmpty(name) ? null : _runtimeSet[name] as T;

        [ShowInInspector, PropertyOrder(-1), PropertySpace(SpaceAfter = 5f)]
        public string GUID => _guid;
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract string Tooltip { get; }
        public Sprite Thumbnail => _thumbnail;
        public uint StackSize => _stackSize;
        public bool CanStack => _canStack;
        
        protected virtual void OnEnable()
        {
            #if UNITY_EDITOR
            var path = UnityEditor.AssetDatabase.GetAssetPath(this);
            var so = new UnityEditor.SerializedObject(this);
            so.FindProperty("_guid").stringValue = UnityEditor.AssetDatabase.GUIDFromAssetPath(path).ToString();
            if (!_canStack) so.FindProperty("_stackSize").intValue = 1;
            so.ApplyModifiedProperties();
            if (_runtimeSet.TryGetValue(_guid, out var item)) Assert.AreEqual(item, this);
            #endif
            
            _runtimeSet[_guid] = this;
        }
    }
}
