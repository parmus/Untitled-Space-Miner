using System;
using SpaceGame.InventorySystem;
using SpaceGame.Utility;
using UnityEngine;

namespace SpaceGame.ShuttleSystems.Storage
{
    public class Storage : MonoBehaviour
    {
        [SerializeField] private DefaultConfiguration _defaultConfiguration = new DefaultConfiguration();

        public readonly Observable<StorageUpgrade> Upgrade = new Observable<StorageUpgrade>();

        public IInventory Inventory { get; private set; }

        private void Awake()
        {
            Inventory = new Inventory(_defaultConfiguration.Slots);
            Upgrade.OnChange +=
                upgrade => Inventory.Resize(upgrade ? upgrade.Slots : _defaultConfiguration.Slots);
        }

        [Serializable]
        private class DefaultConfiguration: IStorageConfiguration
        {
            [SerializeField] private uint _slot = 5;
            public uint Slots => _slot;
        }
    }
}