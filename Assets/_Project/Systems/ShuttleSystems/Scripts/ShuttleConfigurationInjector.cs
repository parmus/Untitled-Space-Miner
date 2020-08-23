using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace SpaceGame.ShuttleSystems {
    public class ShuttleConfigurationInjector : MonoBehaviour
    {
        [SerializeField] private bool _singleUse = true;
        [Header("Shuttle configuration")]
        [SerializeField] private Hull.HullUpgrade _hullUpgrade = default;
        [SerializeField] private InertiaDampers.InertiaDamperUpgrade _inertiaDamperUpgrade = default;
        [SerializeField] private MiningTool.MiningToolUpgrade _miningToolUpgrade = default;
        [SerializeField] private PowerSystem.PowerSystemUpgrade _powerSystemUpgrade = default;
        [SerializeField] private ResourceScanner.Configuration _resourceScannerConfiguration = default;
        [SerializeField] private Storage.StorageUpgrade _storageUpgrade = default;
        [SerializeField] private Thrusters.ThrusterUpgrade _thrusterUpgrade = default;

        private void Start() => ShuttleSpawner.CurrentShuttle.Subscribe(OnNewShuttle);

        private void OnNewShuttle(Shuttle shuttle)
        {
            if (!shuttle) return;
            
            shuttle.Hull.Upgrade.Set(_hullUpgrade);
            shuttle.InertiaDampers.Upgrade.Set(_inertiaDamperUpgrade);
            shuttle.MiningTool.Upgrade.Set(_miningToolUpgrade);
            shuttle.PowerSystem.Upgrade.Set(_powerSystemUpgrade);
            shuttle.ResourceScanner.Configuration.Set(_resourceScannerConfiguration);
            shuttle.Storage.Upgrade.Set(_storageUpgrade);
            shuttle.Thrusters.Upgrade.Set(_thrusterUpgrade);

            if (_singleUse) ShuttleSpawner.CurrentShuttle.Unsubscribe(OnNewShuttle);
        }

        private void OnValidate()
        {
            if (!Application.isPlaying) return;

            var shuttle = ShuttleSpawner.CurrentShuttle.Value;
            if (!shuttle) return;
            
            shuttle.Hull.Upgrade.Set(_hullUpgrade);
            shuttle.InertiaDampers.Upgrade.Set(_inertiaDamperUpgrade);
            shuttle.MiningTool.Upgrade.Set(_miningToolUpgrade);
            shuttle.PowerSystem.Upgrade.Set(_powerSystemUpgrade);
            shuttle.ResourceScanner.Configuration.Set(_resourceScannerConfiguration);
            shuttle.Storage.Upgrade.Set(_storageUpgrade);
            shuttle.Thrusters.Upgrade.Set(_thrusterUpgrade);
        }

        private void Reset()
        {
            gameObject.name = GetType().Name;
            gameObject.tag = "EditorOnly";
        }
        
        [MenuItem("GameObject/Shuttle Configuration Injector", false, 10)]
        private static void CreateShuttleConfigurationInjector(MenuCommand menuCommand)
        {
            var go = new GameObject();
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            go.AddComponent<ShuttleConfigurationInjector>();
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }
    }
}
#endif
