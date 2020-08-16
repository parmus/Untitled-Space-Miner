using System;
using System.Collections.Generic;
using SpaceGame.InventorySystem;
using SpaceGame.Utility;
using UnityEngine;

namespace SpaceGame
{
    public class SessionManager : Singleton<SessionManager> {
        public IInventory Inventory => _currentSession?.Inventory;

        [SerializeField] private Session _currentSession = new Session();

        public void NewGame() {
            _currentSession = new Session();
        }
        
        private readonly Dictionary<string, object> _savedState = new Dictionary<string, object>();

        public void Save() => PersistableEntity.SaveTo(_savedState);

        public void Load() => PersistableEntity.LoadFrom(_savedState);

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                Debug.Log("Saving state...");
                Save();
            } else if (Input.GetKeyDown(KeyCode.F2))
            {
                Debug.Log("Loading state...");
                Load();
            }
        }

        [System.Serializable]
        public class Session {
            [SerializeField] private Inventory _inventory = new Inventory(40, 10);
            public IInventory Inventory => _inventory;
        }
    }
}
