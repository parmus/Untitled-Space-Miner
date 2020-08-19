using UnityEngine;

namespace SpaceGame.Core
{
    public abstract class ItemType : ScriptableObject {
        [SerializeField] protected Sprite _thumbnail = default;
        [SerializeField] protected uint _stackSize = 1;

        public abstract string Name { get; }
        public Sprite Thumbnail => _thumbnail;
        public uint StackSize => _stackSize;
        public bool CanStack => _stackSize > 1;
    }
}
