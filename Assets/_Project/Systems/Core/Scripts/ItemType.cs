using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace SpaceGame.Core
{
    public abstract class ItemType<T> : ItemType where T: ItemType<T>
    {
        private static readonly Dictionary<string, T> _runtimeSet = new Dictionary<string, T>();

        public static T GetByName(string name) => string.IsNullOrEmpty(name) ? null : _runtimeSet[name];

        protected virtual void OnEnable()
        {
            #if UNITY_EDITOR
            if (_runtimeSet.TryGetValue(name, out var item)) Assert.AreEqual(item, this);
            #endif
            _runtimeSet.Add(name, (T) this);
        }
    }
    
    public abstract class ItemType : ScriptableObject
    {
        [SerializeField] protected Sprite _thumbnail = default;
        [SerializeField] protected uint _stackSize = 1;

        public abstract string Name { get; }
        public Sprite Thumbnail => _thumbnail;
        public uint StackSize => _stackSize;
        public bool CanStack => _stackSize > 1;
    }
    
}
