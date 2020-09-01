using SpaceGame.Core;
using SpaceGame.ShuttleSystems.MiningTool.VFX;
using SpaceGame.Utility;
using SpaceGame.Utility.SaveSystem;
using UnityEngine;
using UnityEngine.Assertions;

namespace SpaceGame.ShuttleSystems.MiningTool {
    [AddComponentMenu("ShuttleSystems/MiningTool")]
    public class MiningTool : MonoBehaviour, IPersistable {
        #region Serialized fields
        [SerializeField] private Transform _origin = default;
        [SerializeField] private DefaultMiningTool _defaultMiningTool = new DefaultMiningTool();
        [SerializeField] private LayerMask _layerMask = default;
        #endregion

        #region Public variables
        public event System.Action<ItemType, uint> OnResourceAcquired;
        #endregion 
        
        #region Properties
        public IReadonlyObservable<bool> ResourceDepositInRange => _resourceDepositInRange;
        public readonly IObservable<MiningToolUpgrade> Upgrade = new Observable<MiningToolUpgrade>();
        #endregion

        #region Private variables
        private static readonly Vector3 ScreenCenter = new Vector3(0.5f, 0.5f);
        private readonly Observable<GameObject> _targetGameObject = new Observable<GameObject>();
        private readonly Observable<bool> _resourceDepositInRange = new Observable<bool>();
        private Camera _camera;
        private Transform _cameraTransform;
        private Ray _ray;
        private RaycastHit _hit;
        private Vector3 _laserTarget = Vector3.zero;
        private MiningToolVFX _vfx = default;
        private ResourceDeposit _resourceDeposit = null;
        private Shuttle _shuttle;
        private IMiningToolConfiguration _currentMiningTool;
        #endregion

        #region Private properties
        private bool Use => _shuttle.ShuttleControls.Fire;
        #endregion

        #region Unity hooks
        private void Awake() {
            _shuttle = GetComponent<Shuttle>();
            _camera = Camera.main;
            Assert.IsNotNull(_camera);
            _cameraTransform = _camera.transform;
            _targetGameObject.OnChange += target => _resourceDeposit = target ? target.GetComponentInParent<ResourceDeposit>() : null;
        }

        private void OnUpgradeChange(MiningToolUpgrade upgrade)
        {
            _currentMiningTool = upgrade ? (IMiningToolConfiguration) upgrade : _defaultMiningTool;
            if (_vfx) Destroy(_vfx.gameObject);
            _vfx = Instantiate(_currentMiningTool.VFXPrefab, _origin);
        }

        private void OnEnable() => Upgrade.Subscribe(OnUpgradeChange);

        private void OnDisable() {
            UpdateVFX();
            
            Upgrade.Unsubscribe(OnUpgradeChange);
        }

        private void Reset() => _origin = transform;
        #endregion

        private void UpdateTarget()
        {
            _ray = _camera.ViewportPointToRay(ScreenCenter);
            var range = _currentMiningTool.Range;
            var hit = Physics.Raycast(_ray, out _hit, range, _layerMask);
            _targetGameObject.Value = hit ? _hit.collider.gameObject : null;
            _laserTarget = hit ? _hit.point : _cameraTransform.position + _cameraTransform.forward * range;
        }

        private void UpdateVFX() {
            var isHittingGameobject = enabled && _targetGameObject.Value && Vector3.Distance(_origin.position, _laserTarget) < _currentMiningTool.Range;
            _resourceDepositInRange.Value = isHittingGameobject && _resourceDeposit;

            if (!_vfx) return;
            var isFiringLaser = enabled && Use && !_shuttle.PowerSystem.IsEmpty;
            _vfx.IsMining = isFiringLaser;
            _vfx.TargetPosition = _laserTarget;
            _vfx.IsHitting = isFiringLaser && isHittingGameobject;
        }

        private void Update()
        {
            UpdateTarget();
            UpdateVFX();
            if (!Use || _shuttle.PowerSystem.IsEmpty) return;
            _shuttle.PowerSystem.Consume(_currentMiningTool.PowerConsumption * Time.deltaTime);
            if (!_resourceDepositInRange.Value || !_resourceDeposit.Damage(_currentMiningTool.Strength * Time.deltaTime)) return;
            var resourceAcquired = _shuttle.Storage.Inventory.Add(_resourceDeposit.Type, _resourceDeposit.Amount);
            OnResourceAcquired?.Invoke(_resourceDeposit.Type, resourceAcquired);
        }

        #region IPersistable
        [System.Serializable]
        public class PersistentData
        {
            public readonly string MiningToolUpgradeName;

            public MiningToolUpgrade MiningToolUpgrade =>
                ItemType.GetByName<MiningToolUpgrade>(MiningToolUpgradeName);

            public PersistentData(MiningToolUpgrade miningToolUpgrade) =>
                MiningToolUpgradeName = miningToolUpgrade != null ? miningToolUpgrade.Name : null;
        }

        public object CaptureState() => new PersistentData(Upgrade.Value);

        public void RestoreState(object state)
        {
            var persistentData = (PersistentData) state;
            Upgrade.Set(persistentData.MiningToolUpgrade);
        }
        #endregion

        [System.Serializable]
        private class DefaultMiningTool: IMiningToolConfiguration {
            [Header("Laser parameters")]
            [SerializeField] private float _strength = 10f;
            [SerializeField] private float _range = 50f;
            [SerializeField] private float _powerConsumption = 2f;

            [Header("VFX")]
            [SerializeField] private MiningToolVFX _vfxPrefab = default;

            public float Strength => _strength;
            public float Range => _range;
            public float PowerConsumption => _powerConsumption;
            public MiningToolVFX VFXPrefab => _vfxPrefab;
        }
    }
}