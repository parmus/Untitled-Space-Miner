using System;
using UnityEngine;

namespace SpaceGame.Utility.Settings
{
    [CreateAssetMenu(menuName = "Game Data/Create VideoSettings", fileName = "VideoSettings", order = 0)]
    public class VideoSettings: ScriptableObject
    {
        [SerializeField] private string _fullscreenSettingsKey = "Settings/Video/Fullscreen";
        [SerializeField] private string _qualityIndexSettingsKey = "Settings/Video/QualityIndex";
        [SerializeField] private string _resolutionIndexSettingsKey = "Settings/Video/ResolutionIndex";
    
        public readonly IObservable<bool> Fullscreen = new Observable<bool>();
        public readonly IObservable<int> QualityIndex = new Observable<int>();
        public readonly IObservable<int> ResolutionIndex = new Observable<int>();

        private void Awake()
        {
            #if UNITY_EDITOR
            if (!Application.isPlaying) return;
            #endif
            Fullscreen.Value = Screen.fullScreen;
            QualityIndex.Value = QualitySettings.GetQualityLevel();
            ResolutionIndex.Value = GetCurrentResolutionIndex();

            Fullscreen.OnChange += fullscreen => Screen.fullScreen = fullscreen;
            QualityIndex.OnChange += QualitySettings.SetQualityLevel;
            ResolutionIndex.OnChange += index =>
            {
                var res = Screen.resolutions[index];
                Screen.SetResolution(res.width, res.height, Fullscreen.Value);
            };
        }

        private void OnEnable() => Load();

        private void OnDisable() => Save();

        public void Load()
        {
            Fullscreen.Value = PlayerPrefs.GetInt(_fullscreenSettingsKey, Fullscreen.Value ? 1 : 0) != 0;
            QualityIndex.Value = PlayerPrefs.GetInt(_qualityIndexSettingsKey, QualityIndex.Value);
            ResolutionIndex.Value = PlayerPrefs.GetInt(_resolutionIndexSettingsKey, ResolutionIndex.Value);
        }

        public void Save()
        {
            PlayerPrefs.SetInt(_fullscreenSettingsKey, Fullscreen.Value ? 1 : 0);
            PlayerPrefs.SetInt(_qualityIndexSettingsKey, QualityIndex.Value);
            PlayerPrefs.SetInt(_resolutionIndexSettingsKey, ResolutionIndex.Value);
            PlayerPrefs.Save();
        }

        private static int GetCurrentResolutionIndex() => Array.FindIndex(Screen.resolutions, resolution => Screen.currentResolution.Equals(resolution));
    
#if UNITY_EDITOR
        [ContextMenu("Clear PlayerPrefs")]
        private void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteKey(_fullscreenSettingsKey);
            PlayerPrefs.DeleteKey(_qualityIndexSettingsKey);
            PlayerPrefs.DeleteKey(_resolutionIndexSettingsKey);
        }
#endif
    }
}