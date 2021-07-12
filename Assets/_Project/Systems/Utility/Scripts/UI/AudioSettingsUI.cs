using SpaceGame.Utility.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceGame.Utility.UI
{
    public class AudioSettingsUI : MonoBehaviour
    {
        [SerializeField] private Slider _masterVolumeSlider;
        [SerializeField] private Slider _sfxVolumeSlider;

        [SerializeField] private FMODBusVolume _masterBus;
        [SerializeField] private FMODVCAVolume _sfxVca;

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
