using UnityEngine;

namespace SpaceGame.Utility
{
    public class AudioSettingsManager: Singleton<AudioSettingsManager>
    {
        private const string MasterBus = "bus:/";
        private const string MasterVolumeSettingsKey = "Settings/Audio/MasterVolume";
        private const string SFXVolumeSettingsKey = "Settings/Audio/SFXVolume";
    
    
        [SerializeField] private string _sfxVcaPath = "vca:/sfx";

        private FMOD.Studio.Bus _masterBus;
        private FMOD.Studio.VCA _sfxVca;

        public readonly IObservable<float> MasterVolume = new Observable<float>();
        public readonly IObservable<float> SFXVolume = new Observable<float>();

        protected override void Awake()
        {
            base.Awake();
        
            _masterBus = FMODUnity.RuntimeManager.GetBus(MasterBus);
            _sfxVca = FMODUnity.RuntimeManager.GetVCA(_sfxVcaPath);

            MasterVolume.OnChange += volume => _masterBus.setVolume(volume);
            SFXVolume.OnChange += volume => _sfxVca.setVolume(volume);
        }

        private void OnEnable() => Load();

        private void OnDisable() => Save();

        public void Load()
        {
            MasterVolume.Value = PlayerPrefs.GetFloat(MasterVolumeSettingsKey, 1f);
            SFXVolume.Value = PlayerPrefs.GetFloat(SFXVolumeSettingsKey, 1f);
        }

        public void Save()
        {
            PlayerPrefs.SetFloat(MasterVolumeSettingsKey,  MasterVolume.Value);
            PlayerPrefs.SetFloat(SFXVolumeSettingsKey,  SFXVolume.Value);
        }
    
#if UNITY_EDITOR
        [ContextMenu("Clear PlayerPrefs")]
        private void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteKey(MasterVolumeSettingsKey);
            PlayerPrefs.DeleteKey(SFXVolumeSettingsKey);
        }
#endif

    }
}
