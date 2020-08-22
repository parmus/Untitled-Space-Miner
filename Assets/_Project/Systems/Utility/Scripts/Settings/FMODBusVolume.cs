using UnityEngine;

namespace SpaceGame.Utility.Settings
{
    [CreateAssetMenu(menuName = "Game Data/Create FMODBusVolume", fileName = "FMODBusVolume", order = 0)]
    public sealed class FMODBusVolume: ScriptableObject
    {
        [SerializeField] private string _busPath = "bus:/";
        [SerializeField] private string _playerPrefsKey = "Settings/Audio/MasterVolume";

        public readonly IObservable<float> Volume = new Observable<float>();

        private FMOD.Studio.Bus _bus;

        private void Awake()
        {
            _bus = FMODUnity.RuntimeManager.GetBus(_busPath);
            Volume.OnChange += volume => _bus.setVolume(volume); 
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