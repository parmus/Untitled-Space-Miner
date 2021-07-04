using System.Collections;
using UnityEngine;

namespace SpaceGame
{
    public class CoroutineRunner : MonoBehaviour
    {
        private static CoroutineRunner _instance;

        public static Coroutine Start_Coroutine(IEnumerator coroutine)
        {
            if (_instance == null)
            {
                _instance = new GameObject("[CoroutineRunner]").AddComponent<CoroutineRunner>();
                DontDestroyOnLoad(_instance);
            }

            return _instance.StartCoroutine(coroutine);
        }
    }
}