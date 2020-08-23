using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceGame.UI
{
    public class MainMenu : MonoBehaviour
    {
        public static void NewGame()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1).completed += operation =>
            {
                SessionManager.Instance.NewGame();
            };
        }

        public static void Quit()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif            
        }
    }
}