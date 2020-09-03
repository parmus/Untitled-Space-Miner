using System;
using System.Linq;
using SpaceGame.Utility.Settings;
using UnityEngine;

namespace SpaceGame.Utility.UI
{
    public class VideoSettingsUI: MonoBehaviour
    {
        [SerializeField] private UnityEngine.UI.Toggle _fullscreenToggle = default;
        [SerializeField] private TMPro.TMP_Dropdown _qualityIndexDropDown = default;
        [SerializeField] private TMPro.TMP_Dropdown _resolutionIndexDropDown = default;

        [SerializeField] private VideoSettings _videoSettings = default;
        
        private void Start()
        {
            _fullscreenToggle.isOn = _videoSettings.Fullscreen.Value;
        
            _qualityIndexDropDown.ClearOptions();
            _qualityIndexDropDown.AddOptions(QualitySettings.names.ToList());
            _qualityIndexDropDown.value = _videoSettings.QualityIndex.Value;
            _qualityIndexDropDown.RefreshShownValue();
        
            _resolutionIndexDropDown.ClearOptions();
            _resolutionIndexDropDown.AddOptions(
                Array.ConvertAll(Screen.resolutions, resolution => $"{resolution.width}x{resolution.height}").ToList()
            );
            _resolutionIndexDropDown.value = _videoSettings.ResolutionIndex.Value;
            _resolutionIndexDropDown.RefreshShownValue();
        
            _fullscreenToggle.onValueChanged.AddListener(_videoSettings.Fullscreen.Set);
            _qualityIndexDropDown.onValueChanged.AddListener(_videoSettings.QualityIndex.Set);
            _resolutionIndexDropDown.onValueChanged.AddListener(_videoSettings.ResolutionIndex.Set);

            _videoSettings.Fullscreen.OnChange += fullscreen => _fullscreenToggle.isOn = fullscreen;
            _videoSettings.QualityIndex.OnChange += index => _qualityIndexDropDown.value = index;
            _videoSettings.ResolutionIndex.OnChange += index => _resolutionIndexDropDown.value = index;
        }
    }
}