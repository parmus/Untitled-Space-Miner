using SpaceGame.Core;
using UnityEngine;

namespace SpaceGame.ShuttleSystems
{
    public abstract class ShuttleUpgrade : ItemType
    {
        [SerializeField] private string _description;

        public override string Description => _description;
        public override string Tooltip => $"<b></u>{Name}</u><b>\n{Description}";
        
        
        [System.Serializable]
        public abstract class PersistentData<T> where T : ShuttleUpgrade
        {
            public readonly string GUID;
            public T Upgrade => GetByGUID<T>(GUID);
            protected PersistentData(T upgrade) => GUID = upgrade != null ? upgrade.GUID : null;
        }
        
        
    }
}
