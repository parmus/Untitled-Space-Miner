using System.Collections;
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
        private PersistableSession _persistableSession = new PersistableSession("savegame");
        
        public IEnumerator NewGame() {
            _persistableSession = new PersistableSession("savegame");
            _currentSession = new Session();
            yield return SceneManager.LoadSceneAsync(_firstSceneIndex, LoadSceneMode.Additive);
        }

        public IEnumerator LoadGame(string saveGameName)
        {
            _persistableSession = new PersistableSession(saveGameName);
            yield return CO_Load();
        }

        public void Load() => StartCoroutine(CO_Load());

        public void Save()
        {
            Debug.Log("Saving state...");

            using (var d = new DebugTimer(s => Debug.LogFormat(LogType.Log, LogOption.NoStacktrace, this, s)))
            {
                var state = _persistableSession.State;
                d.Mark("Load");
                PersistableEntity.CaptureStates(state);
                _persistableSession.State["_session"] = _currentSession.CaptureState();
                _persistableSession.State["_loadedScenes"] = SceneLoader.CaptureLoadedScenes();
                d.Mark("Capture");
                _persistableSession.Save();
                d.Mark("Save");
            }
        }

        private IEnumerator CO_Load()
        {
            var prevTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            Debug.Log("Loading state...");
            _persistableSession.Load();
            var sceneIdsToLoad = (int[]) _persistableSession.State["_loadedScenes"];
            yield return SceneLoader.RestoreScenes(sceneIdsToLoad);
            yield return null; // Give the ShuttleSpawner a frame to Spawn a shuttle
            _currentSession.RestoreState(_persistableSession.State["_session"]);
            PersistableEntity.RestoreStates(_persistableSession.State);
            Time.timeScale = prevTimeScale;
        }
    }
}
