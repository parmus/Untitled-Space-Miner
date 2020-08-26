using SpaceGame.InventorySystem;
using SpaceGame.InventorySystem.Utils;
using SpaceGame.Utility.SaveSystem;
using UnityEngine;

namespace SpaceGame
{
    [System.Serializable]
    public class Session: IPersistable {
        [SerializeField] private Inventory _inventory = new Inventory(40, 10);
        public IInventory Inventory => _inventory;
            
        #region IPersistable
        [System.Serializable]
        public class PersistentData
        {
            public readonly SerializableInventory Inventory;
                
            public void RestoreInventory(IInventory inventory) => Inventory.RestoreInventory(inventory);

            public PersistentData(IInventory inventory)
            {
                Inventory = new SerializableInventory(inventory);
            }
        }

        public object CaptureState() => new PersistentData(Inventory);

        public void RestoreState(object state)
        {
            var persistentData = (PersistentData) state;
            persistentData.RestoreInventory(Inventory);
        }
        #endregion
    }
}