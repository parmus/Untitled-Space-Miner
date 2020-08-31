using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace SpaceGame.Core
{
    public abstract class ItemType : ScriptableObject
    {
        private static readonly Dictionary<string, ItemType> _runtimeSet = new Dictionary<string, ItemType>();
        [SerializeField] protected Sprite _thumbnail = default;
        [SerializeField] protected uint _stackSize = 1;

        public static T GetByName<T>(string name) where T: ItemType => string.IsNullOrEmpty(name) ? null : _runtimeSet[name] as T;

        
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract string Tooltip { get; }
        public Sprite Thumbnail => _thumbnail;
        public uint StackSize => _stackSize;
        public bool CanStack => _stackSize > 1;
        
        protected virtual void OnEnable()
        {
            #if UNITY_EDITOR
            if (_runtimeSet.TryGetValue(name, out var item)) Assert.AreEqual(item, this);
            #endif
            _runtimeSet[name] = this;
        }
    }
}
