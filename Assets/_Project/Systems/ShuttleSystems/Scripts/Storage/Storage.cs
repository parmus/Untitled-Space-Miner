using SpaceGame.InventorySystem;
using SpaceGame.InventorySystem.Utils;
using SpaceGame.Utility;
using SpaceGame.Utility.SaveSystem;
using UnityEngine;

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
        public class PersistentData: ShuttleUpgrade.PersistentData<StorageUpgrade>
        {
            public readonly SerializableInventory Inventory;
            public void RestoreInventory(IInventory inventory) => Inventory.RestoreInventory(inventory);

            public PersistentData(StorageUpgrade storageUpgrade, IInventory inventory): base(storageUpgrade)
            {
                Inventory = new SerializableInventory(inventory);
            }
        }

        public object CaptureState() => new PersistentData(Upgrade.Value, Inventory);

        public void RestoreState(object state)
        {
            var persistentData = (PersistentData) state;
            Upgrade.Set(persistentData.Upgrade);
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
