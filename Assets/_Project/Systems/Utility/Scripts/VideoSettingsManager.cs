using System;
using UnityEngine;

namespace SpaceGame.Utility
{
    public class VideoSettingsManager: Singleton<VideoSettingsManager>
    {
        private const string FullscreenSettingsKey = "Settings/Video/Fullscreen";
        private const string QualityIndexSettingsKey = "Settings/Video/QualityIndex";
        private const string ResolutionIndexSettingsKey = "Settings/Video/ResolutionIndex";
    
        public readonly IObservable<bool> Fullscreen = new Observable<bool>();
        public readonly IObservable<int> QualityIndex = new Observable<int>();
        public readonly IObservable<int> ResolutionIndex = new Observable<int>();

        protected override void Awake()
        {
            base.Awake();

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
            Fullscreen.Value = PlayerPrefs.GetInt(FullscreenSettingsKey, Fullscreen.Value ? 1 : 0) != 0;
            QualityIndex.Value = PlayerPrefs.GetInt(QualityIndexSettingsKey, QualityIndex.Value);
            ResolutionIndex.Value = PlayerPrefs.GetInt(ResolutionIndexSettingsKey, ResolutionIndex.Value);
        }

        public void Save()
        {
            PlayerPrefs.SetInt(FullscreenSettingsKey, Fullscreen.Value ? 1 : 0);
            PlayerPrefs.SetInt(QualityIndexSettingsKey, QualityIndex.Value);
            PlayerPrefs.SetInt(ResolutionIndexSettingsKey, ResolutionIndex.Value);
        }

        private static int GetCurrentResolutionIndex() => Array.FindIndex(Screen.resolutions, resolution => Screen.currentResolution.Equals(resolution));
    
#if UNITY_EDITOR
        [ContextMenu("Clear PlayerPrefs")]
        private void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteKey(FullscreenSettingsKey);
            PlayerPrefs.DeleteKey(QualityIndexSettingsKey);
            PlayerPrefs.DeleteKey(ResolutionIndexSettingsKey);
        }
#endif
    }
}
