
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace SpaceGame
{
    public abstract class BetterScriptableObject : ScriptableObject
    {
        protected virtual void OnInitialize() {}
        protected virtual void OnTerminate() {}
    
#if UNITY_EDITOR
        protected  void OnEnable() {
            EditorApplication.playModeStateChanged += state => {
                if (state == PlayModeStateChange.EnteredPlayMode) OnInitialize();
            };
        }

        protected  void OnDisable() {
            EditorApplication.playModeStateChanged += state => {
                if (state == PlayModeStateChange.ExitingPlayMode) OnTerminate();
            };
        }
#else
    protected  void OnEnable() => OnInitialize();
    protected  void OnEnable() => OnTerminate();
#endif
    }
}