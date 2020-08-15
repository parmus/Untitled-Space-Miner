using UnityEngine;

namespace SpaceGame.Utility
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static bool _applicationQuitting = false;
        public static T Instance {
            get {
                if (_applicationQuitting) return null;

                if (_instance != null) return _instance;
                _instance = FindObjectOfType<T>();
                if (_instance != null) return _instance;
                var obj = new GameObject {name = typeof(T).Name};
                _instance = obj.AddComponent<T>();
                return _instance;
            }
        }

        protected virtual void Awake() {
            if (_instance == null) {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            } else if (_instance != this as T) {
                Destroy(gameObject);
            } else {
                Debug.LogError($"[{typeof(T)}] Weird stuff happened");
            }
        }

        private void OnApplicationQuit() {
            _applicationQuitting = true;
        }
    }
}
