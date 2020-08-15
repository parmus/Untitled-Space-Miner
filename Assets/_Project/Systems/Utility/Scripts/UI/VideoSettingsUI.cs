using System;
using System.Linq;
using UnityEngine;

namespace SpaceGame.Utility.UI
{
    public class VideoSettingsUI: MonoBehaviour
    {
        [SerializeField] private UnityEngine.UI.Toggle _fullscreenToggle = default;
        [SerializeField] private TMPro.TMP_Dropdown _qualityIndexDropDown = default;
        [SerializeField] private TMPro.TMP_Dropdown _resolutionIndexDropDown = default;

        private void Start()
        {
            var videoSettingsManager = VideoSettingsManager.Instance;
        
            _fullscreenToggle.isOn = videoSettingsManager.Fullscreen.Value;
        
            _qualityIndexDropDown.ClearOptions();
            _qualityIndexDropDown.AddOptions(QualitySettings.names.ToList());
            _qualityIndexDropDown.value = videoSettingsManager.QualityIndex.Value;
            _qualityIndexDropDown.RefreshShownValue();
        
            _resolutionIndexDropDown.ClearOptions();
            _resolutionIndexDropDown.AddOptions(
                Array.ConvertAll(Screen.resolutions, resolution => resolution.ToString()).ToList()
            );
            _resolutionIndexDropDown.value = videoSettingsManager.ResolutionIndex.Value;
            _resolutionIndexDropDown.RefreshShownValue();
        
            _fullscreenToggle.onValueChanged.AddListener(videoSettingsManager.Fullscreen.Set);
            _qualityIndexDropDown.onValueChanged.AddListener(videoSettingsManager.QualityIndex.Set);
            _resolutionIndexDropDown.onValueChanged.AddListener(videoSettingsManager.ResolutionIndex.Set);

            videoSettingsManager.Fullscreen.OnChange += fullscreen => _fullscreenToggle.isOn = fullscreen;
            videoSettingsManager.QualityIndex.OnChange += index => _qualityIndexDropDown.value = index;
            videoSettingsManager.ResolutionIndex.OnChange += index => _resolutionIndexDropDown.value = index;
        }
    }
}