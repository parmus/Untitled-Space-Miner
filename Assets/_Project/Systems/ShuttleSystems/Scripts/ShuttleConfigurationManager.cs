using System;
using SpaceGame.Utility;
using UnityEngine;

namespace SpaceGame.ShuttleSystems {
    public class ShuttleConfigurationManager : Singleton<ShuttleConfigurationManager> {
        [SerializeField] private Hull.HullUpgrade _hullUpgrade = default;
        [SerializeField] private InertiaDampers.InertiaDamperUpgrade _inertiaDamperUpgrade = default;
        [SerializeField] private MiningTool.MiningToolUpgrade _miningToolUpgrade = default;
        [SerializeField] private PowerSystem.PowerSystemUpgrade _powerSystemUpgrade = default;
        [SerializeField] private ResourceScanner.Configuration _resourceScannerConfiguration = default;
        [SerializeField] private Storage.StorageUpgrade _storageUpgrade = default;
        [SerializeField] private Thrusters.ThrusterUpgrade _thrusterUpgrade = default;

        public static event Action OnChange;
        
        #region Properties

        public static Hull.HullUpgrade HullUpgrade
        {
            get => Instance._hullUpgrade;
            set
            {
                Instance._hullUpgrade = value;
                OnChange?.Invoke();
            }
        }

        public static InertiaDampers.InertiaDamperUpgrade InertiaDamperUpgrade
        {
            get => Instance._inertiaDamperUpgrade;
            set
            {
                Instance._inertiaDamperUpgrade = value;
                OnChange?.Invoke();
            }
        }

        public static MiningTool.MiningToolUpgrade MiningToolUpgrade
        {
            get => Instance._miningToolUpgrade;
            set
            {
                Instance._miningToolUpgrade = value;
                OnChange?.Invoke();
            }
        }

        public static PowerSystem.PowerSystemUpgrade PowerSystemUpgrade
        {
            get => Instance._powerSystemUpgrade;
            set
            {
                Instance._powerSystemUpgrade = value;
                OnChange?.Invoke();
            }
        }

        public static ResourceScanner.Configuration ResourceScannerConfiguration
        {
            get => Instance._resourceScannerConfiguration;
            set
            {
                Instance._resourceScannerConfiguration = value;
                OnChange?.Invoke();
            }
        }

        public static Storage.StorageUpgrade StorageUpgrade
        {
            get => Instance._storageUpgrade;
            set
            {
                Instance._storageUpgrade = value;
                OnChange?.Invoke();
            }
        }

        public static Thrusters.ThrusterUpgrade ThrusterUpgrade
        {
            get => Instance._thrusterUpgrade;
            set
            {
                Instance._thrusterUpgrade = value;
                OnChange?.Invoke();
            }
        }

        #endregion

        
        public static void Clear() {
            Instance._hullUpgrade = null;
            Instance._inertiaDamperUpgrade = null;
            Instance._miningToolUpgrade = null;
            Instance._powerSystemUpgrade = null;
            Instance._resourceScannerConfiguration = null;
            Instance._storageUpgrade = null;
            Instance._thrusterUpgrade = null;
            OnChange?.Invoke();
        }

        private void Reset() => gameObject.name = GetType().Name;

        private void OnValidate() => OnChange?.Invoke();
    }
}