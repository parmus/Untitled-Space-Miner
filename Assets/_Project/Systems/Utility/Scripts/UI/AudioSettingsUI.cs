using SpaceGame.Utility.Settings;
using UnityEngine;

namespace SpaceGame.Utility.UI
{
    public class AudioSettingsUI : MonoBehaviour
    {
        [SerializeField] private UnityEngine.UI.Slider _masterVolumeSlider = default;
        [SerializeField] private UnityEngine.UI.Slider _sfxVolumeSlider = default;

        [SerializeField] private FMODBusVolume _masterBus = default;
        [SerializeField] private FMODVCAVolume _sfxVca = default;

        private void Start()
        {
            _masterVolumeSlider.value = _masterBus.Volume.Value;
            _sfxVolumeSlider.value = _sfxVca.Volume.Value;
        
            _masterVolumeSlider.onValueChanged.AddListener(_masterBus.Volume.Set);
            _sfxVolumeSlider.onValueChanged.AddListener(_sfxVca.Volume.Set);
        
            _masterBus.Volume.OnChange += volume => _masterVolumeSlider.value = volume;
            _sfxVca.Volume.OnChange += volume => _sfxVolumeSlider.value = volume;
        }
    }
}
    
