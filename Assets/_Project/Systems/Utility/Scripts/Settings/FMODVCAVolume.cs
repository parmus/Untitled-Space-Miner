using UnityEngine;

namespace SpaceGame.Utility.Settings
{
    [CreateAssetMenu(menuName = "Game Data/Create FMODVCAVolume", fileName = "FMODVCAVolume", order = 0)]
    public sealed class FMODVCAVolume: ScriptableObject
    {
        [SerializeField] private string _vcaPath = "vca:/sfx";
        [SerializeField] private string _playerPrefsKey = "Settings/Audio/SFXVolume";

        public readonly IObservable<float> Volume = new Observable<float>();
        
        private FMOD.Studio.VCA _vca;

        private void Awake()
        {
            _vca = FMODUnity.RuntimeManager.GetVCA(_vcaPath);
            Volume.OnChange += volume => _vca.setVolume(volume); 
        }

        private void OnEnable() => Volume.Value = PlayerPrefs.GetFloat(_playerPrefsKey, 1f);

        private void OnDisable()
        {
            PlayerPrefs.SetFloat(_playerPrefsKey,  Volume.Value);
            PlayerPrefs.Save();
        }
        
#if UNITY_EDITOR
        [ContextMenu("Clear PlayerPrefs")]
        private void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteKey(_playerPrefsKey);
        }
#endif
    }
}