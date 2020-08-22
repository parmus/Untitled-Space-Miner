using SpaceGame.InventorySystem;
using SpaceGame.Utility;
using SpaceGame.Utility.SaveSystem;
using UnityEngine;

namespace SpaceGame
{
    public class SessionManager : Singleton<SessionManager> {
        public IInventory Inventory => _currentSession?.Inventory;

        [SerializeField] private Session _currentSession = new Session();

        public void NewGame() {
            _currentSession = new Session();
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                Debug.Log("Saving state...");
                SavingSystem.Save("savegame");
            } else if (Input.GetKeyDown(KeyCode.F2))
            {
                Debug.Log("Loading state...");
                SavingSystem.Load("savegame");
            } else if (Input.GetKeyDown(KeyCode.F3))
            {
                Debug.Log("Deleting state...");
                SavingSystem.Delete("savegame");
            }
        }

        [System.Serializable]
        public class Session {
            [SerializeField] private Inventory _inventory = new Inventory(40, 10);
            public IInventory Inventory => _inventory;
        }
    }
}
