using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;

namespace SpaceGame.Core
{
    public abstract class ItemType : ScriptableObject
    {
        private static readonly Dictionary<string, ItemType> _runtimeSet = new Dictionary<string, ItemType>();

        [HorizontalGroup(GroupID = "Base", Width = 110, LabelWidth = 80)]
        [PreviewField(100, ObjectFieldAlignment.Left), HideLabel]
        [SerializeField] protected Sprite _thumbnail;
        
        [VerticalGroup(GroupID = "Base/Right")]
        
        [VerticalGroup(GroupID = "Base/Right/Basic", PaddingBottom = 10), LabelText("GUID"), ReadOnly]
        [SerializeField] private string _guid;
        
        [VerticalGroup(GroupID = "Base/Right/Basic"), ShowInInspector]
        public abstract string Name { get; }

        [TitleGroup("Stackability", GroupID="Base/Right/Stackability")]
        [SerializeField] protected bool _canStack = true;

        [TitleGroup("Stackability", GroupID="Base/Right/Stackability")]
        [SerializeField] protected uint _stackSize = 1;

        public static T GetByGUID<T>(string name) where T: ItemType => string.IsNullOrEmpty(name) ? null : _runtimeSet[name] as T;

        public string GUID => _guid;
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
