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

        [System.Serializable]
        public class Session {
            [SerializeField] private Inventory _inventory = new Inventory(40, 10);
            public IInventory Inventory => _inventory;
        }
    }
}
