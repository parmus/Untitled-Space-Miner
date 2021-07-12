using System.Collections.Generic;
using SpaceGame.Core;
using SpaceGame.InventorySystem;
using SpaceGame.InventorySystem.Utils;
using SpaceGame.Utility;
using SpaceGame.Utility.SaveSystem;
using UnityEngine;
using UnityEngine.Assertions;

namespace SpaceGame.ShuttleSystems.Storage
{
    public class Storage : MonoBehaviour, IPersistable
    {
        [SerializeField] private DefaultConfiguration _defaultConfiguration = new DefaultConfiguration();

        public readonly IObservable<StorageUpgrade> Upgrade = new Observable<StorageUpgrade>();

        public IInventory Inventory { get; private set; }

        private void Awake()
        {
            Inventory = new Inventory(_defaultConfiguration.Slots);
            Upgrade.OnChange +=
                upgrade => Inventory.Resize(
                    upgrade ? upgrade.Slots : _defaultConfiguration.Slots,
                    upgrade ? upgrade.StackMultiplier : _defaultConfiguration.StackMultiplier);
        }
        
        #region IPersistable
        [System.Serializable]
        public class PersistentData
        {
            public readonly string GUID;
            public readonly SerializableInventory Inventory;
            public StorageUpgrade StorageUpgrade => ItemType.GetByGUID<StorageUpgrade>(GUID);
            public void RestoreInventory(IInventory inventory) => Inventory.RestoreInventory(inventory);

            public PersistentData(StorageUpgrade storageUpgrade, IInventory inventory)
            {
                GUID = storageUpgrade != null ? storageUpgrade.GUID : null;
                Inventory = new SerializableInventory(inventory);
            }
        }

        public object CaptureState() => new PersistentData(Upgrade.Value, Inventory);

        public void RestoreState(object state)
        {
            var persistentData = (PersistentData) state;
            Upgrade.Set(persistentData.StorageUpgrade);
            persistentData.RestoreInventory(Inventory);
        }
        #endregion


        [System.Serializable]
        private class DefaultConfiguration: IStorageConfiguration
        {
            [SerializeField] private uint _slot = 5;
            [SerializeField] private uint _stackMultiplier = 1;
            public uint Slots => _slot;
            public uint StackMultiplier => _stackMultiplier;
        }
    }
}
