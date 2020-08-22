using System.Collections.Generic;
using SpaceGame.Core;
using SpaceGame.InventorySystem;
using SpaceGame.Utility;
using SpaceGame.Utility.SaveSystem;
using UnityEngine;
using UnityEngine.Assertions;

namespace SpaceGame
{
    public class SessionManager : Singleton<SessionManager> {
        public IInventory Inventory => _currentSession?.Inventory;

        [SerializeField] private Session _currentSession = new Session();

        private readonly PersistableSession _persistableSession = new PersistableSession("savegame");
        
        public void NewGame() {
            _currentSession = new Session();
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                Load();
            } else if (Input.GetKeyDown(KeyCode.F2))
            {
                Save();
            } else if (Input.GetKeyDown(KeyCode.F3))
            {
                Debug.Log("Deleting state...");
                _persistableSession.Delete();
            }
        }

        private void Save()
        {
            Debug.Log("Loading state...");
            PersistableEntity.RestoreStates(_persistableSession.State);
            _currentSession.RestoreState(_persistableSession.State["_session"]);
        }

        private void Load()
        {
            Debug.Log("Saving state...");
            var d = new DebugTimer();
            var state = _persistableSession.State;
            PersistableEntity.CaptureStates(state);
            d.Mark("Load");
            _persistableSession.State["_session"] = _currentSession.CaptureState();
            d.Mark("Capture");
            _persistableSession.Save();
            d.Mark("Save");

            foreach (var entry in d.Entries) Debug.Log(entry);
        }

        [System.Serializable]
        public class Session: IPersistable {
            [SerializeField] private Inventory _inventory = new Inventory(40, 10);
            public IInventory Inventory => _inventory;
            
            
            #region IPersistable
            [System.Serializable]
            public class PersistentData
            {
                public readonly List<string> ItemTypes = new List<string>();
                public readonly List<uint> ItemAmounts = new List<uint>();

                public void RestoreInventory(IInventory inventory)
                {
                    Assert.IsTrue(inventory.Count == ItemTypes.Count);
                    Assert.IsTrue(ItemAmounts.Count == ItemTypes.Count);
                    inventory.Resize((uint) ItemTypes.Count);
                    inventory.Clear();
                    for (var i = 0; i < ItemTypes.Count; i++)
                    {
                        inventory[i].TryAdd(ItemType.GetByName<ItemType>(ItemTypes[i]), ItemAmounts[i]);
                    }
                }

                public PersistentData(IInventory inventory)
                {
                    foreach (var stack in inventory)
                    {
                        ItemTypes.Add(stack.Type != null ? stack.Type.Name : null);
                        ItemAmounts.Add(stack.Amount);
                    }
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
}
