using System.Collections.Generic;
using SpaceGame.Core;
using SpaceGame.InventorySystem;
using SpaceGame.Utility;
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
                upgrade => Inventory.Resize(upgrade ? upgrade.Slots : _defaultConfiguration.Slots);
        }
        
        #region IPersistable
        [System.Serializable]
        public class PersistentData
        {
            public readonly string StorageUpgradeName;
            public readonly List<string> ItemTypes = new List<string>();
            public readonly List<uint> ItemAmounts = new List<uint>();

            public StorageUpgrade StorageUpgrade => ItemType.GetByName<StorageUpgrade>(StorageUpgradeName);

            public void RestoreInventory(IInventory inventory)
            {
                Assert.IsTrue(inventory.Count == ItemTypes.Count);
                Assert.IsTrue(ItemAmounts.Count == ItemTypes.Count);
                inventory.Clear();
                for (var i = 0; i < ItemTypes.Count; i++)
                {
                    inventory[i].TryAdd(ItemType.GetByName<ItemType>(ItemTypes[i]), ItemAmounts[i]);
                }
            }

            public PersistentData(StorageUpgrade storageUpgrade, IInventory inventory)
            {
                StorageUpgradeName = storageUpgrade != null ? storageUpgrade.Name : null;
                foreach (var stack in inventory)
                {
                    ItemTypes.Add(stack.Type != null ? stack.Type.Name : null);
                    ItemAmounts.Add(stack.Amount);
                }
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
            public uint Slots => _slot;
        }
    }
}