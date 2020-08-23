using SpaceGame.InventorySystem;
using SpaceGame.Utility;
using SpaceGame.Utility.SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceGame
{
    public class SessionManager : Singleton<SessionManager> {
        [SerializeField] private int _firstSceneIndex = 2;

        public IInventory Inventory => _currentSession?.Inventory;

        private Session _currentSession = new Session();
        private readonly PersistableSession _persistableSession = new PersistableSession("savegame");
        
        public void NewGame() {
            _currentSession = new Session();
            var scene = SceneManager.GetSceneByBuildIndex(_firstSceneIndex);
            if (!scene.isLoaded) SceneManager.LoadScene(scene.buildIndex, LoadSceneMode.Additive);
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                Save();
            } else if (Input.GetKeyDown(KeyCode.F2))
            {
                Load();
            } else if (Input.GetKeyDown(KeyCode.F3))
            {
                Debug.Log("Deleting state...");
                _persistableSession.Delete();
            }
        }

        private void Load()
        {
            Debug.Log("Loading state...");
            _currentSession.RestoreState(_persistableSession.State["_session"]);
            PersistableEntity.RestoreStates(_persistableSession.State);
        }

        private void Save()
        {
            Debug.Log("Saving state...");

            using (var d = new DebugTimer(s => Debug.LogFormat(LogType.Log, LogOption.NoStacktrace, this, s)))
            {
                var state = _persistableSession.State;
                d.Mark("Load");
                PersistableEntity.CaptureStates(state);
                _persistableSession.State["_session"] = _currentSession.CaptureState();
                d.Mark("Capture");
                _persistableSession.Save();
                d.Mark("Save");
            }
        }
    }
}
