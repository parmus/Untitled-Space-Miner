using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace SpaceGame.UI
{
    public class GameMenu : MonoBehaviour
    {
        [SerializeField] private Button _loadButton = default;
        [SerializeField] private Button _saveButton = default;
        [SerializeField] private Button _quitButton = default;

        private void Start()
        {
            _loadButton.onClick.AddListener(Load);
            _saveButton.onClick.AddListener(Save);
            _quitButton.onClick.AddListener(Quit);
        }

        private static void Load() => SessionManager.Instance.Load();

        private static void Save() => SessionManager.Instance.Save();

        private static void Quit()
        {
            #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }
}
