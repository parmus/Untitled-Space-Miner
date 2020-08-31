using UnityEngine;

namespace SpaceGame.ShuttleSystems.Storage
{
    [CreateAssetMenu(fileName = "New Storage Upgrade", menuName = "Game Data/Storage Upgrade", order = 1)]
    public class StorageUpgrade : ShuttleUpgrade, IStorageConfiguration
    {
        [SerializeField] private uint _slot = 5;

        public override string Name => name;
        public override string Description => $"{base.Description}\n<#00ff00>• Slots: {_slot}</color>";
        public uint Slots => _slot;
    }
}