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
        
        private readonly SavingSystem _savingSystem = new SavingSystem();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                Debug.Log("Saving state...");
                _savingSystem.Save("savegame");
            } else if (Input.GetKeyDown(KeyCode.F2))
            {
                Debug.Log("Loading state...");
                _savingSystem.Load("savegame");
            } else if (Input.GetKeyDown(KeyCode.F3))
            {
                Debug.Log("Deleting state...");
                _savingSystem.Delete("savegame");
            }
        }

        [System.Serializable]
        public class Session {
            [SerializeField] private Inventory _inventory = new Inventory(40, 10);
            public IInventory Inventory => _inventory;
        }
    }
}
