using System.Numerics;
using SpaceGame.AsteroidSystem;
using SpaceGame.Message_UI_System;
using SpaceGame.ShuttleSystems.MiningTool.VFX;
using SpaceGame.Utility;
using UnityEngine;
using UnityEngine.Assertions;
using Vector3 = UnityEngine.Vector3;

namespace SpaceGame.ShuttleSystems.MiningTool {
    [AddComponentMenu("ShuttleSystems/MiningTool")]
    public class MiningTool : MonoBehaviour {
        #region Serialized fields
        [SerializeField] private Transform _origin = default;
        [SerializeField] private DefaultMiningTool _defaultMiningTool = new DefaultMiningTool();
        [SerializeField] private LayerMask _layerMask = default;
        #endregion

        #region Properties
        public IReadonlyObservable<bool> InRange => _inRange;
        public readonly IObservable<IMiningToolConfiguration> Upgrade = new Observable<IMiningToolConfiguration>();
        #endregion

        #region Private variables
        private static readonly Vector3 ScreenCenter = new Vector3(0.5f, 0.5f);
        private readonly Observable<GameObject> _targetGameObject = new Observable<GameObject>();
        private readonly Observable<bool> _inRange = new Observable<bool>();
        private Camera _camera;
        private Transform _cameraTransform;
        private Ray _ray;
        private RaycastHit _hit;
        private Vector3 _laserTarget = Vector3.zero;
        private MiningToolVFX _vfx = default;
        private ResourceDeposit _resourceDeposit = null;
        private Shuttle _shuttle;
        #endregion

        #region Private properties
        private bool Use => _shuttle.ShuttleControls.Fire;
        private IMiningToolConfiguration CurrentMiningTool => Upgrade.Value ?? _defaultMiningTool;
        #endregion

        #region Unity hooks
        private void Awake() {
            _shuttle = GetComponent<Shuttle>();
            Upgrade.OnChange += upgrade => {
                if (_vfx) Destroy(_vfx.gameObject);
                _vfx = Instantiate(CurrentMiningTool.VFXPrefab, _origin);
            };
            
            _camera = Camera.main;
            Assert.IsNotNull(_camera);
            _cameraTransform = _camera.transform;
            _targetGameObject.OnChange += target => _resourceDeposit = target ? target.GetComponentInParent<ResourceDeposit>() : null;  
        }

        private void Start() {
            if (!_vfx) _vfx = Instantiate(CurrentMiningTool.VFXPrefab, _origin);
        }

        private void OnDisable() {
            UpdateInRange();
            UpdateVFX();
        }

        private void Reset() => _origin = transform;
        #endregion

        
        private void UpdateTarget()
        {
            _ray = _camera.ViewportPointToRay(ScreenCenter);
            var range = CurrentMiningTool.Range;
            var hit = Physics.Raycast(_ray, out _hit, range, _layerMask);
            _targetGameObject.Value = hit ? _hit.collider.gameObject : null;
            _laserTarget = hit ? _hit.point : _cameraTransform.position + _cameraTransform.forward * range;
        }

        private void UpdateInRange() {
            _inRange.Value = enabled && _resourceDeposit && Vector3.Distance(_origin.position, _laserTarget) < CurrentMiningTool.Range;
        }

        private void UpdateVFX() {
            if (!_vfx) return;
            var isFiringLaser = enabled && Use && !_shuttle.PowerSystem.IsEmpty;
            _vfx.IsMining = isFiringLaser;
            _vfx.TargetPosition = _laserTarget;
            _vfx.IsHitting = isFiringLaser && _inRange.Value;
        }

        private void Update()
        {
            UpdateTarget();
            UpdateInRange();
            UpdateVFX();
            if (!Use || _shuttle.PowerSystem.IsEmpty) return;
            _shuttle.PowerSystem.Charge -= CurrentMiningTool.PowerConsumption * Time.deltaTime;
            if (!_inRange.Value || !_resourceDeposit.Damage(CurrentMiningTool.Strength * Time.deltaTime)) return;
            var resourceAcquired = _shuttle.Storage.Inventory.Add(_resourceDeposit.Type, _resourceDeposit.Amount);
            Broker.Push(resourceAcquired < 1
                ? "Shuttle inventory full!"
                : $"+{resourceAcquired} {_resourceDeposit.Type.Name}");
        }


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