using UnityEngine;

namespace SpaceGame.ShuttleSystems.Storage
{
    [CreateAssetMenu(fileName = "New Storage Upgrade", menuName = "Game Data/Storage Upgrade", order = 1)]
    public class StorageUpgrade : ShuttleUpgrade<StorageUpgrade>, IStorageConfiguration
    {
        [SerializeField] private uint _slot = 5;

        public override string Name => name;
        public uint Slots => _slot;
    }
}