using UnityEngine;

namespace SpaceGame.Utility.UI
{
    public class AudioSettingsUI : MonoBehaviour
    {
        [SerializeField] private UnityEngine.UI.Slider _masterVolumeSlider = default;
        [SerializeField] private UnityEngine.UI.Slider _sfxVolumeSlider = default;

        private void Start()
        {
            var audioSettingsManager = AudioSettingsManager.Instance;
        
            _masterVolumeSlider.value = audioSettingsManager.MasterVolume.Value;
            _sfxVolumeSlider.value = audioSettingsManager.SFXVolume.Value;
        
            _masterVolumeSlider.onValueChanged.AddListener(audioSettingsManager.MasterVolume.Set);
            _sfxVolumeSlider.onValueChanged.AddListener(audioSettingsManager.SFXVolume.Set);
        
            audioSettingsManager.MasterVolume.OnChange += volume => _masterVolumeSlider.value = volume;
            audioSettingsManager.SFXVolume.OnChange += volume => _sfxVolumeSlider.value = volume;
        }
    }
}
    
