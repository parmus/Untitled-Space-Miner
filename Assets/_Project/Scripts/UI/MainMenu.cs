using UnityEngine;

namespace SpaceGame.UI
{
    public class MainMenu : MonoBehaviour
    {
        public static void NewGame() => LoadingScreen.Instance.NewGame();

        public static void LoadGame() => LoadingScreen.Instance.LoadGame();

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